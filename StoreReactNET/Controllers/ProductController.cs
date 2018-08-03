using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.Models.Database;
using StoreReactNET.Models.ViewModels;

namespace StoreReactNET.Controllers
{
    [Route("Product/[action]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public ActionResult GetProductsByPage(int CategoryID, int page)
        {
            var respond = new
            {
                success = false,
                products = ""
            };
            var db = new StoreASPContext();

            var result = db.Products
                           .Where(c => c.ProductCategoryId == CategoryID)
                           .Include(c => c.ProductCategory)
                           .Include(c => c.ProductImages)
                           .Take(page * 10)
                           .ToList();



            if(result.Count >= 0)
            {
                var ProductsList = new List<ProductViewModel>();

                foreach(var item in result)
                {
                    ProductsList.Add(new ProductViewModel(item));
                }
                string ProductsListString = JsonConvert.SerializeObject(ProductsList);

                respond = new
                {
                    success = true,
                    products = ProductsListString
                };
            }

            return Json(respond);
        }
        [HttpGet]
        public ActionResult GetAllCategories()
        {
            var respond = new
            {
                success = false,
                categories = ""
            };

            var db = new StoreASPContext();
            var result = db.ProductCategories.ToList();

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