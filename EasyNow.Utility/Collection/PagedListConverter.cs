using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyNow.Utility.Collection
{
    public class PagedListConverter : JsonConverterFactory
        {
            public override bool CanConvert(Type typeToConvert)
            {
                return typeof(IPagedList).IsAssignableFrom(typeToConvert);
            }

            public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            {
                return new PagedListConverterInner();
            }

            class PagedListConverterInner:JsonConverter<IPagedList>
            {
                public override IPagedList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("不是一个对象");
                    }

                    // todo 待实现
                    return null;
                }

                public override void Write(Utf8JsonWriter writer, IPagedList value, JsonSerializerOptions options)
                {
                    writer.WriteStartObject();
                    
                    foreach (var propertyInfo in value.GetType().GetProperties().Where(e=>e.Name!="Item"))
                    {
                        try
                        {
                            writer.WritePropertyName(JsonNamingPolicy.CamelCase.ConvertName(propertyInfo.Name));
                            //var converter = options.GetConverter(propertyInfo.PropertyType);
                            //if (converter != null)
                            //{
                            //    converter.As<>()
                            //}
                            JsonSerializer.Serialize(writer, propertyInfo.GetValue(value), options);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    writer.WriteEndObject();
                }
            }
        }
}