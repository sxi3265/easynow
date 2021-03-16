using System;
using System.Linq;
using System.Reflection;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using Autofac;
using EasyCaching.Core.Configurations;
using EasyCaching.Core.Interceptor;
using EasyCaching.Interceptor.AspectCore;
using Microsoft.Extensions.Options;

namespace EasyNow.Utility.Extensions
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// Add the AspectCore interceptor.
        /// </summary>
        public static void AddEasyCachingAspectCoreInterceptor(this ContainerBuilder builder, Action<EasyCachingInterceptorOptions> action)
        {
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>();

            builder.RegisterType<EasyCachingInterceptor>();

            var config = new EasyCachingInterceptorOptions();

            action(config);

            var options = Options.Create(config);

            builder.Register(x => options);

            builder.RegisterDynamicProxy(configure =>
            {
                bool all(MethodInfo x) => x.CustomAttributes.Any(data => typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType));

                configure.Interceptors.AddTyped<EasyCachingInterceptor>(all);
            });
        }
    }
}