using System.IO;
using System.Threading.Tasks;
using Autofac;
using EasyNow.ApiClient.WxPusher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class WxController : ApiBaseController
    {
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> WxPusherCallback()
        {
            using var sr = new StreamReader(this.Request.Body);
            Scope.Resolve<ILogger<WxController>>().LogInformation(await sr.ReadToEndAsync());
            return NoContent();
        }
    }
}