using System.Threading.Tasks;
using Grpc.Net.Client;
using Xunit;

namespace XUnitTestProject
{
    public class GrpcTests
    {
        private readonly GrpcChannel _channel;

        public GrpcTests()
        {
            _channel= GrpcChannel.ForAddress("https://api.easynow.me");
        }

        [Fact]
        public async Task Test1()
        {
            var client = new Script.ScriptClient(_channel);
            var reply = await client.QueryAsync(new ScriptQueryReq());
        }

        [Fact]
        public async Task Test2()
        {
            var client = new User.UserClient(_channel);
            var result = await client.LoginAsync(new UserLoginReq
            {
                Account = "sxi3265",
                Password = "sbxaialhj"
            });
        }
    }
}