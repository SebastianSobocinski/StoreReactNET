using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreReactNET.Services;
using StoreReactNET.Services.Product;
using StoreReactNET.Services.Product.Models.Inputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Infrastructure.EntityFramework.Queries
{
    public class ProductQueries : IProductQueries
    {
        private readonly StoreASPContext _context;
        public ProductQueries(StoreASPContext context)
        {
            this._context = context;
        }

        public async Task<ProductDTO> GetProduct(int ProductID)
        {
            var product = await _context.Products
                .Where(c => c.Id == ProductID)
                .Include(c => c.ProductCategory)
                .Include(c => c.ProductImages)
                .Include(c => c.ProductDetails)
                .FirstOrDefaultAsync();

            return new ProductDTO()
            {
                ProductCategoryID = product.ProductCategoryId.ToString(),
                ProductCategoryName = product.ProductCategory.CategoryName,
                ProductDescription = product.Description,
                ProductID = product.Id.ToString(),
                ProductImages = product.ProductImages
                    .Select(image => image.ImageName)
                    .ToList(),
                ProductName = product.Name,
                ProductPrice = product.PriceVat
            };

        }

        public async Task<List<ProductDTO>> GetProductsByCategoryWithFilters(int categoryId, List<JSONProductFilter> filters)
        {
            //gets all products from category
            var dbResult = await _context
                .Products
                .Where(c => c.ProductCategoryId == categoryId)
                .Include(c => c.ProductCategory)
                .Include(c => c.ProductImages)
                .Include(c => c.ProductDetails)
                .ToListAsync();

            foreach (var filter in filters)
            {
                //checks if any filters
                if (filter.Value.Count > 0)
                {
                    if (filter.Type == "maxPrice")
                    {
                        try
                        {
                            double maxPrice = double.Parse(filter.Value[0], CultureInfo.InvariantCulture);
                            dbResult = dbResult
                                .Where(c => (c.PriceVat * 1.23) <= maxPrice)
                                .ToList();
                        }
                        catch (Exception) { }

                    }
                    //filtering by other filters than maxPrice
                    else
                    {
                        //checking by db column name from filter and comparing if matches filter
                        dbResult = dbResult
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

            var result = new List<ProductDTO>();
            foreach (var item in dbResult)
            {
                var entry = new ProductDTO()
                {
                    ProductCategoryID = item.ProductCategoryId.ToString(),
                    ProductCategoryName = item.ProductCategory.CategoryName,
                    ProductDescription = item.Description,
                    ProductID = item.Id.ToString(),
                    ProductImages = item.ProductImages
                        .Select(image => image.ImageName)
                        .ToList(),
                    ProductName = item.Name,
                    ProductPrice = item.PriceVat

                };
                result.Add(entry);
            }

            return result;
        }

        public async Task<ClickedProductDTO> GetClickedProduct(int productId)
        {
            var product = await _context.Products
                .Where(c => c.Id == productId)
                .Include(c => c.ProductCategory)
                .Include(c => c.ProductImages)
                .Include(c => c.ProductDetails)
                .FirstOrDefaultAsync();


            var productDetailsList = new List<List<string>>();
            try
            {
                PropertyInfo[] properties = product.ProductDetailsNavigation.GetType().GetProperties();
                foreach (var p in properties)
                {
                    var value = p.GetValue(product.ProductDetailsNavigation, null);
                    if (value != null)
                    {
                        try
                        {
                            var temp = new List<string>();
                            temp.Add(Singleton.FiltersClickedProductName[p.Name]);
                            temp.Add(value.ToString());
                            productDetailsList.Add(temp);
                        }
                        catch (Exception) { }
                    }
                }
                
            }
            catch (Exception) { }

            var productDto = new ClickedProductDTO()
            {
                ProductCategoryID = product.ProductCategoryId.ToString(),
                ProductCategoryName = product.ProductCategory.CategoryName,
                ProductDescription = product.Description,
                ProductID = product.Id.ToString(),
                ProductImages = product.ProductImages
                    .Select(image => image.ImageName)
                    .ToList(),
                ProductName = product.Name,
                ProductPrice = product.PriceVat,
                ProductDetailsList = productDetailsList
            };

            return productDto;
        }

        public async Task<List<ProductDTO>> GetSearchedProducts(List<string> QueryArray)
        {
            if (QueryArray.Count > 0)
            {
                QueryArray = QueryArray
                    .Select(c => c.ToLowerInvariant())
                    .ToList();

                //getting products with filters
                var result = await _context.Products
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
                    .Select(c => new ProductDTO()
                    {
                        ProductCategoryID = c.ProductCategoryId.ToString(),
                        ProductCategoryName = c.ProductCategory.CategoryName,
                        ProductDescription =  c.Description,
                        ProductID = c.Id.ToString(),
                        ProductImages = c.ProductImages
                            .Select(image => image.ImageName)
                            .ToList(),
                        ProductName = c.Name,
                        ProductPrice = c.PriceVat
                    })
                    .ToListAsync();

                return result;
            }
            return null;
        }

        public async Task<List<JSONCategoryFilter>> GetAllFiltersFromCategory(int CategoryID)
        {
            var result = await _context.Products
                .Include(c => c.ProductDetails)
                .Where(c => c.ProductCategoryId == CategoryID)
                .ToListAsync();

            if (result.Count > 0)
            {
                //getting required filters
                var filtersRequired = Singleton.FiltersRequired[CategoryID];
                var filtersHolder = new JSONCategoryFilters();
                foreach (var required in filtersRequired)
                {
                    filtersHolder.Filters.Add(new JSONCategoryFilter(required));
                }
                //collecting details from all project where not null
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
                return filtersHolder.Filters;
            }
            return null;
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var result = await _context.ProductCategories
                .Select(c => new CategoryDTO()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();

            return result;
        }
    }
}
