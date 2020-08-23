using System.IO;
using System.Threading.Tasks;
using Autofac;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.WxPusher;
using EasyNow.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class WxController : ApiBaseController
    {
        private IWxBo WxBo => Scope.Resolve<IWxBo>();

        [HttpPost,AllowAnonymous]
        public async Task WxPusherCallback([FromBody]Req req)
        {
            Scope.Resolve<ILogger<WxController>>().LogInformation($"WxPusherCallback:{req.ToJson()}");
            await WxBo.WxPusherUserSubscribeAsync(req.Data.AppKey,req.Data.To<UserDto>());
        }
    }
}