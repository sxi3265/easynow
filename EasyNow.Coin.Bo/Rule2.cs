using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CryptoExchange.Net.Objects;
using EasyNow.Coin.Dto;
using EasyNow.Utility.Extensions;
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
        private List<OkexSpotTrade> _trades = new ();
        private ILogger _logger => _lifetimeScope.Resolve<ILogger<Rule2>>();
        private decimal[] _gridPricesLong;
        private decimal[] _gridPricesShort;
        private decimal _lastPrice;
        /// <summary>
        /// 网格在多头、空头方向的格子(档位)数量
        /// </summary>
        private int _gridAmount=10;
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
            var symbol = "DOGE-USDT";
            // 起始价位
            var startPrice = 0.050088m;
            // 多头每格价格跌幅(网格密度)
            var gridRegionLong = Enumerable.Repeat(0.005m,_gridAmount);
            // 空头每格价格涨幅(网格密度)
            var gridRegionShort = Enumerable.Repeat(0.005m,_gridAmount);
            // 多头每格持仓手数
            var gridVolumeLong = Enumerable.Range(0, _gridAmount+1);
            // 空头每格持仓手数
            var gridVolumeShort = Enumerable.Range(0, _gridAmount + 1);

            var currentPrice=0m;
            // 多头每格的触发价位列表
            _gridPricesLong = gridRegionLong.Select((e, i) =>
            {
                if (i == 0)
                {
                    return currentPrice=startPrice;
                }

                return currentPrice *= (1m- e);
            }).ToArray();

            // 空头每格的触发价位列表
            _gridPricesShort = gridRegionShort.Select((e, i) =>
            {
                if (i == 0)
                {
                    return currentPrice=startPrice;
                }

                return currentPrice *= (1m + e);
            }).ToArray();
            _logger.LogInformation($"策略开始运行,起始价位:{startPrice},多头每格持仓手数:{gridVolumeLong.ToJson()},多头每格的价位:{_gridPricesLong.ToJson()},空头每格的价位:{_gridPricesShort.ToJson()}");

            await Subscribe(symbol);
            while (this._lastPrice==0)
            {
                await Task.Delay(1000);
            }
            this.WaitPrice(0);
        }

        private void WaitPrice(int layer)
        {
            if (layer > 0 || _lastPrice <= _gridPricesLong[1])
            {
                while (true)
                {
                    if (layer < _gridAmount && _lastPrice <= _gridPricesLong[layer + 1])
                    {
                        _logger.LogInformation($"最新价:{this._lastPrice},进入:多头第{layer+1}档");
                        this.WaitPrice(layer+1);
                    }

                    if (this._lastPrice > _gridPricesLong[layer])
                    {
                        this._logger.LogInformation($"最新价:{_lastPrice},回退到:多头第{layer}档");
                        return;
                    }
                }
            }else if (layer < 0 || this._lastPrice >= _gridPricesShort[1])
            {
                layer = -layer;
                while (true)
                {
                    if (layer < this._gridAmount && this._lastPrice >= this._gridPricesShort[layer + 1])
                    {
                        _logger.LogInformation($"最新价:{this._lastPrice},进入:空头第{layer+1}档");
                        WaitPrice(-(layer+1));
                    }

                    if (this._lastPrice < this._gridPricesShort[layer])
                    {
                        this._logger.LogInformation($"最新价:{_lastPrice},回退到:空头第{layer}档");
                        return;
                    }
                }
            }
        }

        private Task Subscribe(string symbol)
        {
            return _socketClient.Spot_SubscribeToTrades_Async(symbol, data =>
            {
                this._lastPrice = data.Price;
                lock (_trades)
                {
                    _trades.Insert(0, data);
                    _trades = _trades.Take(100).ToList();
                }
            });
        }

        private void Get()
        {
            var time = DateTime.UtcNow;
            time = time.AddSeconds(-1 * time.Second);
            time = time.AddMilliseconds(-1 * time.Millisecond);
            var trade = _trades.FirstOrDefault(e => e.Timestamp <= time);
            var price = trade.Price;
            time = time.AddMinutes(-1);
            trade=_trades.FirstOrDefault(e => e.Timestamp <= time);

        }
    }
}