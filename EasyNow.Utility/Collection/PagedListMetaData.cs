namespace EasyNow.Utility.Collection
{
    public class PagedListMetaData : IPagedList
    {
        protected PagedListMetaData()
        {
        }

        public PagedListMetaData(IPagedList pagedList)
        {
            this.PageCount = pagedList.PageCount;
            this.TotalItemCount = pagedList.TotalItemCount;
            this.PageNumber = pagedList.PageNumber;
            this.PageSize = pagedList.PageSize;
            this.HasPreviousPage = pagedList.HasPreviousPage;
            this.HasNextPage = pagedList.HasNextPage;
            this.IsFirstPage = pagedList.IsFirstPage;
            this.IsLastPage = pagedList.IsLastPage;
            this.FirstItemOnPage = pagedList.FirstItemOnPage;
            this.LastItemOnPage = pagedList.LastItemOnPage;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; protected set; }

        /// <summary>
        /// 总对象数
        /// </summary>
        public int TotalItemCount { get; protected set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNumber { get; protected set; }

        /// <summary>
        /// 页面最大对象数
        /// </summary>
        public int PageSize { get; protected set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage { get; protected set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get; protected set; }

        /// <summary>
        /// 是否第一页
        /// </summary>
        public bool IsFirstPage { get; protected set; }

        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; protected set; }

        /// <summary>
        /// 当前页第一项所处索引
        /// </summary>
        public int FirstItemOnPage { get; protected set; }

        /// <summary>
        /// 当前页最后一项所处索引
        /// </summary>
        public int LastItemOnPage { get; protected set; }
    }
}