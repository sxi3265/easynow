using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EasyNow.AspNetCore.RateLimit.ClientResolvers
{
    public class IdentityResolver : IClientResolver
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        
        public Dictionary<string, string> ResolveClient()
        {
            return new Dictionary<string, string>
            {
                {"Identity", HttpContextAccessor.HttpContext.User?.Identity?.Name}
            };
        }
    }
}