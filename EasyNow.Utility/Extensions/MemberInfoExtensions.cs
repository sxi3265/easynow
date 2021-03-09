using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyNow.Utility.Extensions
{
    public static class MemberInfoExtensions
    {
        public static MethodInfo GetMethod<T>(this T instance, Expression<Func<T, object>> methodSelector)
        {
            // it is not work all the method
            return ((MethodCallExpression)methodSelector.Body).Method;
        }

        public static MethodInfo GetMethod<T>(this T instance,
                                              Expression<Action<T>> methodSelector)
        {
            return ((MethodCallExpression)methodSelector.Body).Method;
        }

        public static bool HasAttribute<TAttribute>(
            this MemberInfo member)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(member).Length > 0;
        }

        public static TAttribute[] GetAttributes<TAttribute>(
            this MemberInfo member)
            where TAttribute : Attribute
        {
            var attributes =
                member.GetCustomAttributes(typeof(TAttribute), true);

            return (TAttribute[])attributes;
        }
    }
}