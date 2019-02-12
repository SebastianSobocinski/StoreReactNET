using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using StoreReactNET.Services.Account;
using StoreReactNET.Services.Account.Models.Outputs;
using Assert = NUnit.Framework.Assert;

namespace StoreReactNET.Services.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private AccountService _service;
        private Mock<IAccountQueries> _queries;

        [SetUp]
        public void Setup()
        {
            _queries = new Mock<IAccountQueries>();
            var hashedTestPassword = SHA256Service.GetHashedString("test");
            _queries.Setup(c => c.GetUserByCredentialsAsync("test", hashedTestPassword)).ReturnsAsync(new UserDTO()
            {
                Email = "test@test",
                FirstName = "Test",
                ID = "1",
                LastName = "TestL"
            });
            _queries.Setup(c => c.GetUserByEmailAsync("test@test")).ReturnsAsync(new UserDTO()
            {
                Email = "test@test"
            });
            _queries.Setup(c => c.GetUserDetailsAsync("1")).ReturnsAsync(new UserDetailsDTO()
            {
                FirstName = "TestName",
                LastName = "TestSurename"
            });
            _queries.Setup(c => c.GetUserAddressesAsync("1")).ReturnsAsync(new List<UserAddressDTO>()
            {
                new UserAddressDTO()
                {
                    StreetName = "TestStreetName"
                }
            });
            _service = new AccountService(_queries.Object);
        }
        [Test]
        public async Task Login_CorrectCredentials_ShouldReturnUser()
        { 
            var result = await _service.Login("test", "test");

            Assert.That(result != null && result.ID == "1" && result.Email == "test@test");
        }

        [Test]
        public async Task Login_IncorrectCredentials_ShouldReturnNull()
        {
            var result = await _service.Login("test", "wrongPassword");

            Assert.IsNull(result);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task Register_EmailAlreadyExists_ShouldThrowException()
        {
            var existingEmail = "test@test";
            var password = "test123!";

            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.Register(existingEmail, password));

            Assert.That(ex.Message, Is.EqualTo("User already exists."));
        }

        [Test]
        public async Task Register_EmailDoesntExists_ShouldntThrowException()
        {
            var notexistingEmail = "test1@test";
            var password = "test123!";

            await _service.Register(notexistingEmail, password);

            Assert.That(true);
        }

        [Test]
        public async Task GetUserDetails_DetailsDoesExists_ShouldReturnDetails()
        {
            var existingUser = "1";

            var result = await _service.GetUserDetails(existingUser);

            Assert.That(result != null && result.FirstName == "TestName" && result.LastName == "TestSurename");
        }
        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task GetUserDetails_DetailsDoesntExists_ShouldThrowException()
        {
            var notexistingUser = "2";

            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.GetUserDetails(notexistingUser));

            Assert.That(ex.Message, Is.EqualTo("Can't find user details"));
        }
        [Test]
        public async Task GetUserAddresses_AddressesDoesExists_ShouldReturnListOfAddresses()
        {
            var existingAddressesUserId = "1";

            var result = await _service.GetUserAddresses(existingAddressesUserId);

            Assert.That(result.Count == 1 && result[0].StreetName == "TestStreetName");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task GetUserAddresses_AddressesDoesntExists_ShouldThrowException()
        {
            var notexistingAddressesUserId = "2";

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _service.GetUserAddresses(notexistingAddressesUserId));

            Assert.That(ex.Message, Is.EqualTo("Couldn't find any addresses"));
        }

        public async Task GetUserLatestOrders_UserDoesHaveOrders_ShouldReturnOrderList()
        {

        }


    }
}
