using System;

namespace EasyNow.Utility.Attributes
{
    /// <summary>
    /// 用于修饰枚举
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumScriptEnableAttribute:Attribute
    {
        /// <summary>
        /// 如果为false,则存储在缓存字典里的是枚举的int值，否则为字符串值，默认为true
        /// </summary>
        public bool CodeKey { get; set; } = true;
    }
}