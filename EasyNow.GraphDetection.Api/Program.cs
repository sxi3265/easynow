using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;

namespace EasyNow.GraphDetection.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var aspnetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configFileName = $"nlog.{aspnetEnvironment}.config";
            if (!File.Exists(configFileName))
            {
                configFileName = "nlog.config";
            }
            var logger = NLogBuilder.ConfigureNLog(configFileName).GetCurrentClassLogger();
            try
            {
                logger.Debug("初始化主程序");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "程序异常停止");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory());
                            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                            config.AddJsonFile(
                                $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                                reloadOnChange: true,
                                optional: true);
                            config.AddCommandLine(args);
                            config.AddEnvironmentVariables();

                            if (hostingContext.HostingEnvironment.IsDevelopment())
                            {
                                config.AddUserSecrets<Startup>();
                            }
                        })
                        .UseStartup<Startup>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddConsole();
                    }
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
