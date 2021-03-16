using System.Collections.Generic;

namespace EasyNow.AspNetCore.RateLimit
{
    public class RequestIdentity
    {
        public IDictionary<string, string> ClientIds { get; set; }

        public string HttpPath { get; set; }

        public string HttpMethod { get; set; }
    }
}