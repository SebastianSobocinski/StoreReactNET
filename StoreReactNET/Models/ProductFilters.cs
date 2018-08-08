using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models
{
    public class ProductFilters
    {
        [JsonProperty("maxPrice")]
        public float? MaxPrice { get; set; }
        [JsonProperty("brands")]
        public List<string> Brands { get; set; }
        [JsonProperty("models")]
        public List<string> Models { get; set; }
        [JsonProperty("vramList")]
        public List<string> VramList { get; set; }
        [JsonProperty("busWidthList")]
        public List<string> BusWidthList { get; set; }
    }
}
