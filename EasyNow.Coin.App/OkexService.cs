using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EasyNow.Coin.Bo;
using Microsoft.Extensions.Hosting;

namespace EasyNow.Coin.App
{
    public class OkexService : IHostedService
    {
        private readonly ILifetimeScope _lifetimeScope;
        public OkexService(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async() =>
            {
                await _lifetimeScope.Resolve<IRule>().RunAsync();
            },TaskCreationOptions.LongRunning);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}