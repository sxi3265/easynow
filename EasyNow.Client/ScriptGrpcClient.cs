using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;
using EasyNow.Utility.Extensions;

namespace EasyNow.Client
{
    public class ScriptGrpcClient: IScriptClient
    {
        private readonly Script.ScriptClient _client;

        public ScriptGrpcClient(Script.ScriptClient client)
        {
            _client = client;
        }

        public async Task<IPagedList<ScriptInfo>> QueryAsync(ScriptQueryModel model)
        {
            return (await _client.QueryAsync(model.To<ScriptQueryReq>())).To<PagedList<ScriptInfo>>();
        }
    }
}
