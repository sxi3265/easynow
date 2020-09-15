using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

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
                    var runner = servicesProvider.GetRequiredService<SmzdmCheckInJob>();
                    await runner.ExecuteAsync();

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
                .AddTransient<SmzdmCheckInJob>()
                .AddSingleton(config)
                .AddApiClient()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                    loggingBuilder.AddConsole();
                })
                .BuildServiceProvider();
        }
    }
}