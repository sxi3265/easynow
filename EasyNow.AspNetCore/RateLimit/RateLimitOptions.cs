using System.Collections.Generic;

namespace EasyNow.AspNetCore.RateLimit
{
    public class RateLimitOptions
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool Enable { get; set; } = true;

        /// <summary>
        /// 策略
        /// </summary>
        public virtual IEnumerable<ClientPolicy> Policies { get; set; }

        /// <summary>
        /// 配额超出响应
        /// </summary>
        public virtual QuotaExceededResponse QuotaExceededResponse { get; set; }

        /// <summary>
        /// 累加阻止的请求
        /// </summary>
        public virtual bool StackBlockedRequests { get; set; }

        /// <summary>
        /// 禁用频率限制Header
        /// </summary>
        public virtual bool DisableRateLimitHeaders { get; set; }
    }
}