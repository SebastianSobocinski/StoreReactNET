﻿using System.Collections.Generic;

namespace StoreReactNET.Infrastructure.EntityFramework.Entities
{
    public partial class Products
    {
        public Products()
        {
            ProductDetails = new HashSet<ProductDetails>();
            ProductImages = new HashSet<ProductImages>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceVat { get; set; }
        public int ProductCategoryId { get; set; }
        public int? ProductDetailsId { get; set; }

        public ProductCategories ProductCategory { get; set; }
        public ProductDetails ProductDetailsNavigation { get; set; }
        public ICollection<ProductDetails> ProductDetails { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
    }
}
