using System;
using System.IO;
using System.Threading.Tasks;
using EasyNow.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace EasyNow.Job.Smzdm
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var launchOptions = new LaunchOptions
            {
                Headless = false,
                // Args = new []{"--no-sandbox"},
                IgnoredDefaultArgs = new []{"--enable-automation"},
                DefaultViewport = new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                }
            };
            var browser = await Puppeteer.LaunchAsync(launchOptions);
            var page = await browser.NewPageAsync();
            await page.SetUserAgentAsync(Configuration["UserAgent"]);
            await page.SetCookieAsync(Configuration["Cookies"].FromJson<CookieParam[]>());
            await page.GoToAsync("https://www.smzdm.com/");
            var element =await page.WaitForSelectorAsync("a.J_punch");
            var text=await (await element.GetPropertyAsync("textContent")).JsonValueAsync<string>();
            // var b=await page.EvaluateFunctionAsync<string>("e=>e.textContent",element);
            if (text == "签到领奖")
            {
                await element.ClickAsync();
            }
        }
    }
}