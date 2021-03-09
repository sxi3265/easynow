using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNow.Utility.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllDestinationVirtual<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var desType = typeof(TDestination);
            foreach (var property in desType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAllSourceVirtual<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var srcType = typeof(TSource);
            foreach (var property in srcType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForSourceMember(property.Name, opt => opt.DoNotValidate());
            }

            return expression;
        }

        public static IMappingExpression IgnoreAllDestinationVirtual(this IMappingExpression expression, Type desType)
        {
            foreach (var property in desType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            return expression;
        }

        public static IMappingExpression IgnoreAllSourceVirtual(this IMappingExpression expression, Type srcType)
        {
            foreach (var property in srcType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForSourceMember(property.Name, opt => opt.DoNotValidate());
            }

            return expression;
        }

        public static IMappingExpression IgnoreVirtual(this IMappingExpression expression, Type srcType, Type desType)
        {
            foreach (var property in desType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            foreach (var property in srcType.GetProperties().Where(p => p.GetGetMethod().IsVirtual && !p.GetGetMethod().IsFinal))
            {
                expression.ForSourceMember(property.Name, opt => opt.DoNotValidate());
            }

            return expression;
        }

        /// <summary>
        /// 注册AutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services,params Assembly[] assemblies)
        {
            var types=assemblies.SelectMany(e =>
            {
                try
                {
                    return e.GetTypes();
                }
                catch
                {
                    return new Type[0];
                }
            }).Where(e => typeof(Profile).GetTypeInfo().IsAssignableFrom(e.GetTypeInfo())).ToArray();
            UtilitySetup.profileTypes = types;
            services.AddSingleton(UtilitySetup.Mapper);
            return services;
        }
    }
}
