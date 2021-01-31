using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Server;

namespace EasyNow.GraphDetection.Api
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCodeFirstGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Fastest;
                config.MaxSendMessageSize = null;
                config.MaxReceiveMessageSize = null;
            });
            services.AddCodeFirstGrpcReflection();
        }

        /// <summary>
        /// autofac×¢²á
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder builder)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
