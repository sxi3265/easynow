using System;

namespace EasyNow.Dto
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Guid Creator { get; set; }
        public Guid Updater { get; set; }
    }
}