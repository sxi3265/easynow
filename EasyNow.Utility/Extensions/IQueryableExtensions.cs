using System;
using System.Linq;
using System.Linq.Expressions;

namespace EasyNow.Utility.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> sequence, bool cond, Expression<Func<T, bool>> predicate)
        {
            return cond ? sequence.Where(predicate) : sequence;
        }
    }
}