using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StoreReactNET.Infrastructure.EntityFramework.Entities;

namespace StoreReactNET.WebAPI.Models.ViewModels
{
    public class ProductViewModel
    {
        [JsonProperty("productID")]
        public string ProductID { get; set; }
        [JsonProperty("productCategoryID")]
        public string ProductCategoryID { get; set; }
        [JsonProperty("productCategoryName")]
        public string ProductCategoryName { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }
        [JsonProperty("productPrice")]
        public double ProductPrice { get; set; }
        [JsonProperty("productImages")]
        public List<string> ProductImages { get; set; }



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
            catch (Exception) { }

        }
    }
}
