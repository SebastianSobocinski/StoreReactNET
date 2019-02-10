using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Product.Models.Inputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductQueries _queries;
        public ProductService(IProductQueries queries)
        {
            this._queries = queries;
        }
        public async Task<List<ProductDTO>> GetProducts(int CategoryID, int Page, List<JSONProductFilter> Filters, string OrderBy)
        {
            var products = await _queries.GetProductsByCategoryWithFilters(CategoryID, Filters);

            //sorting
            switch (OrderBy)
            {
                case "relevance":
                    products = products.OrderBy(c => int.Parse(c.ProductID)).ToList();
                    break;
                case "toLower":
                    products = products.OrderByDescending(c => c.ProductPrice).ToList();
                    break;
                case "toHigher":
                    products = products.OrderBy(c => c.ProductPrice).ToList();
                    break;
                default:
                    break;
            }
            //paginating

            products = products
                .Take(Page * 10)
                .ToList();

            return products;
        }
        public async Task<ClickedProductDTO> GetClickedProduct(int ProductID)
        {
            var product = await _queries.GetClickedProduct(ProductID);

            if(product == null)
                throw new Exception("Couldn't find product");

            return product;
        }
        public async Task<List<ProductDTO>> GetSearchedProducts(List<string> QueryArray, int Page, string OrderBy)
        {
            var products = await _queries.GetSearchedProducts(QueryArray);

            if (products == null)
                throw new Exception("Couldn't find searched products");

            //sorting
            switch (OrderBy)
            {
                case "relevance":
                    products.Sort(delegate (ProductDTO p1, ProductDTO p2)
                    {
                        //checking how many matching conditions each product have
                        int p1Completed = 0;
                        int p2Completed = 0;
                        foreach (var condition in QueryArray)
                        {
                            if (p1.ProductName.Replace(" ", "").ToLowerInvariant().Contains(condition))
                            {
                                p1Completed++;
                            }
                            if (p1.ProductCategoryName.Replace(" ", "").ToLowerInvariant().Contains(condition))
                            {
                                p1Completed++;
                            }
                            if (p2.ProductName.Replace(" ", "").ToLowerInvariant().Contains(condition))
                            {
                                p2Completed++;
                            }
                            if (p2.ProductCategoryName.Replace(" ", "").ToLowerInvariant().Contains(condition))
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
                            //if same number compare productID
                            return (int.Parse(p1.ProductID))
                                .CompareTo(
                                    int.Parse(p2.ProductID)
                                    );
                        }
                    });
                    break;
                case "toLower":
                    products = products.OrderByDescending(c => c.ProductPrice).ToList();
                    break;
                case "toHigher":
                    products = products.OrderBy(c => c.ProductPrice).ToList();
                    break;
                default:
                    break;
            }
            //paging
            var ItemsByPage = new List<ProductDTO>();
            for (int i = (Page - 1) * 10; i < (Page * 10); i++)
            {
                try
                {
                    ItemsByPage.Add(products[i]);
                }
                catch (Exception) { }
            }

            return ItemsByPage;
        }
        public async Task<List<JSONCategoryFilter>> GetAllFiltersFromCategory(int CategoryID)
        {
            var filters = await _queries.GetAllFiltersFromCategory(CategoryID);

            if(filters == null)
                throw new Exception("Couldn't find any filters");

            return filters;
        }
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await _queries.GetAllCategories();

            if(categories == null)
                throw new Exception("Couldn't find categories");

            return categories;
        }
    }
}
