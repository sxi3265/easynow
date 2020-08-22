using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class WxController : ApiBaseController
    {
        [HttpPost,AllowAnonymous]
        public IActionResult WxPusherCallback()
        {
            return NoContent();
        }
    }
}