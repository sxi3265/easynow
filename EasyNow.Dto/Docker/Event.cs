using System;

namespace EasyNow.Dto.Docker
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public EventTarget Target { get; set; }
        public EventRequest Request { get; set; }
        public EventSource Source { get; set; }
    }
}