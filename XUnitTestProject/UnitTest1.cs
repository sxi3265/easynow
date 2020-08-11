using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using EasyNow.Dto.Script;
using EasyNow.Utility.Collection;
using Esprima.Ast;
using Jint;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test2()
        {
            var jdoc = JsonDocument.Parse(
                "{\"type\":\"hello\",\"data\":{\"device_name\":\"Xiaomi Mi 10\",\"app_version\":\"4.1.1 Alpha2\",\"app_version_code\":461,\"client_version\":2}}");
        }

        [Fact]
        public void Test1()
        {
            var pagedList = new PagedList<ScriptInfo>(new []{new ScriptInfo
            {
                Id = Guid.NewGuid(),
                Name = "test",
                //Content = "ssss"
            } },new Pagination
            {
                PageNumber = 1,
                PageSize = 2
            });

            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
            options.Converters.Add(new PagedListConverter());
            var json = JsonSerializer.Serialize(pagedList,options);
        }

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
                        throw new JsonException("����һ������");
                    }

                    return null;
                }

                public override void Write(Utf8JsonWriter writer, IPagedList value, JsonSerializerOptions options)
                {
                    writer.WriteStartObject();
                    foreach (var propertyInfo in value.GetType().GetProperties().Where(e=>e.Name!="Item"))
                    {
                        try
                        {
                            writer.WritePropertyName(propertyInfo.Name);
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
}
