using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CryptoExchange.Net.Objects;
using EasyNow.Coin.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okex.Net;
using Okex.Net.CoreObjects;
using Okex.Net.Enums;
using Okex.Net.RestObjects;

namespace EasyNow.Coin.Bo
{
    public class Rule2:IRule
    {
        private readonly OkexSocketClient _socketClient;
        private ILifetimeScope _lifetimeScope;
        private IOptionsMonitor<OkexOptions> _optionsMonitor;
        private OkexClient _client;
        private List<OkexSpotTrade> _buyTrades = new ();
        private List<OkexSpotTrade> _sellTrades = new();
        private ILogger _logger => _lifetimeScope.Resolve<ILogger<Rule2>>();
        public Rule2(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _optionsMonitor = _lifetimeScope.Resolve<IOptionsMonitor<OkexOptions>>();
            var options = _optionsMonitor.CurrentValue;
            var socketClientOptions = new OkexSocketClientOptions();
            var clientOptions = new OkexClientOptions();
            if (options.Proxy!=null&&!string.IsNullOrEmpty(options.Proxy.Host)&&options.Proxy.Port!=default)
            {
                socketClientOptions.Proxy = new ApiProxy(options.Proxy.Host,options.Proxy.Port);
                clientOptions.Proxy = new ApiProxy(options.Proxy.Host,options.Proxy.Port);
            }
            _socketClient = new OkexSocketClient(socketClientOptions);
            _socketClient.SetApiCredentials(options.ApiKey, options.ApiSecret,
                options.PassPhrase);
            _client = new OkexClient();
            _client.SetApiCredentials(options.ApiKey, options.ApiSecret,
                options.PassPhrase);
        }

        public async Task RunAsync()
        {
            var symbol = "MASK-USDT";
            await _socketClient.Spot_SubscribeToTrades_Async(symbol, data =>
            {
                if (data.Side == OkexSpotOrderSide.Buy)
                {
                    lock (_buyTrades)
                    {
                        _buyTrades.Insert(0, data);
                        _buyTrades = _buyTrades.Take(100).ToList();
                    }
                }
                else
                {
                    lock (_sellTrades)
                    {
                        _sellTrades.Insert(0, data);
                        _sellTrades = _sellTrades.Take(100).ToList();
                    }
                }
                _logger.LogInformation($"{data.Timestamp:HH:mm:ss}:{data.Side}:{data.Price}-{data.Size}");
            });
        }
    }
}