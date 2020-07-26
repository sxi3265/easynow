using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EasyNow.AutoJsServer
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
            PathString path,
            WebSocketHandler handler)
        {
            return app.Map(path, (a) => a.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
    }
}