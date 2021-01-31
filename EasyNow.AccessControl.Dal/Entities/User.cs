using EasyNow.Common.Dal;

namespace EasyNow.AccessControl.Dal.Entities
{
    public class User:BaseEntity,IDeleteStateEntity
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        public bool IsDeleted { get; set; }
    }
}
