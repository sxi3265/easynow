using System;
using System.IO;
using System.Threading.Tasks;
using EasyNow.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace EasyNow.Job.Smzdm
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                Configuration = builder.Build();
                var servicesProvider = BuildDi(Configuration);
                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<Runner>();
                    await runner.DoActionAsync();

                    // Console.WriteLine("Press ANY key to exit");
                    // Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
        
        private static IServiceProvider BuildDi(IConfigurationRoot config)
        {
            return new ServiceCollection()
                .AddTransient<Runner>()
                .AddSingleton(config)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })
                .BuildServiceProvider();
        }
    }
    
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        private readonly IConfigurationRoot _configuration;

        public Runner(ILogger<Runner> logger, IConfigurationRoot configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task DoActionAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var launchOptions = new LaunchOptions
            {
                Headless = _configuration["Headless"].To<bool>(),
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
            await page.SetUserAgentAsync(_configuration["UserAgent"]);
            await page.SetCookieAsync(_configuration["Cookies"].FromJson<CookieParam[]>());
            await page.GoToAsync("https://www.smzdm.com/",new NavigationOptions
            {
                Timeout = 0,
                WaitUntil = new []
                {
                    WaitUntilNavigation.Load
                }
            });
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