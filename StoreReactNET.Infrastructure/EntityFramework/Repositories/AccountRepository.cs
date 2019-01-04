
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreReactNET.Services.Account;
using StoreReactNET.Infrastructure.EntityFramework.Entities;
using StoreReactNET.Services;
using StoreReactNET.Services.Account.Models;
using StoreReactNET.Services.Account.Models.Outputs;

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
    }
}
