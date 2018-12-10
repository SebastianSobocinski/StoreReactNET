using Newtonsoft.Json;
using StoreReactNET.Infrastructure.EntityFramework.Entities;

namespace StoreReactNET.WebAPI.Models.ViewModels
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
