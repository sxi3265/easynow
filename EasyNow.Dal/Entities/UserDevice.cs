using System;

namespace EasyNow.Dal.Entities
{
    public class UserDevice : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public virtual User User { get; set; }
        public virtual Device Device { get; set; }
    }
}