namespace EasyNow.Utility.Collection
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 页码
        /// </summary>
        /// <remarks>页码从1开始</remarks>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 20;
    }
}