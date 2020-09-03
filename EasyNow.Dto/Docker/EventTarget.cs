namespace EasyNow.Dto.Docker
{
    public class EventTarget
    {
        public string MediaType { get; set; }
        public int Size { get; set; }
        public string Digest { get; set; }
        public int Length { get; set; }
        public string Repository { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
    }
}