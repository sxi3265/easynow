using System.Collections.Generic;
using EasyNow.Dto;
using EasyNow.Dto.Script;

namespace EasyNow.Bo.Abstractions
{
    public interface IScriptBo
    {
        IEnumerable<ScriptInfo> Query(ScriptQueryModel model);
    }
}