using System.Linq;
using System.Threading.Tasks;
using KubeClient;
using Xunit;

namespace XUnitTestProject
{
    public class KubeTests
    {
        [Fact]
        public async Task Test1()
        {
            KubeClientOptions clientOptions = K8sConfig.Load("kubeconfig").ToKubeClientOptions(defaultKubeNamespace: "kube-system");
            clientOptions.AllowInsecure = true;

            KubeApiClient client = KubeApiClient.Create(clientOptions);
            //(await client.PodsV1().Get("test"))
            //var a=await client.DeploymentsV1().Get("test","default");


            await client.DeploymentsV1().Update("test", p =>
            {
                p.Replace(d => d.Spec.Paused, false);
                //d.Replace(d=>d.Sta)
                //d.Replace(d=>d.Spec.Template.)
            },"default");
        }
    }
}