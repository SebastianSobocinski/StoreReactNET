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
        public ActionResult GetProducts(int CategoryID, int Page, string Filters, string OrderBy)
        {
            var respond = new
            {
                success = false,
                products = new List<ProductViewModel>()
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
                                                      .ToString()
                                                      ))   
                                                )
                                         .ToList();
                            }
                        }
                        
                    }
                }

                switch (OrderBy)
                {
                    case "relevance":
                        result = result.OrderBy(c => c.Id).ToList();
                        break;
                    case "toLower":
                        result = result.OrderByDescending(c => c.PriceVat).ToList();
                        break;
                    case "toHigher":
                        result = result.OrderBy(c => c.PriceVat).ToList();
                        break;
                    default:
                        break; 
                }

                result = result
                         .Take(Page * 10)
                         .ToList();
               
                
                var ItemsByPage = new List<ProductViewModel>();
                for(int i = (Page - 1) * 10; i < (Page * 10); i++)
                {
                    try
                    {
                        ItemsByPage.Add(new ProductViewModel(result[i]));
                    }
                    catch(Exception) { }
                }
                respond = new
                {
                    success = true,
                    products = ItemsByPage
                };
            }
            
            return Json(respond);
        }
        [HttpGet]
        public ActionResult GetSearchedProducts(string Query, int Page, string OrderBy)
        {
            var respond = new
            {
                success = false,
                products = new List<ProductViewModel>()
            };
            if (Query != null)
            {
                var QueryArray = JsonConvert.DeserializeObject<List<string>>(Query);
                
                if(QueryArray.Count > 0)
                {
                    QueryArray = QueryArray.Select(c => c.ToLowerInvariant()).ToList();
                    var db = new StoreASPContext();
                    var result = db.Products
                                   .Include(c => c.ProductCategory)
                                   .Include(c => c.ProductImages)
                                   .Include(c => c.ProductDetails)
                                   .Where(c =>
                                        QueryArray.Any(el => c.Name.Replace(" ", "")
                                                                   .ToLowerInvariant()
                                                                   .Contains(el)
                                                                   )
                                        ||
                                        QueryArray.Any(el => c.ProductCategory
                                                                  .CategoryName
                                                                  .Replace(" ", "")
                                                                  .ToLowerInvariant()
                                                                  .Contains(el)
                                                                  )
                                        )
                                   .ToList();

                    if (result.Count > 0)
                    {
                        switch (OrderBy)
                        {
                            case "relevance":
                                result.Sort(delegate (Products p1, Products p2)
                                {
                                    int p1Completed = 0;
                                    int p2Completed = 0;
                                    foreach (var condition in QueryArray)
                                    {
                                        if (p1.Name.Replace(" ", "").ToLowerInvariant().Contains(condition))
                                        {
                                            p1Completed++;
                                        }
                                        if (p1.ProductCategory.CategoryName.Replace(" ", "").ToLowerInvariant().Contains(condition))
                                        {
                                            p1Completed++;
                                        }
                                        if (p2.Name.Replace(" ", "").ToLowerInvariant().Contains(condition))
                                        {
                                            p2Completed++;
                                        }
                                        if (p2.ProductCategory.CategoryName.Replace(" ", "").ToLowerInvariant().Contains(condition))
                                        {
                                            p2Completed++;
                                        }

                                    }
                                    if (p1Completed > p2Completed)
                                    {
                                        return -1;
                                    }
                                    else if (p2Completed > p1Completed)
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        return p1.Id.CompareTo(p2.Id);
                                    }
                                });
                                break;
                            case "toLower":
                                result = result.OrderByDescending(c => c.PriceVat).ToList();
                                break;
                            case "toHigher":
                                result = result.OrderBy(c => c.PriceVat).ToList();
                                break;
                            default:
                                break;
                        }



                        var ItemsByPage = new List<ProductViewModel>();
                        for (int i = (Page - 1) * 10; i < (Page * 10); i++)
                        {
                            try
                            {
                                ItemsByPage.Add(new ProductViewModel(result[i]));
                            }
                            catch (Exception) { }
                        }
                        respond = new
                        {
                            success = true,
                            products = ItemsByPage
                        };
                    }

                }



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
                try
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
                                string value = null;
                                try
                                {
                                    value = item.ProductDetailsNavigation
                                        .GetType()
                                        .GetProperty(required)
                                        .GetValue(item.ProductDetailsNavigation, null)
                                        .ToString();
                                }
                                catch (Exception) { }

                                if (value != null)
                                {
                                    filter.Variables.Add(value);
                                }
                                
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
                catch (Exception) { }  
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