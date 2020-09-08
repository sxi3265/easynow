using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNow.Dto;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;

namespace EasyNow.Bo.Abstractions
{
    public interface IScriptBo
    {
        Task<IPagedList<ScriptInfo>> QueryAsync(ScriptQueryModel model);
    }
}