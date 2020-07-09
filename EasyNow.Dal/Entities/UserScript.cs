using System;

namespace EasyNow.Dal.Entities
{
    public class UserScript:BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ScriptId { get; set; }

        public virtual User User { get; set; }

        public virtual Script Script { get; set; }
    }
}