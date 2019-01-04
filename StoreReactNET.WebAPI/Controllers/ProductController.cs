using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreReactNET.Services.Product;
using StoreReactNET.Services.Product.Models.Inputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.WebAPI.Controllers
{
    [Route("Product/[action]")]
    public sealed class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult> GetProducts(int CategoryID, int Page, string Filters, string OrderBy)
        {
             var respond = new
             {
                 success = false,
                 products = new List<ProductDTO>()
             };


            try
            {
                var filters = new List<JSONProductFilter>();
                if (Filters != "null")
                    filters = JsonConvert.DeserializeObject<List<JSONProductFilter>>(Filters);
                    
                var result = await _productService.GetProducts(CategoryID, Page, filters, OrderBy);
                respond = new
                {
                    success = true,
                    products = result
                };
            }
            catch(Exception) { }

             
            return Json(respond);
         }
        [HttpGet]
        public async Task<ActionResult> GetClickedProduct(int ProductID)
        {

            var respond = new
            {
                success = false,
                product = new ClickedProductDTO()
            };
            try
            {
                var result = await _productService.GetClickedProduct(ProductID);

                respond = new
                {
                    success = true,
                    product = result
                };
            }
            catch (Exception) { }
            



            return Json(respond);
             
         }
        [HttpGet]
        public async Task<ActionResult> GetSearchedProducts(string Query, int Page, string OrderBy)
        {
            var respond = new
            {
                success = false,
                products = new List<ProductDTO>()
            };
            if (Query != null)
            {
                var QueryArray = JsonConvert.DeserializeObject<List<string>>(Query);
                try
                {
                    var result = await _productService.GetSearchedProducts(QueryArray, Page, OrderBy);

                    respond = new
                    {
                        success = true,
                        products = result
                    };
                }
                catch (Exception) { }
            }
            
            return Json(respond);
             
        }
        [HttpGet]
        public async Task<ActionResult> GetAllFiltersFromCategory(int CategoryID)
        {
            var respond = new
            {
                success = false,
                filters = new List<JSONCategoryFilter>()
            };
            try
            {
                var result = await _productService.GetAllFiltersFromCategory(CategoryID);
                respond = new
                {
                    success = true,
                    filters = result
                };
            }
            catch (Exception) { }
            
            return Json(respond);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            var respond = new
            {
                success = false,
                categories = ""
            };


            var result = await _productService.GetAllCategories();

 
            if(result.Count > 0)
            {
                string resultString = JsonConvert.SerializeObject(result);
                respond = new
                {
                    success = true,
                    categories = resultString
                };
            }
            return Json(respond);
         }
         
    }
}