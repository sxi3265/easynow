using System.Collections.Generic;
using Newtonsoft.Json;

namespace EasyNow.AutoJsServer
{
    public class AutoJsMsg
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public Dictionary<string,object> Data { get; set; }
    }
}