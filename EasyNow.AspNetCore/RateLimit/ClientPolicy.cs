using System.Collections.Generic;

namespace EasyNow.AspNetCore.RateLimit
{
    /// <summary>
    /// 策略
    /// </summary>
    public class ClientPolicy
    {
        public IEnumerable<Rule> Rules { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        public string ClientType { get; set; }
    }
}