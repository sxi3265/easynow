using EasyNow.Common.Dal;

namespace EasyNow.AccessControl.Dal.Entities
{
    public class Role : BaseEntity, IDeleteStateEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        public bool IsDeleted { get; set; }
    }
}