using System;

namespace EasyNow.Dal.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Guid Creator { get; set; }
        public Guid Updater { get; set; }
    }
}