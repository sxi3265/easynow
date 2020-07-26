using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EasyNow.Utility.Tools
{
    public class JsonTool
    {
        static JsonTool()
        {
            // 枚举转换为小驼峰命名的字符串
            JsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            });
        }
        
        public static JsonSerializerSettings JsonSerializerSettings=new JsonSerializerSettings
        {
            // 小驼峰命名
            ContractResolver=new CamelCasePropertyNamesContractResolver(),
            // 设置参数时区
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            // 忽略循环引用
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            // 时间格式
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
    }
}