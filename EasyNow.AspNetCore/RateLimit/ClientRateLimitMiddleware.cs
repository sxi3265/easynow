using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.Core;
using EasyNow.Utility.Extensions;
using Microsoft.AspNetCore.Http;

namespace EasyNow.AspNetCore.RateLimit
{
    public class ClientRateLimitMiddleware:IMiddleware
    {
        private readonly RateLimitOptions _options;
        private readonly IEnumerable<IClientResolver> _clientResolvers;
        private readonly IEasyCachingProvider _cachingProvider;

        public ClientRateLimitMiddleware(RateLimitOptions options, IEnumerable<IClientResolver> clientResolvers, IEasyCachingProvider cachingProvider)
        {
            _options = options;
            _clientResolvers = clientResolvers;
            _cachingProvider = cachingProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_options == null || !_options.Enable || !_options.Policies.Any())
            {
                await next(context);
                return;
            }

            var identity = GetRequestIdentity(context);

            var matchRules = GetMatchingRules(identity);

            var ruleCounterList = new List<KeyValuePair<Rule,Counter>>();

            foreach (var rule in matchRules)
            {
                var counters = GetCounters(identity,rule).ToArray();

                foreach (var counter in counters)
                {
                    if (rule.Limit > 0)
                    {
                        // 计数器已过期，则跳过
                        if (counter.StartTime + rule.Period < DateTime.UtcNow)
                        {
                            continue;
                        }

                        if (counter.Count > rule.Limit)
                        {
                            var diff = counter.StartTime + rule.Period - DateTime.UtcNow;
                            var retryAfter = Math.Max(diff.TotalSeconds, 1);
                            await BlockRequest(context,rule,retryAfter.ToString("F0"));
                            return;
                        }
                    }
                    else
                    {
                        await BlockRequest(context,rule,int.MaxValue.ToString());
                        return;
                    }
                    ruleCounterList.Add(new KeyValuePair<Rule, Counter>(rule,counter));
                }
            }

            if (ruleCounterList.Any() && !_options.DisableRateLimitHeaders)
            {
                // 找出次数剩余最少的规则和计数器
                var ruleCounter=ruleCounterList.OrderBy(e => e.Key.Limit - e.Value.Count).First();
                context.Response.OnStarting(obj =>
                {
                    var state = obj as dynamic;
                    if (state.Context is HttpContext c)
                    {
                        c.Response.Headers["X-RateLimit-Limit"]=state.Limit as string;
                        c.Response.Headers["X-RateLimit-Remaining"]=state.Remaining as string;
                        c.Response.Headers["X-RateLimit-Reset"]=state.Reset as string;
                    }

                    return Task.CompletedTask;
                },new
                {
                    Context=context,
                    Limit=ruleCounter.Key.Limit.ToString(),
                    Remaining=(ruleCounter.Key.Limit-ruleCounter.Value.Count).ToString(CultureInfo.InvariantCulture),
                    Reset=(ruleCounter.Value.StartTime+ruleCounter.Key.Period-DateTime.UtcNow).TotalSeconds.ToString("F0")
                });
            }
            await next(context);
        }
        
        private async Task BlockRequest(HttpContext context,Rule rule,string retryAfter)
        {
            if (!_options.DisableRateLimitHeaders)
            {
                context.Response.Headers["Retry-After"]=retryAfter;
            }

            // 优先级 规则>配置>默认值
            context.Response.StatusCode = rule.QuotaExceededResponse?.StatusCode??_options.QuotaExceededResponse?.StatusCode??429;
            context.Response.ContentType =rule.QuotaExceededResponse?.ContentType??_options.QuotaExceededResponse?.ContentType??"text/plain; charset=UTF-8";
            await context.Response.WriteAsync(rule.QuotaExceededResponse?.Content??_options.QuotaExceededResponse?.Content??$"调用超出配额!每{rule.Period.TotalSeconds:F0}秒允许{rule.Limit}次.");
        }

        private RequestIdentity GetRequestIdentity(HttpContext httpContext)
        {
            return new RequestIdentity
            {
                ClientIds = _clientResolvers.SelectMany(e=>e.ResolveClient()).Where(e=>!string.IsNullOrEmpty(e.Value)).ToDictionary(e=>e.Key,e=>e.Value),
                HttpPath = httpContext.Request.Path.ToString().ToLowerInvariant(),
                HttpMethod = httpContext.Request.Method.ToLowerInvariant()
            };
        }

        private IEnumerable<Counter> GetCounters(RequestIdentity identity,Rule rule)
        {
            foreach (var clientId in identity.ClientIds)
            {
                var key = $"RateLimit_{clientId.Key}:{clientId.Value}_{rule.Period}";
                lock (string.Intern(key))
                {
                    var counter = _cachingProvider.Get<Counter>(key).Value;

                    if (counter != null && counter.StartTime + rule.Period >= DateTime.UtcNow)
                    {
                        counter=new Counter
                        {
                            StartTime = counter.StartTime,
                            Count = counter.Count + 1
                        };
                    }
                    else
                    {
                        counter=new Counter
                        {
                            StartTime = DateTime.UtcNow,
                            Count = 1
                        };
                    }

                    _cachingProvider.Set(key,counter,rule.Period);
                    yield return counter;
                }
            }
        }

        private IEnumerable<Rule> GetMatchingRules(RequestIdentity identity)
        {
            var matchPolicies = _options.Policies.Where(e =>!string.IsNullOrEmpty(e.ClientType)&&!string.IsNullOrEmpty(e.ClientId)&&
                    identity.ClientIds.ContainsKey(e.ClientType) && e.ClientId.IsMatchWildcard(identity.ClientIds[e.ClientType],true)&&e.Rules.Any())
                .ToArray();
            var rules = GetAvailableRules(identity, matchPolicies).ToList();

            var generalPolicies = _options.Policies.Where(e => e.ClientType == null && e.ClientId == null).ToArray();
            if (generalPolicies.Any())
            {
                var matchRules = GetAvailableRules(identity, generalPolicies);

                // 通用规则优先级低
                foreach (var rule in matchRules)
                {
                    if (!rules.Exists(e => e.Period == rule.Period))
                    {
                        rules.Add(rule);
                    }
                }
            }

            return _options.StackBlockedRequests ? rules.OrderByDescending(e => e.Period) : rules.OrderBy(e => e.Period);
        }

        private IEnumerable<Rule> GetAvailableRules(RequestIdentity identity,ClientPolicy[] policies)
        {
            var rules=new List<Rule>();
            rules.AddRange(policies.SelectMany(e => e.Rules).Where(e=>e.Endpoint.IsMatchWildcard($"{identity.HttpMethod}:{identity.HttpPath}",true)).ToArray());
            
            // 相同周期中取限制数小的
            rules = rules.GroupBy(e => e.Period).Select(e => e.OrderBy(r => r.Limit)).Select(e => e.First()).ToList();

            // 取出基本规则，同样相同周期中取限制数小的
            var basicRules=policies.SelectMany(e => e.Rules).Where(e => e.Endpoint == "*").GroupBy(e=>e.Period).Select(e=>e.OrderBy(r=>r.Limit)).Select(e=>e.First()).ToArray();

            // 如果指定周期的规则没有，则使用基本规则
            foreach (var rule in basicRules)
            {
                if (!rules.Exists(e => e.Period == rule.Period))
                {
                    rules.Add(rule);
                }
            }

            return rules;
        }
    }
}