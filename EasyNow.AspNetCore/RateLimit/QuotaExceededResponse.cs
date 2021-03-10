namespace EasyNow.AspNetCore.RateLimit
{
    /// <summary>
    /// 配额超出响应
    /// </summary>
    public class QuotaExceededResponse
    {
        /// <summary>
        /// 被限制后的http状态码
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType { get; set; }
    }
}