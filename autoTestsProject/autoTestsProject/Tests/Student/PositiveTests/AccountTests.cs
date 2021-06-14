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

namespace autoTestsProject.Tests.Student.PositiveTests
{
    [TestFixture(), Order(1)]
    public class AccountTests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        private IDictionary<string, string> Logins = new Dictionary<string, string>();

        public AccountTests()
        {
            Logins.Add(new KeyValuePair<string, string>("UserLoginShort", $"T_{DateTime.Now.Hour}"));
            Logins.Add(new KeyValuePair<string, string>("UserLoginNormal", $"TestUser_{DateTime.Now.DayOfWeek}"));
            Logins.Add(new KeyValuePair<string, string>("UserLoginLong", $"TestLoginForTestUserForTest_{DateTime.Now.Day}"));
            Logins.Add(new KeyValuePair<string, string>("UserSurnameShort", $"N{DateTime.Now.Hour}"));
            Logins.Add(new KeyValuePair<string, string>("UserSurnameNormal", $"Surname{DateTime.Now.DayOfWeek}"));
            Logins.Add(new KeyValuePair<string, string>("UserSurnameLong", $"TestSurnameForTest_{DateTime.Now.Day}"));
        }

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
        [TestCase("UserLoginShort", "Pass1_", "Pass1_", "UserSurnameShort", "Tes", "Tes", "test")]
        [TestCase("UserLoginNormal", "TestPassword_1234567", "TestPassword_1234567", "UserSurnameNormal", "Some_TestUser", "Some_TestUser", "test")]
        [TestCase("UserLoginLong", "PasswordForTesting12345678910_", "PasswordForTesting12345678910_", "UserSurnameLong", "TestLogin", "TestLogin", "test")]
        public void RegisterUser(string userLogin, string userPassword, string userConfirmPassword, string userSurname, string userFirstname, string userFathername, string userAnswer)
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(Logins[userLogin]); // login
            driver.FindElement(By.CssSelector(".ng-tns-c5-1 .mat-form-field-infix")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(userPassword); // password
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(userConfirmPassword);
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(Logins[userSurname]); // surname
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(userFirstname); // firstname
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(userFathername); // fathername
            driver.FindElement(By.CssSelector(".mat-select-value > .ng-tns-c6-7")).Click();
            driver.Wait(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]"));
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]")).Click();
            driver.Wait(By.CssSelector(".mat-select-placeholder"));
            driver.FindElement(By.CssSelector(".mat-select-placeholder")).Click();
            driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Кличка любимого животного? \')]")).Click();
            driver.FindElement(By.Id("mat-input-6")).Click();
            driver.FindElement(By.Id("mat-input-6")).SendKeys(userAnswer);
            driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            driver.Close();
        }

        [Test, Order(2)]
        [TestCase("UserSurnameShort")]
        [TestCase("UserSurnameNormal")]
        [TestCase("UserSurnameLong")]
        public void AddNewStudentInSystem(string surnameStudent)
        {
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.Wait(By.XPath("//mat-icon[contains(.,\' person_add_alt_1\')]"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\' person_add_alt_1\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(7000);
            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.ClickJS(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            Thread.Sleep(1000);
            driver.Wait(By.XPath("//span[contains(.,\' Тестовая \')]"));
            driver.FindElement(By.XPath("//span[contains(.,\' Тестовая \')]")).Click();
            Thread.Sleep(4000);
            js.ExecuteScript("window.scrollTo(0,26)");
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(Logins[surnameStudent]));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon"));
            driver.Wait(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
            var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("UserLoginShort", "Pass1_")]
        [TestCase("UserLoginNormal", "TestPassword_1234567")]
        [TestCase("UserLoginLong", "PasswordForTesting12345678910_")]
        public void LoginUser(string userLogin, string userPassword)
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(Logins[userLogin]);

            driver.FindElement(By.Id("mat-input-1")).Click();

            driver.FindElement(By.Id("mat-input-1")).SendKeys(userPassword);
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            driver.Wait(By.XPath("//a[contains(.,\'Предметы\')]"));

            var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
            Assert.True(elements.Count > 0);
        }

        [Test, Order(4)]
        [TestCase("TestStudentUser7", "test", "new123N", "new123N")]
        public void ForgetPassword(string userLogin, string userAnswer, string newPassword, string newConfirmPassword)
        {
            driver.Navigate().GoToUrl("https://educats.by/login");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 949);
            driver.FindElement(By.XPath("//a[contains(.,\'Забыли пароль?\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.FindElement(By.Id("mat-input-0")).SendKeys(userLogin);
            driver.FindElement(By.XPath("//span[contains(.,\'Секретный вопрос\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Кличка любимого животного?\')]")).Click();
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(userAnswer);
            driver.FindElement(By.XPath("//button[contains(.,\'Сбросить\')]")).Click();
            driver.Wait(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            var changePasswordForm = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            Assert.True(changePasswordForm.Count > 0);
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(newPassword);
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(newConfirmPassword);
            driver.FindElement(By.XPath("//button[contains(.,\'Сменить\')]")).Click();
            driver.Wait(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            var elements = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.Close();
        }
    }
}
