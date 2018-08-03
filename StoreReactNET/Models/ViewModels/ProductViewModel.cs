using StoreReactNET.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreReactNET.Models.ViewModels
{
    public class ProductViewModel
    {
        public string ProductID { get; }
        public string ProductCategoryID { get; }
        public string ProductCategoryName { get; }
        public string ProductName { get; }
        public string ProductDescription { get; }
        public double ProductPrice { get; }
        public List<string> ProductImages { get; }



        public ProductViewModel(Products Product)
        {
            try
            {
                this.ProductID = Product.Id.ToString();
                this.ProductCategoryID = Product.ProductCategoryId.ToString();
                this.ProductCategoryName = Product.ProductCategory.CategoryName;
                this.ProductName = Product.Name;
                this.ProductDescription = Product.Description;
                this.ProductPrice = Product.PriceVat;
                this.ProductImages = new List<string>();

                foreach(var image in Product.ProductImages)
                {
                    this.ProductImages.Add(image.ImageName);
                }
            }
            catch (Exception ex) { }

        }
    }
}
