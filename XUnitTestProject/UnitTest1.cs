using System;
using Jint;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var engine = new Engine();
            var a = engine.Execute("Math.random()").GetCompletionValue().ToString();
        }
    }
}
