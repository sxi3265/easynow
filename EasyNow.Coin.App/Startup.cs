using System.Linq;
using Autofac;
using EasyNow.Coin.Bo;
using EasyNow.Coin.Dal;
using EasyNow.Coin.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNow.Coin.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OkexOptions>(Configuration.GetSection("OkexConfig"));
            services.AddDbContext<CoinContext>(builder =>
            {
                builder.UseMySql(Configuration.GetConnectionString("CoinDb"),ServerVersion.AutoDetect(Configuration.GetConnectionString("CoinDb")));
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<OkexService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Rule1>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterType<Rule2>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}