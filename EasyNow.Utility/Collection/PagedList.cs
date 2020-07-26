using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace EasyNow.Utility.Collection
{
    public class PagedList<T, TKey> : BasePagedList<T>
    {
        public PagedList(IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector, Pagination page) : this(queryable, keySelector.Compile(), page)
        {

        }

        public PagedList(IQueryable<T> queryable, Func<T, TKey> keySelectorFunc, Pagination page) : base(
            page, queryable?.Count() ?? 0)
        {
            if (TotalItemCount > 0)
            {
                this.Items.AddRange(page.PageNumber == 1
                    ? queryable.OrderBy(keySelectorFunc).Take(page.PageSize)
                    : queryable.OrderBy(keySelectorFunc).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize));
            }
        }
    }

    public class PagedList<T> : BasePagedList<T>
    {
        [JsonConstructor]
        public PagedList(int pageNumber,int pageSize,IEnumerable<T> items,int totalItemCount):this(items,new Pagination{PageNumber = pageNumber,PageSize = pageSize},totalItemCount)
        {
            
        }

        public PagedList(IQueryable<T> queryable, Pagination page) : base(page, queryable?.Count() ?? 0)
        {
            if (TotalItemCount > 0)
            {
                this.Items.AddRange(page.PageNumber == 1 ? queryable.Take(page.PageSize) : queryable.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize));
            }
        }

        public PagedList(IEnumerable<T> enumerable, Pagination page) : this(enumerable.AsQueryable<T>(), page)
        {

        }

        public PagedList(IEnumerable<T> enumerable, Pagination page, int totalItemCount) : base(page, totalItemCount)
        {
            if (enumerable.Any())
            {
                this.Items.AddRange(enumerable);
            }
        }

        public PagedList(IPagedList pagedList, IEnumerable<T> enumerable)
        {
            TotalItemCount = pagedList.TotalItemCount;
            PageSize = pagedList.PageSize;
            PageNumber = pagedList.PageNumber;
            PageCount = pagedList.PageCount;
            HasPreviousPage = pagedList.HasPreviousPage;
            HasNextPage = pagedList.HasNextPage;
            IsFirstPage = pagedList.IsFirstPage;
            IsLastPage = pagedList.IsLastPage;
            FirstItemOnPage = pagedList.FirstItemOnPage;
            LastItemOnPage = pagedList.LastItemOnPage;

            this.Items.AddRange(enumerable);
        }
    }
}
