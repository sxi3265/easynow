using System;
using EasyNow.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EasyNow.Utility.Tools
{
    public class MillisecondEpochConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value).TimeStamp().ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ((long?) reader.Value)?.FromTimeStamp();
        }
    }
}