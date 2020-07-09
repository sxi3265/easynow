using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyNow.Utility.Collection
{
    public abstract class BasePagedList<T> : PagedListMetaData, IPagedList<T>
    {
        /// <summary>
        /// 项
        /// </summary>
        public List<T> Items { get;protected set; } = new List<T>();

        protected internal BasePagedList()
        {

        }

        protected internal BasePagedList(Pagination page, int totalItemCount)
        {
            if (page.PageNumber < 1)
            {
                throw new ArgumentOutOfRangeException($"页码{page.PageNumber}超出范围");
            }

            if (page.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException($"分页{page.PageSize}超出范围");
            }

            this.TotalItemCount = totalItemCount;
            this.PageSize = page.PageSize;
            this.PageNumber = page.PageNumber;
            this.PageCount = totalItemCount > 0 ? (int)Math.Ceiling(totalItemCount / (double)page.PageSize) : 0;
            this.HasPreviousPage = page.PageNumber > 1;
            this.HasNextPage = page.PageNumber < this.PageCount;
            this.IsFirstPage = page.PageNumber == 1;
            this.IsLastPage = page.PageNumber == this.PageCount;
            this.FirstItemOnPage = (page.PageNumber - 1) * page.PageSize + 1;

            var numberOfLastItemOnPage = this.FirstItemOnPage + page.PageSize - 1;
            this.LastItemOnPage = numberOfLastItemOnPage > totalItemCount ? totalItemCount : numberOfLastItemOnPage;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] => this.Items[index];

        /// <summary>
        /// 当前页总数
        /// </summary>
        public virtual int Count => this.Items.Count;

        public PagedListMetaData GetMetaData()
        {
            return new PagedListMetaData(this);
        }
    }
}