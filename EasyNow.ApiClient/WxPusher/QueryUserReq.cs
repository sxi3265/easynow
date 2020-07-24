namespace EasyNow.ApiClient.WxPusher
{
    public class QueryUserReq
    {
        public string AppToken { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Uid { get; set; }
    }
}