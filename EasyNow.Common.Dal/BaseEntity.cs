using System;

namespace EasyNow.Common.Dal
{
    public abstract class BaseEntity : IIdKeyEntity, IAuditEntity
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public DateTime CreateTime { get; set; }

        /// <inheritdoc />
        public DateTime UpdateTime { get; set; }

        /// <inheritdoc />
        public Guid Creator { get; set; }

        /// <inheritdoc />
        public Guid Updater { get; set; }
    }
}