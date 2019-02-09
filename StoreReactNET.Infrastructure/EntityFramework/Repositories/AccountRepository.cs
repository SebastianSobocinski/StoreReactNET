
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreReactNET.Services.Account;
using StoreReactNET.Infrastructure.EntityFramework.Entities;
using StoreReactNET.Services;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Infrastructure.EntityFramework.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly StoreASPContext _context;
        public AccountRepository(StoreASPContext context)
        {
            this._context = context;
        }

        public async Task<UserDTO> GetUserByCredentialsAsync(string Email, string HashedPassword)
        {
            var result = await _context.Users
                    .Where(c => c.Email == Email && c.Password == HashedPassword)
                    .Include(c => c.UserDetails)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                var respond = new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails?.Name ?? "",
                    LastName = result.UserDetails?.FullName ?? ""
                };
                return respond;
            }

            return null;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string Email)
        {
            var result = await _context.Users
                    .Where(c => c.Email == Email)
                    .Include(c => c.UserDetails)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                return new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails.Name,
                    LastName = result.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task<UserDTO> GetUserByIDAsync(string userID)
        {
            var result = await _context.Users
                    .Include(c => c.UserDetails)
                    .Where(c => c.Id.ToString() == userID)
                    .FirstOrDefaultAsync();

            if (result != null)
            {
                return new UserDTO()
                {
                    ID = result.Id.ToString(),
                    Email = result.Email,
                    FirstName = result.UserDetails.Name,
                    LastName = result.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task RegisterUserAsync(string Email, string HashedPassword)
        {
            var entry = new Users
            {
                Email = Email,
                Password = HashedPassword
            };

            await _context.Users.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDetailsDTO> GetUserDetailsAsync(string userID)
        {
            var user = await _context.Users
                    .Include(c => c.UserDetails)
                    .Where(c => userID == c.Id.ToString())
                    .FirstOrDefaultAsync();

            if (user.UserDetails != null)
            {
                return new UserDetailsDTO()
                {
                    FirstName = user.UserDetails.Name,
                    DateOfBirth = user.UserDetails.DateOfBirth,
                    LastName = user.UserDetails.FullName
                };
            }

            return null;
        }

        public async Task<List<UserAddressDTO>> GetUserAddressesAsync(string userID)
        {
            var result = await _context.UserAdresses
                .Where(c => userID == c.UserId.ToString())
                .ToListAsync();

            var resultDto = new List<UserAddressDTO>();
            foreach (var item in result)
            {
                var entry = new UserAddressDTO()
                {
                    Id = item.Id,
                    AppartmentNr = item.AppartmentNr,
                    City = item.City,
                    HomeNr = item.HomeNr,
                    Country = item.Country,
                    StreetName = item.StreetName,
                    Zipcode = item.Zipcode
                };
                resultDto.Add(entry);
            }

            return resultDto;
        }

        public async Task<List<OrderDTO>> GetUserOrders(string userID)
        {
            //getting all orders
            var orders = await _context.Orders
                .Where(c => userID == c.UserId.ToString())
                .OrderByDescending(c => c.Id)
                .Take(10).ToListAsync();

            var ordersDto = new List<OrderDTO>();
            foreach (var item in orders)
            {
                //getting all products in order
                var orderItems = await _context.OrderItems
                    .Where(c => c.OrderId == item.Id)
                    .ToListAsync();

                //init total price value and list of products in order
                var temp = new List<OrderItemDTO>();
                var totalPrice = 0.00;

                foreach (var product in orderItems)
                {
                    //for each product in order we find matching product in db
                    var dbproduct = await _context.Products
                        .Where(c => c.Id == product.ProductId)
                        .FirstOrDefaultAsync();

                    //creating new entry to temp list
                    var entry = new OrderItemDTO
                    {
                        ProductID = dbproduct.Id,
                        ProductName = dbproduct.Name,
                        Quantity = product.Quantity,
                        //calculates quantity * product price * vat of product
                        ProductTotalPrice = dbproduct.PriceVat * 1.23 * product.Quantity
                    };
                    //adds product to temp array
                    temp.Add(entry);
                    //adds to total price of order
                    totalPrice += entry.ProductTotalPrice;
                }
                //creates new entry of order
                var ordersEntry = new OrderDTO
                {
                    Date = item.Date.ToShortDateString(),
                    OrderID = item.Id,
                    OrderProducts = temp,
                    Status = Singleton.OrderStatuses[item.Status],
                    TotalPrice = totalPrice
                };
                ordersDto.Add(ordersEntry);

            }

            return ordersDto;

        }

        public async Task<bool> SetUserDetails(int userId, UserDetailsViewModel userDetailsViewModel)
        {
            var user = await _context.Users
                .Include(c => c.UserDetails)
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null)
                return false;

            if (user.UserDetailsId == null)
            {
                var entry = new UserDetails()
                {
                    Name = userDetailsViewModel.FirstName,
                    FullName = userDetailsViewModel.LastName,
                    DateOfBirth = userDetailsViewModel.DateOfBirth
                };
                await _context.UserDetails.AddAsync(entry);
                await _context.SaveChangesAsync();
                user.UserDetailsId = entry.Id;
            }
            else
            {
                user.UserDetails.Name = userDetailsViewModel.FirstName;
                user.UserDetails.FullName = userDetailsViewModel.LastName;
                user.UserDetails.DateOfBirth = userDetailsViewModel.DateOfBirth;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetAddress(int userId, UserAddressDTO userAddress)
        {
            if (userAddress.Id != null)
            {
                var result = await _context.UserAdresses
                    .FirstOrDefaultAsync(
                        c =>
                            c.UserId == userId
                            &&
                            c.Id == userAddress.Id
                    );
                if (result != null)
                {
                    result.StreetName = userAddress.StreetName;
                    result.HomeNr = userAddress.HomeNr;
                    result.AppartmentNr = userAddress.AppartmentNr;
                    result.Zipcode = userAddress.Zipcode;
                    result.City = userAddress.City;
                    result.Country = userAddress.Country;
                }
                else
                    return false;
            }
            else
            {
                var entry = new UserAdresses()
                {
                    UserId = userId,
                    StreetName = userAddress.StreetName,
                    HomeNr = userAddress.HomeNr,
                    AppartmentNr = userAddress.AppartmentNr,
                    Zipcode = userAddress.Zipcode,
                    City = userAddress.City,
                    Country = userAddress.Country
                };
                await _context.UserAdresses.AddAsync(entry);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserAddress(int userId, int addressId)
        {
            var result = await _context.UserAdresses
                .FirstOrDefaultAsync(
                    c =>
                        c.Id == addressId
                        &&
                        c.UserId == userId
                );

            if (result == null)
                return false;

            //sets user id null instead of delete entry
            result.UserId = null;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubmitOrder(int userId, List<CartProductDTO> cart, SentOrderViewModel sentOrder)
        {
            var result = await _context.UserAdresses
                .FirstOrDefaultAsync(
                    c => 
                        c.Id == sentOrder.AddressID
                        &&
                        c.UserId == userId
                );

            if (result == null || cart == null || cart.Count <= 0)
                return false;

            var orderEntry = new Orders()
            {
                UserId = userId,
                UserAddressId = sentOrder.AddressID,
                Date = DateTime.Now,
                Status = 0
            };
            await _context.Orders.AddAsync(orderEntry);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var entryItem = new OrderItems()
                {
                    OrderId = orderEntry.Id,
                    ProductId = int.Parse(item.ProductID),
                    Quantity = item.Quantity
                };
                await _context.OrderItems.AddAsync(entryItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
