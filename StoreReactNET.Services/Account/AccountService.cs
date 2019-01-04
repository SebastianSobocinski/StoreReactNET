using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Outputs;

namespace StoreReactNET.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            this._repository = repository;
        }

        public async Task<UserDTO> Login(string Email, string Password)
        {
            return await _repository.GetUserByCredentialsAsync(Email, SHA256Service.GetHashedString(Password));
        }

        public async Task Register(string Email, string Password)
        {
            var existingUser = await _repository.GetUserByEmailAsync(Email);

            if (existingUser != null)
                throw new Exception("User already exists.");
            else
                await _repository.RegisterUserAsync(Email, SHA256Service.GetHashedString(Password));

        }

        public async Task<UserDetailsDTO> GetUserDetails(string userID)
        {
            var details = await _repository.GetUserDetailsAsync(userID);

            if (details == null)
                throw new Exception("Can't find user details");
            else
                return details;
        }

        public async Task<List<UserAddressDTO>> GetUserAddresses(string userID)
        {
            var addresses = await _repository.GetUserAddressesAsync(userID);

            if (addresses.Count == 0)
                throw new Exception("Couldn't find any addresses");
            else
                return addresses;
        }

        public async Task<List<OrderDTO>> GetUserLatestOrders(string userID)
        {
            var orders = await _repository.GetUserOrders(userID);

            if (orders.Count == 0)
                throw new Exception("Couldn't find any orders");
            else
                return orders;
        }
    }
}
