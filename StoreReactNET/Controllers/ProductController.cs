using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreReactNET.Models;
using StoreReactNET.Models.Database;
using StoreReactNET.Models.ViewModels;

namespace StoreReactNET.Controllers
{
    [Route("Product/[action]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public ActionResult GetProducts(int CategoryID, int Page, string Filters)
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
                           .Include(c => c.ProductDetails)
                           .ToList();

            if(result.Count > 0)
            {
                var selectedFilters = JsonConvert.DeserializeObject<List<JSONProductFilter>>(Filters);
                if(selectedFilters != null)
                {
                    foreach (var filter in selectedFilters)
                    {
                        if(filter.Value.Count > 0)
                        {
                            if (filter.Type == "maxPrice")
                            {
                                try
                                {
                                    double maxPrice = double.Parse(filter.Value[0], CultureInfo.InvariantCulture);
                                    result = result
                                         .Where(c => (c.PriceVat * 1.23) <= maxPrice)
                                         .ToList();
                                }
                                catch (Exception) { }

                            }
                            else
                            {
                                result = result
                                         .Where(c => c.ProductDetailsId != null && c.ProductDetails.Count > 0)
                                         .Where(c => (
                                                      filter.Value.Contains
                                                      (c.ProductDetailsNavigation
                                                      .GetType()
                                                      .GetProperty(filter.Type)
                                                      .GetValue(c.ProductDetailsNavigation, null)
                                                      ))   
                                                )
                                         .ToList();
                            }
                        }
                        
                    }
                }
                
               
                result = result
                         .Take(Page * 10)
                         .Reverse()
                         .ToList();

                result = result
                         .Take(10)
                         .ToList();

                var ProductsList = new List<ProductViewModel>();
                foreach (var item in result)
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
        public ActionResult GetAllFiltersFromCategory(int CategoryID)
        {
            var respond = new
            {
                success = false,
                filters = new List<JSONCategoryFilter>()
            };

            var db = new StoreASPContext();
            var result = db.Products
                           .Include(c => c.ProductDetails)
                           .Where(c => c.ProductCategoryId == CategoryID)
                           .ToList<Products>();

            if(result.Count > 0)
            {
                var filtersRequired = Singleton.FiltersRequired[CategoryID];

                var filtersHolder = new JSONCategoryFilters();
                foreach (var required in filtersRequired)
                {
                    filtersHolder.Filters.Add(new JSONCategoryFilter(required));
                }

                foreach (var item in result)
                {
                    if (item.ProductDetailsId != null && item.ProductDetails.Count > 0)
                    {
                        foreach (var required in filtersRequired)
                        {
                            var filter = filtersHolder.GetFilter(required);
                            string value = item.ProductDetailsNavigation
                                    .GetType()
                                    .GetProperty(required)
                                    .GetValue(item.ProductDetailsNavigation, null)
                                    .ToString();

                            filter.Variables.Add(value);
                        }
                    }
                }
                filtersHolder.DistinctFilters();
                respond = new
                {
                    success = true,
                    filters = filtersHolder.Filters
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