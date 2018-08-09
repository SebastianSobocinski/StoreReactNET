using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models
{
    public class JSONProductFilter
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public List<string> Value { get; set; }

    }
}
