using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyNow.Utility.Extensions
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// 获取字段特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute GetFieldAttribute<TAttribute, T>(Expression<Func<T, object>> expression) where TAttribute : Attribute
        {
            var fieldInfo = typeof(T).GetProperty(GetPropertyName(expression),BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo == null)
                return null;
            return fieldInfo.GetCustomAttribute(typeof(TAttribute), false) as TAttribute;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> exp)
        {
            return ((MemberExpression)exp.Body).Member.Name;
        }

        // requires explicit specification of both object type and property type
        private static string GetPropertyName<TObject, TResult>(Expression<Func<TObject, TResult>> exp)
        {
            // extract property name
            return ((MemberExpression)exp.Body).Member.Name;
        }

        static string GetPropertyName<TObject>(Expression<Func<TObject, object>> exp)
        {
            var body = exp.Body;
            if (body is UnaryExpression convertExpression)
            {
                if (convertExpression.NodeType != ExpressionType.Convert)
                {
                    throw new ArgumentException("Invalid property expression.", nameof(exp));
                }

                body = convertExpression.Operand;
            }

            return ((MemberExpression)body).Member.Name;
        }
    }
}