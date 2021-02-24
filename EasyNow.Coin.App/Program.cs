using System.IO;
using Autofac.Extensions.DependencyInjection;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Okex.Net;
using Okex.Net.CoreObjects;

namespace EasyNow.Coin.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureHostConfiguration(config =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("hostsettings.json",true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json",true,true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                })
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}
