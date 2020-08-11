using System.Threading.Tasks;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.Script;
using EasyNow.Utility.Extensions;
using Grpc.Core;

namespace EasyNow.Api.Services
{
    public class ScriptService:Script.ScriptBase
    {
        private readonly IScriptBo _scriptBo;

        public ScriptService(IScriptBo scriptBo)
        {
            _scriptBo = scriptBo;
        }

        public override async Task<ScriptQueryResp> Query(ScriptQueryReq request, ServerCallContext context)
        {
            return (await _scriptBo.QueryAsync(request.To<ScriptQueryModel>())).To<ScriptQueryResp>();
        }
    }
}