using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNow.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace EasyNow.AspNetCore.RateLimit.ClientResolvers
{
    public class JwtResolver:IClientResolver
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        
        private readonly RateLimitOptions _options;

        public JwtResolver(RateLimitOptions options)
        {
            _options = options;
        }

        public Dictionary<string, string> ResolveClient()
        {
            var dic = new Dictionary<string,string>();
            if (!HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization",out var authorizationValues) || !authorizationValues.Any())
            {
                return dic;
            }

            var authorization = authorizationValues.First();
            if (string.IsNullOrEmpty(authorization)||!authorization.StartsWith("Bearer ",StringComparison.InvariantCultureIgnoreCase))
            {
                return dic;
            }

            var token = authorization.Remove(0, 7);
            var strs=token.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length != 3)
            {
                return dic;
            }
            // 修复base64字符串
            var payload = Encoding.UTF8.GetString(Convert.FromBase64String(strs[1].ToFixedBase64Str()));
            var jObject = JObject.Parse(payload);

            var clientTypes=_options.Policies.Where(e => !string.IsNullOrEmpty(e.ClientType) && e.ClientType.StartsWith("Jwt:")).Select(e=>e.ClientType).Distinct().ToArray();
            foreach (var clientType in clientTypes)
            {
                // 移除Jwt:
                var name = clientType.Remove(0, 4);
                if (!string.IsNullOrEmpty(name))
                {
                    dic.Add(clientType,jObject.GetValue(name).ToString());
                }
            }

            return dic;
        }
    }
}