using Newtonsoft.Json;

namespace EasyNow.AutoJsServer
{
    public class ClientInfo
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }
        [JsonProperty("app_version")]
        public string AppVersion { get; set; }
        [JsonProperty("app_version_code")]
        public int AppVersionCode { get; set; }
        [JsonProperty("client_version")]
        public int ClientVersion { get; set; }
    }
}