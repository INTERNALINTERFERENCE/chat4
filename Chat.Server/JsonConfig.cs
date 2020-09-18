using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server
{
    public class JsonConfig
    {
        [JsonProperty("first")]
        public string badWord { get; private set; }
        [JsonProperty("firstCommand")]
        public string firstCommand { get; private set; }
    }
}
