using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Autofac;
using Blazored.LocalStorage;
using EasyNow.ApiClient;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace EasyNow.Web
{
    public class Program
    {
        private static WebAssemblyHost _host;
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddAntDesign();
            builder.Services.AddRefitClient<IEasyNowApi>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("http://localhost:5001");
            }).AddHttpMessageHandler<JwtDelegatingHandler>();
            builder.Services.AddScoped<JwtDelegatingHandler>();
            builder.Services.AddBlazoredLocalStorage(opts =>
            {
                opts.JsonSerializerOptions.WriteIndented = true;
            });

            _host = builder.Build();

            await _host.RunAsync();
        }
    }

    public class JwtDelegatingHandler : DelegatingHandler
    {
        private IServiceProvider _serviceProvider;

        public JwtDelegatingHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _serviceProvider.GetService<ILocalStorageService>().GetItemAsStringAsync("token").AsTask());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
