using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.Script;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Bo
{
    public class ScriptBo:BaseBo,IScriptBo
    {
        public IEnumerable<ScriptInfo> Query(ScriptQueryModel model)
        {
            var query = Db.Script.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(model.Name), e => e.Name.Contains(model.Name));
            return query.ProjectTo<ScriptInfo>(Scope.Resolve<IConfigurationProvider>()).AsEnumerable();
        }
    }
}
