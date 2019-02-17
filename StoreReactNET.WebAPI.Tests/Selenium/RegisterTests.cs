using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace StoreReactNET.WebAPI.Tests.Selenium
{
    [TestFixture]
    public class RegisterTests
    {
        private IWebDriver _driver;
        

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver("./");
        }

        [Test]
        public void Register_CorrectCredentials_ShouldRedirectToHome()
        {
            var correctEmail = Faker.InternetFaker.Email();
            var correctPassword = "Test123!";

            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Register");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("registerContainer"))
                );

            _driver.FindElement(By.Id("emailRegister")).SendKeys(correctEmail);
            _driver.FindElement(By.Id("passwordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("rePasswordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("submitRegister")).Click();

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/"));
        }

        [Test]
        public void Register_WrongEmailFormat_ShouldShowError()
        {
            var wrongEmail = "test";

            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Register");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("registerContainer"))
                );
            _driver.FindElement(By.Id("emailRegister")).SendKeys(wrongEmail);
            _driver.FindElement(By.Id("submitRegister")).Click();


            var displayedError = _driver.FindElements(By.ClassName("error"))
                                  .Contains(_driver.FindElement(By.Id("emailRegister")));

            Assert.IsTrue(displayedError);
        }

        [Test]
        public void Register_PasswordTooShort_ShouldShowError()
        {
            var correctEmail = Faker.InternetFaker.Email();
            var correctPassword = "test";
            var wrongRePassword = "test";


            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Register");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("registerContainer"))
                );

            _driver.FindElement(By.Id("emailRegister")).SendKeys(correctEmail);
            _driver.FindElement(By.Id("passwordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("rePasswordRegister")).SendKeys(wrongRePassword);
            _driver.FindElement(By.Id("submitRegister")).Click();

            var displayedError = _driver.FindElements(By.ClassName("error"))
                .Contains(_driver.FindElement(By.Id("passwordRegister")));

            Assert.IsTrue(displayedError);
        }
        [Test]
        public void Register_NotSamePasswords_ShouldShowError()
        {
            var correctEmail = Faker.InternetFaker.Email();
            var correctPassword = "Test123!";
            var wrongRePassword = "Test123!!";


            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Register");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("registerContainer"))
                );

            _driver.FindElement(By.Id("emailRegister")).SendKeys(correctEmail);
            _driver.FindElement(By.Id("passwordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("rePasswordRegister")).SendKeys(wrongRePassword);
            _driver.FindElement(By.Id("submitRegister")).Click();

            var displayedError = _driver.FindElements(By.ClassName("error"))
                                    .Contains(_driver.FindElement(By.Id("rePasswordRegister")));

            Assert.IsTrue(displayedError);

        }

        [Test]
        public void Register_UserAlreadyExists_ShouldDisplayError()
        {
            var existingEmail = "test@test";
            var correctPassword = "Test123!";

            _driver.Navigate().GoToUrl("http://storereactnet.azurewebsites.net/Account/Register");

            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(
                    SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("registerContainer"))
                );

            _driver.FindElement(By.Id("emailRegister")).SendKeys(existingEmail);
            _driver.FindElement(By.Id("passwordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("rePasswordRegister")).SendKeys(correctPassword);
            _driver.FindElement(By.Id("submitRegister")).Click();

            Assert.That(_driver.Url, Is.EqualTo("http://storereactnet.azurewebsites.net/Account/Register/?failCode=0"));

        }
        [TearDown]
        public void Close()
        {
            _driver.Close();
        }

    }
}
