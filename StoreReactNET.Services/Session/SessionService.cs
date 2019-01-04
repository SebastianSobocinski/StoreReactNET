using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Product;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IProductRepository _repository;
        public SessionService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductDTO> GetCartProduct(int ProductID)
        {
            var product = await _repository.GetProduct(ProductID);

            if(product == null)
                throw new Exception("Couldn't find product");

            return product;
        }
    }
}
