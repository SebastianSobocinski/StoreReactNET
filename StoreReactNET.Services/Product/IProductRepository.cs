using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Product.Models.Inputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Product
{
    public interface IProductRepository
    {
        Task<ProductDTO> GetProduct(int ProductID);
        Task<List<ProductDTO>> GetProductsByCategoryWithFilters(int categoryId, List<JSONProductFilter> filters);
        Task<ClickedProductDTO> GetClickedProduct(int productId);
        Task<List<ProductDTO>> GetSearchedProducts(List<string> QueryArray);
        Task<List<JSONCategoryFilter>> GetAllFiltersFromCategory(int CategoryID);
        Task<List<CategoryDTO>> GetAllCategories();
    }
}
