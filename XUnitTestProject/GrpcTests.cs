using System.Threading.Tasks;
using Grpc.Net.Client;
using Xunit;

namespace XUnitTestProject
{
    public class GrpcTests
    {
        [Fact]
        public async Task Test1()
        {
            using var channel = GrpcChannel.ForAddress("https://api.easynow.me");
            var client = new Script.ScriptClient(channel);
            var reply = await client.QueryAsync(new ScriptQueryReq());
        }
    }
}