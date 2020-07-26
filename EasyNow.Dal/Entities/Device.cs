using System;

namespace EasyNow.Dal.Entities
{
    public class Device : BaseEntity
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public string SocketId { get; set; }
        public string Uuid { get; set; }
    }
}