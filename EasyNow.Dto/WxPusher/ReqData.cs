namespace EasyNow.Dto.WxPusher
{
    public class ReqData
    {
        public string AppKey { get; set; }
        public string AppName { get; set; }
        public string Source { get; set; }
        public string UserName { get; set; }
        public string UserHeadImg { get; set; }
        public long Time { get; set; }
        public string Uid { get; set; }
        public string Extra { get; set; }
    }
}