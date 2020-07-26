using System.Collections.Generic;
using Newtonsoft.Json;

namespace EasyNow.Utility.Collection
{
    [JsonObject]
    public interface IPagedList
    {
        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// 总对象数
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// 当前页码
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// 页面最大对象数
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// 是否第一页
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// 是否最后一页
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// 当前页第一项所处索引
        /// </summary>
        int FirstItemOnPage { get; }

        /// <summary>
        /// 当前页最后一项所处索引
        /// </summary>
        int LastItemOnPage { get; }
    }

    public interface IPagedList<T> : IPagedList, IEnumerable<T>
    {
        /// <summary>
        /// 项
        /// </summary>
        List<T> Items { get; }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] { get; }

        /// <summary>
        /// 当前页总数
        /// </summary>
        int Count { get; }

        PagedListMetaData GetMetaData();
    }
}
