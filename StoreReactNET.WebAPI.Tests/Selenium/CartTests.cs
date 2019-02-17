using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using StoreReactNET.Services.Account.Models.Outputs;
using StoreReactNET.Services.Product.Models.Outputs;

namespace StoreReactNET.WebAPI.Tests.Selenium
{
    public class CartTests
    {
        private IWebDriver _driver;
        private readonly string test_email_correct = "test@test";
        private readonly string test_password_correct = "test";

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver("./");
        }

        [Test]
        public void AddToCart_NotLoggedUser_ShouldDisplayAlert()
        {
            Assert.That(this.AddToCart(), Is.EqualTo("Please log in."));
        }
        [Test]
        public void AddToCart_LoggedUser_ShouldDisplayAlert()
        {
            this.Login();
            Assert.That(this.AddToCart(), Is.EqualTo("Success"));
        }

        [Test]
        public void ShowCart_NotLoggedUser_ShouldRedirectToHome()
        {
            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Cart");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("storeContainer"))
                );

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/"));
        }

        [Test]
        public void ShowCart_LoggedUserEmptyCart_ShouldRedirectToHome()
        {
            this.Login();

            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Cart");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("storeContainer"))
                );

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/"));
        }

        [Test]
        public void ShowCart_LoggedUserNotEmptyCart_ShouldShowCart()
        {
            this.Login();
            this.AddToCart();

            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Cart");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("cartContainer"))
                );

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/Account/Cart"));
        }
        [TearDown]
        public void Close()
        {
            _driver.Close();
        }

        private string AddToCart()
        {
            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Products/2");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("clickedProductContainer"))
                );

            _driver.FindElement(By.ClassName("productAdd")).Click();

            var alertMessage = _driver.SwitchTo().Alert().Text;
            _driver.SwitchTo().Alert().Accept();

            return alertMessage;
        }
        private bool Login()
        {
            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Login");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("loginContainer"))
                );

            _driver.FindElement(By.Id("emailLogin")).SendKeys(test_email_correct);
            _driver.FindElement(By.Id("passwordLogin")).SendKeys(test_password_correct);
            _driver.FindElement(By.Id("submitLogin")).Click();

            return true;
        }
    }
}
