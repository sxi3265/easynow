using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CryptoExchange.Net.Objects;
using EasyCaching.Core;
using EasyNow.Coin.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okex.Net;
using Okex.Net.CoreObjects;
using Okex.Net.Enums;
using Okex.Net.RestObjects;

namespace EasyNow.Coin.Bo
{
    public class Rule3:IRule
    {
        private readonly OkexSocketClient _socketClient;
        private ILifetimeScope _lifetimeScope;
        private IOptionsMonitor<OkexOptions> _optionsMonitor;
        private OkexClient _client;
        private ILogger _logger => _lifetimeScope.Resolve<ILogger<Rule2>>();
        //private IEasyCachingProviderFactory _cachingProviderFactory;
        //private IEasyCachingProvider _cachingProvider;
        private decimal _lastPrice;
        private ConcurrentDictionary<decimal, decimal> _orderDic = new ();
        private ConcurrentDictionary<decimal, SemaphoreSlim> _priceLockDic = new();

        private Dictionary<decimal, decimal> _priceDic;
        //{
        //    get => _cachingProvider.Get<Dictionary<decimal, decimal>>($"{nameof(Rule3)}_PriceDic").Value;
        //    set => _cachingProvider.Set($"{nameof(Rule3)}_PriceDic",value,TimeSpan.FromDays(1));
        //}
        public Rule3(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _optionsMonitor = _lifetimeScope.Resolve<IOptionsMonitor<OkexOptions>>();
            //_cachingProviderFactory = _lifetimeScope.Resolve<IEasyCachingProviderFactory>();
            //_cachingProvider=_cachingProviderFactory.GetCachingProvider("default");
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
            var tickerResult = await this._client.Spot_GetSymbolTicker_Async(symbol);
            var startPrice = tickerResult.Data.LastPrice;

            //_cachingProvider.Set("","",TimeSpan.MaxValue);
            //var startPrice=0.05072m;

            startPrice = startPrice * 1.1m;

            var rateOfReturn = 0.002m;
            var currentPrice=0m;
            var precision = 6;
            var tmp = (decimal)Math.Pow(10, precision);
            _priceDic = Enumerable.Range(0, 100).Select((e, i) =>
            {
                if (i == 0)
                {
                    currentPrice = startPrice;
                }

                var keyValue= new KeyValuePair<decimal, decimal>(Math.Ceiling(currentPrice * tmp) / tmp,
                    Math.Ceiling(currentPrice * (1m + rateOfReturn) * tmp) / tmp);
                currentPrice = currentPrice * (1m - rateOfReturn);
                return keyValue;
            }).GroupBy(e=>e.Key).ToDictionary(e => e.Key, e => e.Select(i=>i.Value).FirstOrDefault());
            await _socketClient.Spot_SubscribeToOrders_Async(symbol, async data =>
            {
                if(data.LastFillQuantity==0m)
                {
                    return;
                }
                try
                {
                    if (data.Side == OkexSpotOrderSide.Buy)
                    {
                        var sellPrice = GetSellPriceByBuyPrice(data.Price.Value);
                        if (sellPrice == 0)
                        {
                            return;
                        }

                        _orderDic.AddOrUpdate(data.Price.Value, _=>0, (k, v) => v - data.Size.Value);
                        await _client.Spot_PlaceOrder_Async(new OkexSpotPlaceOrder
                        {
                            Symbol = symbol,
                            ClientOrderId = Guid.NewGuid().ToString("N"),
                            Type = OkexSpotOrderType.Limit,
                            Side = OkexSpotOrderSide.Sell,
                            TimeInForce = OkexSpotTimeInForce.NormalOrder,
                            Price = sellPrice.ToString(),
                            Size = data.Size.ToString()
                        });
                        _logger.LogInformation($"开仓挂单成交,挂止盈单,网格价:{data.Price},止盈价格:{sellPrice}");
                    }
                    else
                    {
                        var buyPrice = GetBuyPriceBySellPrice(data.Price.Value);
                        if (buyPrice == 0)
                        {
                            return;
                        }

                        await BuyCoin(symbol, buyPrice, data.Size.Value);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e,"挂单失败");
                }
            });

            //foreach (var price in _priceDic)
            //{
            //    await _client.Spot_PlaceOrder_Async(new OkexSpotPlaceOrder
            //    {
            //        Symbol = symbol,
            //        ClientOrderId = Guid.NewGuid().ToString("N"),
            //        Type = OkexSpotOrderType.Limit,
            //        Side = OkexSpotOrderSide.Buy,
            //        TimeInForce = OkexSpotTimeInForce.NormalOrder,
            //        Price = price.Key.ToString(),
            //        Size = "100"
            //    });
            //}
            await _socketClient.Spot_SubscribeToTrades_Async(symbol, async data =>
            {
                if (_lastPrice == 0)
                {
                    _lastPrice = data.Price;
                    return;
                }

                if (_lastPrice > data.Price)
                {
                    _lastPrice = data.Price;
                    var buyPrice = GetBuyPrice(data.Price);
                    if (buyPrice != 0)
                    {
                        await BuyCoin(symbol, buyPrice, 100m);
                        _logger.LogInformation($"开仓挂单,网格价:{buyPrice}");
                    }
                }
            });
        }

        private async Task BuyCoin(string symbol,decimal price,decimal amount)
        {
            var semaphoreSlim = _priceLockDic.GetOrAdd(price, _ => new SemaphoreSlim(1, 1));
            await semaphoreSlim.WaitAsync();
            try
            {
                var count=_orderDic.GetOrAdd(price, _ => 0);
                if (count >= 200m)
                {
                    return;
                }
                var result =await _client.Spot_PlaceOrder_Async(new OkexSpotPlaceOrder
                {
                    Symbol = symbol,
                    ClientOrderId = Guid.NewGuid().ToString("N"),
                    Type = OkexSpotOrderType.Limit,
                    Side = OkexSpotOrderSide.Buy,
                    TimeInForce = OkexSpotTimeInForce.NormalOrder,
                    Price = price.ToString(),
                    Size = amount.ToString()
                });
                if (result.Success)
                {
                    _orderDic.AddOrUpdate(price, _ => count + amount, (_,_) => count + amount);
                }
                _logger.LogInformation($"开仓挂单,网格价:{price}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private decimal GetSellPriceByBuyPrice(decimal price)
        {
            return _priceDic
                .Where(e => e.Key >= price)
                .OrderBy(e => e.Key)
                .Select(e => e.Value)
                .FirstOrDefault();
        }

        private decimal GetBuyPriceBySellPrice(decimal price)
        {
            return _priceDic
                .Where(e => e.Value <= price)
                .OrderByDescending(e => e.Value)
                .Select(e => e.Key)
                .FirstOrDefault();
        }

        private decimal GetBuyPrice(decimal price)
        {
            return this._priceDic
                .Where(e => e.Key <= price)
                .OrderByDescending(e => e.Key)
                .Select(e => e.Key)
                .FirstOrDefault();
        }
    }
}