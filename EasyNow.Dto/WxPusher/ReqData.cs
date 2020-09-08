using System;
using EasyNow.Utility.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EasyNow.Dto.WxPusher
{
    public class ReqData
    {
        public string AppKey { get; set; }
        public string AppName { get; set; }
        public string Source { get; set; }
        public string UserName { get; set; }
        public string UserHeadImg { get; set; }
        [JsonConverter(typeof(MillisecondEpochConverter))]
        public DateTime Time { get; set; }
        public string Uid { get; set; }
        public string Extra { get; set; }
    }
}