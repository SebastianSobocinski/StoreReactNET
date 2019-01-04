using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Product.Models.Inputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Product
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProducts(int CategoryID, int Page, List<JSONProductFilter> Filters, string OrderBy);
        Task<ClickedProductDTO> GetClickedProduct(int ProductID);
        Task<List<ProductDTO>> GetSearchedProducts(List<string> QueryArray, int Page, string OrderBy);
        Task<List<JSONCategoryFilter>> GetAllFiltersFromCategory(int CategoryID);
        Task<List<CategoryDTO>> GetAllCategories();
    }
}
