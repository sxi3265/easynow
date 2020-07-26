using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyNow.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]"),Authorize]
    public abstract class ApiBaseController:ControllerBase
    {
        public ILifetimeScope Scope { get; set; }
    }
}