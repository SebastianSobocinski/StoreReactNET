using System;
using System.Collections.Generic;
using System.Text;

namespace StoreReactNET.Services.Product.Models.Outputs
{
    public class ClickedProductDTO : ProductDTO
    {
        public List<List<string>> ProductDetailsList { get; set; }
    }
}
