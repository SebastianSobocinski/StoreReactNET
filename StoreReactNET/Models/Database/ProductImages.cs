using System;
using System.Collections.Generic;

namespace StoreReactNET.Models.Database
{
    public partial class ProductImages
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }

        public Products Product { get; set; }
    }
}
