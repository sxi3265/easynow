using EasyNow.Common.Dal;

namespace EasyNow.Coin.Dal.Entities
{
    public class OrderBook:BaseEntity
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 币对
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
    }
}
