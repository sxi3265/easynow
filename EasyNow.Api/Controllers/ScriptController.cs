using System.Threading.Tasks;
using Autofac;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;
using Microsoft.AspNetCore.Mvc;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class ScriptController : ApiBaseController
    {
        private IScriptBo ScriptBo => Scope.Resolve<IScriptBo>();

        [HttpGet]
        public Task<IPagedList<ScriptInfo>> Query([FromQuery]ScriptQueryModel model)
        {
            return ScriptBo.QueryAsync(model);
        }
    }
}
