using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto;
using EasyNow.Dto.Script;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyNow.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ScriptController : ControllerBase
    {
        public ILifetimeScope Scope { get; set; }
        private IScriptBo ScriptBo => Scope.Resolve<IScriptBo>();

        [HttpGet]
        public IEnumerable<ScriptInfo> Query([FromQuery]ScriptQueryModel model)
        {
            return ScriptBo.Query(model);
        }
    }
}
