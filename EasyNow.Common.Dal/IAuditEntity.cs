using System;

namespace EasyNow.Common.Dal
{
    public interface IAuditEntity:IEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        Guid Creator { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        Guid Updater { get; set; }
    }
}