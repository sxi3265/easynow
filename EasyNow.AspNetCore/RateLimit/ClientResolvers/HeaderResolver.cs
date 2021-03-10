using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace EasyNow.AspNetCore.RateLimit.ClientResolvers
{
    public class HeaderResolver : IClientResolver
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        
        private readonly RateLimitOptions _options;

        public HeaderResolver(RateLimitOptions options)
        {
            _options = options;
        }

        public Dictionary<string, string> ResolveClient()
        {
            var clientTypes=_options.Policies.Where(e => !string.IsNullOrEmpty(e.ClientType) && e.ClientType.StartsWith("Header:")).Select(e=>e.ClientType).Distinct().ToArray();
            var dic = new Dictionary<string,string>();
            foreach (var clientType in clientTypes)
            {
                // 移除Header:
                var headerName = clientType.Remove(0, 7);
                if (!string.IsNullOrEmpty(headerName))
                {
                    if (HttpContextAccessor.HttpContext.Request.Headers.TryGetValue(headerName, out var values) && values.Any() &&
                        !string.IsNullOrEmpty(values.First()))
                    {
                        dic.Add(clientType, values.First());
                    }
                }
            }

            return dic;
        }
    }
}