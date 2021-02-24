using System;

namespace EasyNow.Coin.Dto
{
    public class Order
    {
        public decimal BuyPrice { get; set; }
        public string BuyOrderId { get; set; }
        public decimal SellPrice { get; set; }
        public string SellOrderId { get; set; }
        public decimal Balance { get; set; }
    }
}
