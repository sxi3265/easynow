using System;

namespace EasyNow.AspNetCore.RateLimit
{
    /// <summary>
    /// 规则
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// 端点
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// 调用限制
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 配额超出响应
        /// </summary>
        public QuotaExceededResponse QuotaExceededResponse { get; set; }
    }
}