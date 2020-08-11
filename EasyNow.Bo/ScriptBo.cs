using EasyNow.Bo.Abstractions;
using EasyNow.Dal.Entities;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EasyNow.Bo
{
    public class ScriptBo:BaseBo,IScriptBo
    {
        public Task<IPagedList<ScriptInfo>> QueryAsync(ScriptQueryModel model)
        {
            var query = Db.Script.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(model.Name), e => e.Name.Contains(model.Name));
            return Task.FromResult(query.ToPagedList<Script, ScriptInfo>(model));
        }
    }
}
