using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EasyNow.Coin.Dto
{
    public class OkexOptions
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PassPhrase { get; set; }
        public Proxy Proxy { get; set; }
        public IConfigurationSection RuleCfg { get; set; }
    }

    public class RuleCfg:Dictionary<string,decimal[]>
    {
        public RuleCfg()
        {

        }
    }
}