using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StoreReactNET.Infrastructure.EntityFramework;
using StoreReactNET.Infrastructure.EntityFramework.Queries;
using StoreReactNET.Services.Account.Models.Inputs;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.Infrastructure.Tests
{
    [TestFixture]
    public class AccountQueriesTests
    {
        private AccountQueries _accountQueries;
        private StoreASPContext _context;

        public AccountQueriesTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreASPContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=den1.mssql8.gear.host;Initial Catalog=storeasptests;User ID=storeasptests;Password=Sp3FG22Bkt_!");

            var context = new StoreASPContext(optionsBuilder.Options);

            _context = context;
            _accountQueries = new AccountQueries(
                context
                );
        }
        [Test]
        public async Task GetUserByCredentialsAsync_CorrectCredentials_ShouldReturnUser()
        {
            var correctEmail = "test@test";
            var correctPassword = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";

            var user = await _accountQueries.GetUserByCredentialsAsync(correctEmail, correctPassword);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task GetUserByCredentials_InvalidCredentials_ShouldReturnNull()
        {
            var invalidEmail = "invalid@email";
            var invalidPassword = "invalidPas.Sword123";

            var user = await _accountQueries.GetUserByCredentialsAsync(invalidEmail, invalidPassword);

            Assert.That(user == null);
        }
        [Test]
        public async Task GetUserByEmailAsync_ExistingUser_ShouldReturnUser()
        {
            var existingEmail = "test@test";

            var user = await _accountQueries.GetUserByEmailAsync(existingEmail);

            Assert.That(user != null && user.Email == existingEmail);
        }
        [Test]
        public async Task GetUserByEmailAsync_NonExistentUser_ShouldReturnNull()
        {
            var nonExistingEmail = "invalidEmail@invalid.com";

            var user = await _accountQueries.GetUserByEmailAsync(nonExistingEmail);

            Assert.That(user == null);
        }

        [Test]
        public async Task RegisterUserAsync_UserAlreadyExists_ShouldReturnExistingUser()
        {
            var correctEmail = "test@test";
            var correctPassword = "someTestPassword";

            await _accountQueries.RegisterUserAsync(correctEmail, correctPassword);
            var user = await _accountQueries.GetUserByEmailAsync(correctEmail);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task RegisterUserAsync_UserDoesntExists_ShouldReturnNewUser()
        {
            var correctEmail = "notExistingUser@test";
            var correctPassword = "someTestPassword";

            await _accountQueries.RegisterUserAsync(correctEmail, correctPassword);
            var user = await _accountQueries.GetUserByEmailAsync(correctEmail);

            Assert.That(user != null && user.Email == correctEmail);
        }
        [Test]
        public async Task GetUserDetailsAsync_UserDoesntExists_ShouldReturnNull()
        {
            var userId = "99test99";

            var details = await _accountQueries.GetUserByEmailAsync(userId);

            Assert.That(details == null);
        }

        [Test]
        public async Task GetUserDetailsAsync_UserDoesntHaveDetails_ShouldReturnNull()
        {
            var userId = "2006";

            var details = await _accountQueries.GetUserDetailsAsync(userId);

            Assert.That(details == null);
        }

        [Test]
        public async Task GetUserDetailsAsync_UserDoesHaveDetails_ShouldReturnUser()
        {
            var userId = "2005";

            var details = await _accountQueries.GetUserDetailsAsync(userId);

            Assert.That(details != null && details.FirstName == "Sebastian");
        }
        [Test]
        public async Task GetUserAddressesAsync_UserDoesntHaveAnyAddress_ShouldReturnEmptyList()
        {
            var userId = "2003";

            var addresses = await _accountQueries.GetUserAddressesAsync(userId);

            Assert.That(addresses.Count == 0);
        }
        [Test]
        public async Task GetUserAddressesAsync_UserDoesHaveAnyAddress_ShouldReturnAddressList()
        {
            var userId = "1";

            var addresses = await _accountQueries.GetUserAddressesAsync(userId);

            Assert.That(addresses.Count > 0);
        }
        [Test]
        public async Task GetUserOrders_UserDoesntHaveAnyOrder_ShouldReturnEmptyList()
        {
            var userId = "2004";

            var orders = await _accountQueries.GetUserOrders(userId);

            Assert.That(orders.Count == 0);
        }

        [Test]
        public async Task GetUserOrders_UserDoesHaveOrders_ShouldReturnOrdersList()
        {
            var userId = (await _context.Orders.FirstOrDefaultAsync()).UserId.ToString();

            var orders = await _accountQueries.GetUserOrders(userId);

            Assert.That(orders.Count > 0);
        }

        [Test]
        public async Task SetUserDetails_UserDoesntExists_ShouldReturnFalse()
        {
            var userId = 9999;

            var newFirstName = Faker.NameFaker.MaleFirstName();
            var newLastName = Faker.NameFaker.MaleName();
            var userDetailsDto = new UserDetailsViewModel()
            {
                DateOfBirth = new DateTime(1990, 1, 1),
                FirstName = newFirstName,
                LastName = newLastName
            };

            var succeed = await _accountQueries.SetUserDetails(userId, userDetailsDto);

            Assert.IsFalse(succeed);

        }

        [Test]
        public async Task SetUserDetails_UserExistsDoesntHaveDetails_ShouldReturnTrue()
        {
            var userId = 2004;

            var newFirstName = Faker.NameFaker.MaleFirstName();
            var newLastName = Faker.NameFaker.MaleName();
            var userDetailsDto = new UserDetailsViewModel()
            {
                DateOfBirth = new DateTime(1990, 1, 1),
                FirstName = newFirstName,
                LastName = newLastName
            };

            var succeed = await _accountQueries.SetUserDetails(userId, userDetailsDto);
            var userDetails = await _accountQueries.GetUserDetailsAsync(userId.ToString());

            Assert.IsTrue(succeed);
            Assert.That(userDetails.FirstName == newFirstName && userDetails.LastName == newLastName);
        }

        [Test]
        public async Task SetUserDetails_UserExistsDoesHaveDetails_ShouldOverrideAndReturnTrue()
        {
            var userId = 1;

            var newFirstName = Faker.NameFaker.MaleFirstName();
            var newLastName = Faker.NameFaker.MaleName();
            var userDetailsDto = new UserDetailsViewModel()
            {
                DateOfBirth = new DateTime(1990, 1, 1),
                FirstName = newFirstName,
                LastName = newLastName
            };

            var succeed = await _accountQueries.SetUserDetails(userId, userDetailsDto);
            var userDetails = await _accountQueries.GetUserDetailsAsync(userId.ToString());

            Assert.IsTrue(succeed);
            Assert.That(userDetails.FirstName == newFirstName && userDetails.LastName == newLastName);
        }

        [Test]
        public async Task SetAddress_AddressIdNotNullDoesntExists_ShouldReturnFalse()
        {
            var userId = 1;
            var userAddress = new UserAddressDTO()
            {
                Id = 9999
            };

            var result = await _accountQueries.SetAddress(userId, userAddress);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetAddress_AddressIdNotNullDoesExists_ShouldReturnTrue()
        {
            var userId = 1;
            var userAddress = new UserAddressDTO()
            {
                Id = (await _context.UserAdresses.FirstOrDefaultAsync(c => c.UserId == userId)).Id,
                AppartmentNr = Faker.NumberFaker.Number().ToString(),
                City = Faker.LocationFaker.City(),
                Country = Faker.LocationFaker.Country(),
                HomeNr = Faker.NumberFaker.Number().ToString(),
                StreetName = Faker.LocationFaker.StreetName(),
                Zipcode = Faker.LocationFaker.ZipCode()
            };

            var result = await _accountQueries.SetAddress(userId, userAddress);

            Assert.IsTrue(result);
        }
        [Test]
        public async Task SetAddress_AddressIdNull_ShouldReturnTrue()
        {
            var userId = 1;
            var userAddress = new UserAddressDTO()
            {
                AppartmentNr = Faker.NumberFaker.Number().ToString(),
                City = Faker.LocationFaker.City(),
                Country = Faker.LocationFaker.Country(),
                HomeNr = Faker.NumberFaker.Number().ToString(),
                StreetName = Faker.LocationFaker.StreetName(),
                Zipcode = Faker.LocationFaker.ZipCode()
            };

            var result = await _accountQueries.SetAddress(userId, userAddress);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveUserAddress_AddressDoesntExists_ShouldReturnFalse()
        {
            var userId = 1;
            var addressId = 99999999;

            var result = await _accountQueries.RemoveUserAddress(userId, addressId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveUserAddress_AddressDoesExists_ShouldReturnTrue()
        {
            var userId = 1;
            var addressId = (await _context.UserAdresses.FirstOrDefaultAsync(c => c.UserId == userId)).Id;

            var result = await _accountQueries.RemoveUserAddress(userId, addressId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task SubmitOrder_CartIsEmpty_ReturnsFalse()
        {
            var userId = 1;
            var cart = new List<CartProductDTO>();
            var sentOrder = new SentOrderViewModel();

            var result = await _accountQueries.SubmitOrder(userId, cart, sentOrder);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SubmitOrder_UserDoesntExists_ReturnsFalse()
        {
            var userId = 99999999;
            var cart = new List<CartProductDTO>()
            {
                new CartProductDTO()
                {
                    ProductID = "1",
                    Quantity = 5
                }
            };
            var sentOrder = new SentOrderViewModel()
            {
                AddressID = 1
            };

            var result = await _accountQueries.SubmitOrder(userId, cart, sentOrder);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task SubmitOrder_CorrectEntries_ReturnsTrue()
        {
            var userId = 1;
            var cart = new List<CartProductDTO>()
            {
                new CartProductDTO()
                {
                    ProductID = "1",
                    Quantity = 5
                }
            };
            var sentOrder = new SentOrderViewModel()
            {
                AddressID = (await _context.UserAdresses.FirstOrDefaultAsync(c => c.UserId == 1)).Id
            };

            var result = await _accountQueries.SubmitOrder(userId, cart, sentOrder);

            Assert.IsTrue(result);
        }
    }
}
