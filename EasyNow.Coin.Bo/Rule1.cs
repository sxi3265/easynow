using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CryptoExchange.Net.Objects;
using EasyNow.Coin.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okex.Net;
using Okex.Net.CoreObjects;
using Okex.Net.Enums;
using Okex.Net.RestObjects;

namespace EasyNow.Coin.Bo
{
    public class Rule1:IRule
    {
        private readonly OkexSocketClient _socketClient;
        private readonly OkexClient _client;
        private readonly Dictionary<string, Dictionary<decimal,decimal>> _buyDic = new();
        private readonly Dictionary<string, Dictionary<decimal,decimal>> _sellDic = new();
        private readonly ConcurrentDictionary<string, Order> _orderDic = new();
        private ILogger _logger=>_lifetimeScope.Resolve<ILogger<Rule1>>();
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IOptionsMonitor<OkexOptions> _optionsMonitor;

        private Dictionary<string, decimal[]> TransactionDic
        {
            get
            {
                var options = _optionsMonitor.CurrentValue;
                return options.RuleCfg.GetSection(nameof(Rule1)).GetSection("transaction").Get<Dictionary<string, decimal[]>>();
            }
        }

        public Rule1(ILifetimeScope lifetimeScope)
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

        public Task RunAsync()
        {
            var symbol = "MASK-USDT";
            Subscribe(symbol);
            _orderDic.TryAdd(symbol, new Order
            {
                Balance = -1m
            });

            Task.Factory.StartNew(() =>
            {
                CancelOrder(symbol);
                while (true)
                {
                    try
                    {
                        Transaction(symbol);

                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex,$"{symbol}交易发生错误");
                    }
                    Thread.Sleep(2000);
                }
            }, TaskCreationOptions.LongRunning);
            return Task.CompletedTask;
        }

        private void Subscribe(string symbol)
        {
            _socketClient.Spot_SubscribeToOrderBook(symbol, OkexOrderBookDepth.Depth400, (data) =>
            {
                if (data != null && ((data.Asks != null && data.Asks.Any()) || (data.Bids != null && data.Bids.Any())))
                {
                    if (!_sellDic.TryGetValue(data.Symbol, out var sellDic))
                    {
                        sellDic = new Dictionary<decimal, decimal>();
                        _sellDic.Add(data.Symbol,sellDic);
                    }
                    if (!_buyDic.TryGetValue(data.Symbol, out var buyDic))
                    {
                        buyDic = new Dictionary<decimal, decimal>();
                        _buyDic.Add(data.Symbol,buyDic);
                    }
                    if (data.DataType == OkexOrderBookDataType.DepthPartial)
                    {
                        foreach (var entry in data.Asks)
                        {
                            sellDic.Add(entry.Price,entry.Quantity);
                        }
                        foreach (var entry in data.Bids)
                        {
                            buyDic.Add(entry.Price,entry.Quantity);
                        }
                    }
                    else
                    {
                        foreach (var entry in data.Asks)
                        {
                            if (sellDic.ContainsKey(entry.Price))
                            {
                                if (entry.Quantity == 0)
                                {
                                    sellDic.Remove(entry.Price);
                                }
                                else
                                {
                                    sellDic[entry.Price] = entry.Quantity;
                                }
                            }
                            else
                            {
                                sellDic.Add(entry.Price,entry.Quantity);
                            }
                        }
                        foreach (var entry in data.Bids)
                        {
                            if (buyDic.ContainsKey(entry.Price))
                            {
                                if (entry.Quantity == 0)
                                {
                                    buyDic.Remove(entry.Price);
                                }
                                else
                                {
                                    buyDic[entry.Price] = entry.Quantity;
                                }
                            }
                            else
                            {
                                buyDic.Add(entry.Price,entry.Quantity);
                            }
                        }
                    }
                }
            });
        }

        private void CancelOrder(string symbol)
        {
            var orders=this._client.Spot_GetAllOrders(symbol,OkexSpotOrderState.Open);
            if (!orders.Data.Any())
            {
                return;
            }
            this._client.Spot_BatchCancelOrders(orders.Data.GroupBy(e => e.Symbol).Select(e => new OkexSpotCancelOrder
            {
                Symbol = e.Key,
                OrderIds = e.Select(o => o.OrderId.ToString()).ToArray()
            }).ToArray());
        }

        private decimal GetPrice(Dictionary<decimal,decimal> data,decimal depth)
        {
            var count = 0m;
            foreach (var item in data)
            {
                count += item.Value;
                if (count > depth)
                {
                    return item.Key;
                }
            }

            return 0;
        }

        private void Transaction(string symbol)
        {
            // 如果没有相应数据则结束
            if (!_buyDic.TryGetValue(symbol, out var buyData) || !_sellDic.TryGetValue(symbol, out var sellData))
            {
                return;
            }
            // 如果未设置参数则结束
            if (!TransactionDic.TryGetValue(symbol, out var parameters)||parameters.Length!=4)
            {
                return;
            }
            var buyPrice = GetPrice(buyData.OrderByDescending(e => e.Key).ToDictionary(e => e.Key, e => e.Value), parameters[0]);
            if (buyPrice == 0)
            {
                return;
            }
            buyPrice*=(1m+parameters[2]*0.1m);
            var sellPrice = GetPrice(sellData.OrderBy(e => e.Key).ToDictionary(e => e.Key, e => e.Value), parameters[1]);
            if (sellPrice == 0)
            {
                return;
            }
            sellPrice*=(1m-parameters[2]*0.1m);
            while (sellPrice - buyPrice < parameters[2]*buyPrice)
            {
                //buyPrice *= 0.9m;
                //sellPrice *= 1.1m;
                return;
            }

            var symbols = symbol.Split("-");
            _orderDic.TryGetValue(symbol, out var order);
            var balance=this._client.Spot_GetSymbolBalance(symbols[1]);
            var buyCount = Math.Round(balance.Data.Balance / buyPrice*0.999m, 6);
            balance = this._client.Spot_GetSymbolBalance(symbols[0]);
            var sellCount = Math.Round(balance.Data.Available, 6);

            

            if (sellPrice != order.SellPrice)
            {
                if (!string.IsNullOrEmpty(order.SellOrderId))
                {
                    this._client.Spot_CancelOrder(symbol, clientOrderId: order.SellOrderId);
                }

                if (sellCount > parameters[3])
                {
                    var orderId = Guid.NewGuid().ToString("N");
                    var result=_client.Spot_PlaceOrder(new OkexSpotPlaceOrder
                    {
                        Symbol = symbol,
                        ClientOrderId = orderId,
                        Type = OkexSpotOrderType.Limit,
                        Side = OkexSpotOrderSide.Sell,
                        TimeInForce = OkexSpotTimeInForce.NormalOrder,
                        Price = sellPrice.ToString(),
                        Size = sellCount.ToString()
                    });
                    if (result.Data.ErrorCode!="0")
                    {
                        this._logger.LogError(result.Data.ErrorMessage);
                    }
                    else
                    {
                        order.SellOrderId = orderId;
                        order.SellPrice = sellPrice;
                    }
                    _logger.LogInformation($"卖:{sellPrice}-{sellCount}");
                }
            }

            if (buyPrice != order.BuyPrice)
            {
                if (!string.IsNullOrEmpty(order.BuyOrderId))
                {
                    this._client.Spot_CancelOrder(symbol, clientOrderId: order.BuyOrderId);
                }

                if (buyCount >= parameters[3])
                {
                    var orderId = Guid.NewGuid().ToString("N");
                    var result=_client.Spot_PlaceOrder(new OkexSpotPlaceOrder
                    {
                        Symbol = symbol,
                        ClientOrderId = orderId,
                        Type = OkexSpotOrderType.Limit,
                        Side = OkexSpotOrderSide.Buy,
                        TimeInForce = OkexSpotTimeInForce.NormalOrder,
                        Price = buyPrice.ToString(),
                        Size = buyCount.ToString()
                    });
                    if (result.Data.ErrorCode!="0")
                    {
                        this._logger.LogError(result.Data.ErrorMessage);
                    }
                    else
                    {
                        order.BuyOrderId = orderId;
                        order.BuyPrice = buyPrice;
                    }
                    _logger.LogInformation($"买:{buyPrice}-{buyCount}");
                }
            }
        }
    }
}
