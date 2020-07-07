using System;

namespace EasyNow.Utility.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long TimeStamp(this DateTime dateTime)
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.Seconds);
        }
    }
}