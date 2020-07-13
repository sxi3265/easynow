using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;
using Esprima.Ast;
using Jint;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var pagedList = new PagedList<ScriptInfo>(new []{new ScriptInfo
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Content = "ssss"
            } },new Pagination
            {
                PageNumber = 1,
                PageSize = 2
            });

            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
            //options.Converters.Add(new PagedListConverter());
            var json = JsonSerializer.Serialize(pagedList,options);
        }
    }
}
