using System.Collections.Generic;

namespace EasyNow.AspNetCore.RateLimit
{
    public interface IClientResolver
    {
        Dictionary<string,string> ResolveClient();
    }
}