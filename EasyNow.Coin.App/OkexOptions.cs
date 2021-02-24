using System.Collections.Generic;

namespace EasyNow.Coin.App
{
    public class OkexOptions
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PassPhrase { get; set; }
        public Proxy Proxy { get; set; }
        public Dictionary<string,decimal[]> Transaction { get; set; }
    }

    public class Proxy
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}