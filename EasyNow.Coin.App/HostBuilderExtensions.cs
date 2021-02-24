using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EasyNow.Coin.App
{
    /// <summary>
    /// Extensions to emulate a typical "Startup.cs" pattern for <see cref="IHostBuilder"/>
    /// </summary>
    public static class HostBuilderExtensions
    {
        private const string ConfigureServicesMethodName = "ConfigureServices";
        private const string ConfigureContainerMethodName = "ConfigureContainer";

        /// <summary>
        /// Specify the startup type to be used by the host.
        /// </summary>
        /// <typeparam name="TStartup">The type containing an optional constructor with
        /// an <see cref="IConfiguration"/> parameter. The implementation should contain a public
        /// method named ConfigureServices with <see cref="IServiceCollection"/> parameter.</typeparam>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to initialize with TStartup.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder) where TStartup : class
        {
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                var cfgServicesMethod = typeof(TStartup).GetMethod(
                    ConfigureServicesMethodName, new Type[] { typeof(IServiceCollection) });

                var hasConfigCtor = typeof(TStartup).GetConstructor(
                    new Type[] { typeof(IConfiguration) }) != null;

                var startUpObj = hasConfigCtor ?
                    (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration) :
                    (TStartup)Activator.CreateInstance(typeof(TStartup), null);

                ctx.Properties["startup"] = startUpObj;

                cfgServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
            });

            hostBuilder.ConfigureContainer<ContainerBuilder>((ctx, containerBuilder) =>
            {
                var startUpObj = ctx.Properties["startup"];
                var methodInfo = typeof(TStartup).GetMethod(
                    ConfigureContainerMethodName, new Type[] { typeof(ContainerBuilder) });
                methodInfo?.Invoke(startUpObj, new object[] { containerBuilder });
            });

            return hostBuilder;
        }
    }
}