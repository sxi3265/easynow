using System;

namespace EasyNow.AspNetCore.RateLimit
{
    /// <summary>
    /// 开始时间和计数
    /// </summary>
    public class Counter
    {
        public DateTime StartTime { get; set; }

        public double Count { get; set; }
    }
}