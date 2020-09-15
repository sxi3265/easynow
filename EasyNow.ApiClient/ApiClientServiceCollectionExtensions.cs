using System;
using EasyNow.ApiClient.WxPusher;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiClientServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClient(this IServiceCollection services)
        {
            var settings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                })
            };
            services.AddRefitClient<IWxPusher>(settings).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("http://wxpusher.zjiecode.com");
                c.Timeout = TimeSpan.FromDays(1);
            });
            return services;
        }
    }
}