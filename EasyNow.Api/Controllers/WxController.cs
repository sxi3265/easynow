using System.IO;
using System.Threading.Tasks;
using Autofac;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Bo.Abstractions;
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
        public async Task WxPusherCallback(Dto.WxPusher.Req req)
        {
            await WxBo.WxPusherUserSubscribeAsync(req.Data.AppKey,req.Data.Uid);
        }
    }
}