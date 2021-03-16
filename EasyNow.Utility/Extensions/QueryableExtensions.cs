using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EasyNow.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EasyNow.Utility.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int? pageNumber, int pageSize)
        {
            var truePageNumber = pageNumber ?? 1;
            var itemIndex = (truePageNumber - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            return new PagedList<T>(pageOfItems.ToArray(), new Pagination{PageNumber = truePageNumber, PageSize = pageSize}, allItems.Count());
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, IPagination pagination)
        {
            var itemIndex = (pagination.PageNumber - 1) * pagination.PageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pagination.PageSize);
            return new PagedList<T>(pageOfItems.ToArray(), pagination, allItems.Count());
        }

        /// <summary>
        /// 分页数据查询,并转换数据类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="allItems"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static PagedList<TDestination> ToPagedList<TSource,TDestination>(this IQueryable<TSource> allItems, IPagination pagination)
        {
            var itemIndex = (pagination.PageNumber - 1) * pagination.PageSize;
            var count = allItems.Count();
            var pageOfItems = (itemIndex > 0 ? allItems.Skip(itemIndex) : allItems).Take(pagination.PageSize).AsEnumerable().Select(e=>e.To<TDestination>()).ToArray();
            return new PagedList<TDestination>(pageOfItems, pagination,
                count);
        }

        public static async Task<PagedList<TDestination>> ToPagedListAsync<TSource,TDestination>(this IQueryable<TSource> allItems, IPagination pagination)
        {
            var itemIndex = (pagination.PageNumber - 1) * pagination.PageSize;
            if (allItems.Provider is EntityQueryProvider)
            {
                return new PagedList<TDestination>(
                    (await (itemIndex > 0 ? allItems.Skip(itemIndex) : allItems).Take(pagination.PageSize)
                        .ToArrayAsync()).Select(e => e.To<TDestination>()).ToArray(), pagination,
                    await allItems.CountAsync());
            }

            var count = allItems.Count();
            var pageOfItems = (itemIndex > 0 ? allItems.Skip(itemIndex) : allItems).Take(pagination.PageSize).AsEnumerable().Select(e=>e.To<TDestination>()).ToArray();
            return new PagedList<TDestination>(pageOfItems, pagination,
                count);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> sequence, bool cond, Expression<Func<T, bool>> predicate)
        {
            return cond ? sequence.Where(predicate) : sequence;
        }

        /// <summary>
        /// 获取最大值，没有则返回空
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MaxOrNull<TSource, TResult>(this IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector) where TResult:struct
        {
            return source.Max(e => (TResult?) selector.Compile()(e));
        }

        /// <summary>
        /// 获取最小值，没有则返回空
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult? MinOrNull<TSource, TResult>(this IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector) where TResult:struct
        {
            return source.Min(e => (TResult?) selector.Compile()(e));
        }
    }
}