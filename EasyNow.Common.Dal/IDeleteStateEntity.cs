namespace EasyNow.Common.Dal
{
    public interface IDeleteStateEntity:IEntity
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}