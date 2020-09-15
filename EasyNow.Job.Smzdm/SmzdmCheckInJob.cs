using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNow.Job.Smzdm
{
    public class SmzdmCheckInJob:IJob
    {
        private readonly ILogger _logger;

        private readonly IConfigurationRoot _configuration;
        
        private readonly IServiceProvider _serviceProvider;

        public SmzdmCheckInJob(ILogger<SmzdmCheckInJob> logger, IConfigurationRoot configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var args = new List<string>
            {
                "--no-sandbox",
                "--disable-setuid-sandbox"
            };
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                args.Add("--use-mock-keychain");
            }else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                args.Add("--disable-dev-shm-usage");
            }

            var launchOptions = new LaunchOptions
            {
                Headless = _configuration["Headless"].To<bool>(),
                Args = args.ToArray(),
                IgnoredDefaultArgs = new []{"--enable-automation"},
                DefaultViewport = new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                }
            };
            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            await page.SetUserAgentAsync(_configuration["UserAgent"]);
            await page.SetCookieAsync(_configuration["Cookies"].FromJson<CookieParam[]>());
            await page.GoToAsync("https://www.smzdm.com/",new NavigationOptions
            {
                Timeout = 0,
                WaitUntil = new []
                {
                    WaitUntilNavigation.Load,
                    WaitUntilNavigation.Networkidle2
                }
            });
            
            // 未登录状态，说明cookie过期
            if ((await page.QuerySelectorAsync("a.name-link"))== null)
            {
                var result=await _serviceProvider.GetService<IWxPusher>().SendMessage(new MessageReq
                {
                    AppToken = _configuration["AppToken"],
                    ContentType = ContentType.Text,
                    Content = @"什么值得买Cookie已过期,请更新Cookie",
                    Uids = new []{_configuration["Uid"]}
                });
                _logger.LogInformation($"什么值得买Cookie已过期,已发送微信消息,结果:{result.ToJson()}");
                return;
            }
            var element =await page.WaitForSelectorAsync("a.J_punch");
            var text=await (await element.GetPropertyAsync("textContent")).JsonValueAsync<string>();
            // var b=await page.EvaluateFunctionAsync<string>("e=>e.textContent",element);
            if (text == "签到领奖")
            {
                await element.ClickAsync();
                await Task.Delay(5000);
                element =await page.WaitForSelectorAsync("a.J_punch");
                text=await (await element.GetPropertyAsync("textContent")).JsonValueAsync<string>();
            }

            _logger.LogInformation(text);
        }
    }
}