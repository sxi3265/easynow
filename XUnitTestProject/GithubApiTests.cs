using System.Linq;
using System.Threading.Tasks;
using Octokit;
using Xunit;

namespace XUnitTestProject
{
    public class GithubApiTests
    {
        [Fact]
        public async Task Test1()
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue("EasyNow"));
            var repo=await gitHubClient.Repository.Release.GetLatest("frida","frida");
            var assets=repo.Assets.ToArray();
        }
    }
}