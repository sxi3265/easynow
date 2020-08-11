using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using EasyNow.Utility.Collection;

namespace EasyNow.Utility.Extensions
{
    public static class QueryableExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int? pageNumber, int pageSize)
        {
            var truePageNumber = pageNumber ?? 1;
            var itemIndex = (truePageNumber - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            return new PagedList<T>(pageOfItems.ToArray(), new Pagination{PageNumber = truePageNumber, PageSize = pageSize}, allItems.Count());
        }

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> allItems, Pagination pagination)
        {
            var itemIndex = (pagination.PageNumber - 1) * pagination.PageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pagination.PageSize);
            return new PagedList<T>(pageOfItems.ToArray(), new Pagination{PageNumber = pagination.PageNumber, PageSize = pagination.PageSize}, allItems.Count());
        }

        /// <summary>
        /// 使用数据库批量处理来完成分页数据查询,并转换数据类型
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="allItems"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IPagedList<TDestination> ToPagedList<TSource,TDestination>(this IQueryable<TSource> allItems, Pagination pagination)
        {
            var itemIndex = (pagination.PageNumber - 1) * pagination.PageSize;
            var count = allItems.Count();
            var pageOfItems = (itemIndex > 0 ? allItems.Skip(itemIndex) : allItems).Take(pagination.PageSize).ProjectTo<TDestination>(UtilitySetup.Mapper.ConfigurationProvider).ToArray();
            return new PagedList<TDestination>(pageOfItems, pagination,
                count);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> sequence, bool cond, Expression<Func<T, bool>> predicate)
        {
            return cond ? sequence.Where(predicate) : sequence;
        }
    }
}