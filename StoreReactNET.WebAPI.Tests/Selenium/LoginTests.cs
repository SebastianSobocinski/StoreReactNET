using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace StoreReactNET.WebAPI.Tests.Selenium
{
    [TestFixture]
    public class LoginTests
    {
        private IWebDriver _driver;

        private readonly string test_email_correct = "test@test";
        private readonly string test_password_correct = "test";
        private readonly string test_password_incorrect = "wrongPassword123";

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver("./");
        }

        [Test]
        public void Login_CorrectCredentials_ShouldLoginCorrectly()
        {
            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Login");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("loginContainer"))
                    );

            _driver.FindElement(By.Id("emailLogin")).SendKeys(test_email_correct);
            _driver.FindElement(By.Id("passwordLogin")).SendKeys(test_password_correct);
            _driver.FindElement(By.Id("submitLogin")).Click();

            //If user logged in correctly redirects to main page
            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/"));
        }

        [Test]
        public void Login_IncorrectCredentials_ShouldRedirectDisplayError()
        {
            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Login");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("loginContainer"))
                );

            _driver.FindElement(By.Id("emailLogin")).SendKeys(test_email_correct);
            _driver.FindElement(By.Id("passwordLogin")).SendKeys(test_password_incorrect);
            _driver.FindElement(By.Id("submitLogin")).Click();

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/Account/Login/?failed=true"));
        }
        [TearDown]
        public void Close()
        {
            _driver.Close();
        }
    }
}
