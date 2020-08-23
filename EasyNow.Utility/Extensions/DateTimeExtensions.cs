using System;

namespace EasyNow.Utility.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartTime = new DateTime(1970, 1, 1);
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long TimeStamp(this DateTime dateTime)
        {
            var ts = dateTime - StartTime;
            return Convert.ToInt64(ts.Seconds);
        }

        /// <summary>
        /// 从时间戳得到时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromTimeStamp(this long timestamp)
        {
            return StartTime.AddMilliseconds(timestamp);
        }
    }
}