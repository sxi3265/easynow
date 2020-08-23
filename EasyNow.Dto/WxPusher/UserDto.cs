using System;

namespace EasyNow.Dto.WxPusher
{
    public class UserDto
    {
        public string Uid { get; set; }
        public string NickName { get; set; }
        public string HeadImg { get; set; }
        public bool Enable { get; set; }
        public DateTime SubTime { get; set; }
    }
}