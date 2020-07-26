namespace EasyNow.Dal.Entities
{
    public class Script:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public byte[] Content { get; set; }
    }
}