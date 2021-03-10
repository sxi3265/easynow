using System;

namespace EasyNow.Utility.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// 用于加入自定义的format
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetString(this TimeSpan ts, string format)
        {
            switch (format)
            {
                //把天算入到小时中
                case "HHmm":
                    return (ts.Days * 24 + ts.Hours).ToString("00") + (ts.Minutes.ToString("00"));
            }
            return ts.ToString(format);
        }
    }
}