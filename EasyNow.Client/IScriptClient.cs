using System.Threading.Tasks;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;

namespace EasyNow.Client
{
    public interface IScriptClient
    {
        Task<IPagedList<ScriptInfo>> QueryAsync(ScriptQueryModel model);
    }
}