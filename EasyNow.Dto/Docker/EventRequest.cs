using System;

namespace EasyNow.Dto.Docker
{
    public class EventRequest
    {
        public Guid Id { get; set; }
        public string Addr { get; set; }
        public string Host { get; set; }
        public string Method { get; set; }
        public string Useragent { get; set; }
    }
}