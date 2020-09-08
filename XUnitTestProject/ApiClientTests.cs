using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNow.ApiClient.WxPusher;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using Xunit;

namespace XUnitTestProject
{
    public class ApiClientTests
    {
        
        [Fact]
        public async Task TestWxPusher()
        {
            var services=new ServiceCollection();
            var settings = new RefitSettings();
            settings.ContentSerializer =new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            services.AddRefitClient<IWxPusher>(settings).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("http://wxpusher.zjiecode.com");
            });

            var serviceProvider = services.BuildServiceProvider();
            var wxPusher = serviceProvider.GetService<IWxPusher>();
            await wxPusher.SendMessage(new MessageReq
            {
                AppToken = "AT_A2cAsKKucEBs2QWc6ZFQQx3bclfj7tfr",
                ContentType = ContentType.Markdown,
                Content = @"# 一级标题
## 二级标题
### 三级标题
#### 四级标题
##### 五级标题
###### 六级标题",
                Uids = new []{"UID_HfgFMB2bmZ0vzumuoJlAvOPj86Yg"}
            });
        }

        [Fact]
        public async Task QueryUser()
        {
            var services = new ServiceCollection();
            var settings = new RefitSettings();
            settings.ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            services.AddRefitClient<IWxPusher>(settings).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("http://wxpusher.zjiecode.com");
            });

            var serviceProvider = services.BuildServiceProvider();
            var wxPusher = serviceProvider.GetService<IWxPusher>();
            var result = await wxPusher.QueryUser(new QueryUserReq
            {
                AppToken = "AT_A2cAsKKucEBs2QWc6ZFQQx3bclfj7tfr",
                PageSize = int.MaxValue,
                Page = 1
            });
            Assert.True(result.Data?.Records.Any(e=>e.Uid== "UID_HfgFMB2bmZ0vzumuoJlAvOPj86Yg"));
        }
    }
}