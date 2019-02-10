using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountQueries _queries;
        public AccountService(IAccountQueries queries)
        {
            this._queries = queries;
        }

        public async Task<UserDTO> Login(string Email, string Password)
        {
            return await _queries.GetUserByCredentialsAsync(Email, SHA256Service.GetHashedString(Password));
        }

        public async Task Register(string Email, string Password)
        {
            var existingUser = await _queries.GetUserByEmailAsync(Email);

            if (existingUser != null)
                throw new Exception("User already exists.");
            else
                await _queries.RegisterUserAsync(Email, SHA256Service.GetHashedString(Password));

        }

        public async Task<UserDetailsDTO> GetUserDetails(string userID)
        {
            var details = await _queries.GetUserDetailsAsync(userID);

            if (details == null)
                throw new Exception("Can't find user details");
            else
                return details;
        }

        public async Task<List<UserAddressDTO>> GetUserAddresses(string userID)
        {
            var addresses = await _queries.GetUserAddressesAsync(userID);

            if (addresses.Count == 0)
                throw new Exception("Couldn't find any addresses");
            else
                return addresses;
        }

        public async Task<List<OrderDTO>> GetUserLatestOrders(string userID)
        {
            var orders = await _queries.GetUserOrders(userID);

            if (orders.Count == 0)
                throw new Exception("Couldn't find any orders");
            else
                return orders;
        }

        public async Task SetUserDetails(int userId, UserDetailsViewModel userDetailsViewModel)
        {
            var succeed = await _queries.SetUserDetails(userId, userDetailsViewModel);

            if(!succeed)
                throw new Exception("Couldn't set user details!");
        }

        public async Task SetAddress(int userId, UserAddressDTO userAddress)
        {
            var succeed = await _queries.SetAddress(userId, userAddress);

            if(!succeed)
                throw new Exception("Couldn't set user address!");
        }

        public async Task RemoveUserAddress(int userId, int addressId)
        {
            var succeed = await _queries.RemoveUserAddress(userId, addressId);

            if(!succeed)
                throw new Exception("Couldn't remove user address!");
        }

        public async Task SubmitOrder(int userId, List<CartProductDTO> cart, SentOrderViewModel sentOrder)
        {
            var succeed = await _queries.SubmitOrder(userId, cart, sentOrder);

            if(!succeed)
                throw new Exception("Couldn't submit order!");
        }
    }
}
