using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace EasyNow.AspNetCore.RateLimit.ClientResolvers
{
    public class ConnectionIpResolver : IClientResolver
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dictionary<string, string> ResolveClient()
        {
            return new Dictionary<string, string>
            {
                {"ConnectionIp", HttpContextAccessor.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString()}
            };
        }
    }
}