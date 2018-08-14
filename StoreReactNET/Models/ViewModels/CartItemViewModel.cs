using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    public class CartItemViewModel : ProductViewModel
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        public CartItemViewModel(Products product) : base (product)
        {

        }
    }
}
