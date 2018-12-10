using System.Collections.Generic;
using Newtonsoft.Json;

namespace StoreReactNET.WebAPI.Models
{
    public class JSONProductFilter
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public List<string> Value { get; set; }

    }
}
