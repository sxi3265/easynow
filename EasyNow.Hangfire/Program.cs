using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;

namespace EasyNow.Hangfire
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static BackgroundJobServer BackgroundJobServer { get; set; }
        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(GracefulServerShutdown);
            ConfigureApplication();

            await Console.Out.WriteLineAsync("Hangfire Client finished its work. Press return to exit...");
            await Console.In.ReadLineAsync();
        }

        private static void ConfigureApplication()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            GlobalConfiguration.Configuration.UseStorage(
                new MySqlStorage(
                    Configuration.GetConnectionString("Hangfire"),
                    new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 50000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire"
                    }));

            BackgroundJobServer = new BackgroundJobServer();
        }

        private static void GracefulServerShutdown(object sender, EventArgs e)
        {
            BackgroundJobServer.SendStop();
            BackgroundJobServer.Dispose();
        }
    }
}
