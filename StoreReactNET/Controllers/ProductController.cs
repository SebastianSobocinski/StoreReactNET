using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                if (Filters != "null")
                {
                    var SelectedFilters = JsonConvert.DeserializeObject<ProductFilters>(Filters);
                    if(SelectedFilters.MaxPrice != null)
                    {
                        result = result
                                 .Where(c => (c.PriceVat * 1.23) <= SelectedFilters.MaxPrice)
                                 .ToList();
                    }
                    if (SelectedFilters.Brands.Count > 0)
                    {
                        result = result
                                 .Where(c => c.ProductDetailsId != null)
                                 .Where(c => SelectedFilters.Brands.Contains(c.ProductDetailsNavigation.Brand))
                                 .ToList();
                    }
                    if(SelectedFilters.Models.Count > 0)
                    {
                        result = result
                                 .Where(c => c.ProductDetailsId != null)
                                 .Where(c => SelectedFilters.Models.Contains(c.ProductDetailsNavigation.Model))
                                 .ToList();
                    }
                    if(SelectedFilters.VramList.Count > 0)
                    {
                        result = result
                                 .Where(c => c.ProductDetailsId != null)
                                 .Where(c => SelectedFilters.VramList.Contains(c.ProductDetailsNavigation.Vram.ToString()))
                                 .ToList();
                    }
                    if(SelectedFilters.BusWidthList.Count > 0)
                    {
                        result = result
                                 .Where(c => c.ProductDetailsId != null)
                                 .Where(c => SelectedFilters.BusWidthList.Contains(c.ProductDetailsNavigation.BusBandwith.ToString()))
                                 .ToList();
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
            var db = new StoreASPContext();
            var result = db.Products
                           .Include(c => c.ProductDetails)
                           .Where(c => c.ProductCategoryId == CategoryID)
                           .ToList<Products>();
           
            if (result.Count >= 0)
            {


                var brandsList = new List<string>();
                brandsList.Add("Brands");
                var modelsList = new List<string>();
                modelsList.Add("Models");
                var vramList = new List<string>();
                vramList.Add("VRAM");
                var busWidthList = new List<string>();
                busWidthList.Add("Bus Bandwith");

                foreach (var item in result)
                {
                    if (item.ProductDetailsNavigation != null)
                    {
                        if (item.ProductDetailsNavigation.Brand != null)
                        {
                            brandsList.Add(item.ProductDetailsNavigation.Brand);
                        }
                        if (item.ProductDetailsNavigation.Model != null)
                        {
                            modelsList.Add(item.ProductDetailsNavigation.Model);
                        }
                        if (item.ProductDetailsNavigation.Vram != null)
                        {
                            vramList.Add(item.ProductDetailsNavigation.Vram.ToString());
                        }
                        if (item.ProductDetailsNavigation.Vram != null)
                        {
                            busWidthList.Add(item.ProductDetailsNavigation.BusBandwith.ToString());
                        }

                    }
                }
                var filtersList = new
                {
                    brands = brandsList.Distinct().ToList(),
                    models = modelsList.Distinct().ToList(),
                    vramList = vramList.Distinct().ToList(),
                    busWidthList = busWidthList.Distinct().ToList()
                };
                
                //string filtersString = JsonConvert.SerializeObject(filters);
                var respond = new
                {
                    success = true,
                    filters = filtersList
                };
                return Json(respond);
            }
            return Json(null);
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