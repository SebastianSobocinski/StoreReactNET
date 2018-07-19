using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class Products
    {
        public Products()
        {
            CartItems = new HashSet<CartItems>();
            ProductImages = new HashSet<ProductImages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceVat { get; set; }
        public int ProductCategoryId { get; set; }

        public ProductCategories ProductCategory { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
    }
}
