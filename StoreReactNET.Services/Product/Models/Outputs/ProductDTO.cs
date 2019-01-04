using System;
using System.Collections.Generic;
using System.Text;

namespace StoreReactNET.Services.Product.Models.Outputs
{
    public class ProductDTO
    {
        public string ProductID { get; set; }
        public string ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public List<string> ProductImages { get; set; }
    }
}
