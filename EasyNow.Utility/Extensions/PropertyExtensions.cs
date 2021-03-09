using System.Collections.Generic;
using System.Linq;
using EasyNow.Utility.Attributes;

namespace EasyNow.Utility.Extensions
{
    public static class PropertyExtensions
    {
        public static IEnumerable<string> GetChangedProperty<TEntity, TDto>(TEntity entity, TDto dto, bool skipNullProperty = false)
        {
            var properties = typeof(TDto).GetProperties();

            foreach (var prop in properties)
            {
                var attrs = prop.GetAttributes<PropertyAttribute>();
                var entity_prop = typeof(TEntity).GetProperty(prop.Name);
                if (attrs.Any() && entity_prop != null)
                {
                    var dto_value = prop.GetValue(dto);
                    var entity_value = entity_prop.GetValue(entity);
                    if (dto_value == null && entity_value != null)
                    {
                        if (!skipNullProperty)
                        {
                            yield return string.Format("【{0}】 由 【{1}】 修改为 null", attrs.FirstOrDefault().Name, entity_value);
                        }
                    }
                    else
                    {
                        if (entity_value == null)
                        {
                            yield return string.Format("【{0}】 由 null 修改为 【{1}】", attrs.FirstOrDefault().Name, dto_value);
                        }
                        else if (dto_value.ToString() != entity_value.ToString())
                        {
                            yield return string.Format("【{0}】 由 【{1}】 修改为 【{2}】", attrs.FirstOrDefault().Name, entity_value, dto_value);
                        }
                    }
                }
            }
        }
    }
}