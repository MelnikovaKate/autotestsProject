using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Net;
using Actions = OpenQA.Selenium.Interactions.Actions;
using OfficeOpenXml;
using autoTestsProject.Enums;

namespace autoTestsProject.Tests.Student.NegativeTests
{
    [TestFixture(), Order(13)]
    public class AccountTests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        [TestCase("", "", "", "", "", "", "")]
        public void ErrorRegisterUserWithoutData(string userLogin, string userPassword, string userConfirmPassword, string userSurname, string userFirstname, string userFathername, string userAnswer) // TODO
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(userLogin); // login
            driver.FindElement(By.CssSelector(".ng-tns-c5-1 .mat-form-field-infix")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(userPassword); // password
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(userConfirmPassword);
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(userSurname); // surname
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(userFirstname); // firstname
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(userFathername); // fathername
            driver.FindElement(By.CssSelector(".mat-select-value > .ng-tns-c6-7")).Click();
            driver.FindElement(By.CssSelector(".cdk-overlay-backdrop")).Click();

            driver.Wait(By.Id("mat-select-1"));
            driver.FindElement(By.Id("mat-select-1")).Click();
            driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Кличка любимого животного? \')]")).Click();
            driver.FindElement(By.Id("mat-input-6")).Click();
            driver.FindElement(By.Id("mat-input-6")).SendKeys(userAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();

            string invalidValue = driver.FindElement(By.Id("mat-input-0")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-1")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-3")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-4")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-select-0")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-6")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            driver.Close();
        }

        [Test, Order(2)]
        [TestCase("T1", "Pass1", "Passw", "Te", "Te", "Tes", "test")]
        [TestCase("T", "Test", "Tes", "T", "T", "T", "test")]
        [TestCase("S", "T", "Te", "S", "So", "Som", "test")]
        public void ErrorRegisterUserWithBadData(string userLogin, string userPassword, string userConfirmPassword, string userSurname, string userFirstname, string userFathername, string userAnswer) // TODO
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(userLogin); // login
            driver.FindElement(By.CssSelector(".ng-tns-c5-1 .mat-form-field-infix")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(userPassword); // password
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(userConfirmPassword);
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(userSurname); // surname
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(userFirstname); // firstname
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(userFathername); // fathername
            driver.FindElement(By.CssSelector(".mat-select-value > .ng-tns-c6-7")).Click();
            driver.FindElement(By.CssSelector(".cdk-overlay-backdrop")).Click();

            driver.Wait(By.Id("mat-select-1"));
            driver.FindElement(By.Id("mat-select-1")).Click();
            driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Кличка любимого животного? \')]")).Click();
            driver.FindElement(By.Id("mat-input-6")).Click();
            driver.FindElement(By.Id("mat-input-6")).SendKeys(userAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();

            string invalidValue = driver.FindElement(By.Id("mat-input-0")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-1")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-2")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-3")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-input-4")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            invalidValue = driver.FindElement(By.Id("mat-select-0")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValue);

            driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            driver.Close();
        }

        [Test, Order(3)]
        [TestCase("", "")]
        [TestCase("T", "Pas")]
        [TestCase("Te", "Pas1_")]
        public void ErrorLoginUser(string badUserLogin, string badUserPassword)
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(badUserLogin);
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(badUserPassword);
            driver.FindElement(By.Id("mat-input-0")).Click();

            string invalidValueLogin = driver.FindElement(By.Id("mat-input-0")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValueLogin);

            string invalidValuePassword = driver.FindElement(By.Id("mat-input-1")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", invalidValuePassword);

            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            driver.Close();
        }
    }
}
