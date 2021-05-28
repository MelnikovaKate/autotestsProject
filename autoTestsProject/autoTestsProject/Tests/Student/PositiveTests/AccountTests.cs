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
    [TestFixture()]
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

        [Test]
        [TestCase("kate")]
        public void GoodLoginUser(string name)
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "kate", "10039396" }, new string[] { "danilyuk", "kostya2478_KEY" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);

                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.Id("mat-input-0")).Count > 0);
                //Thread.Sleep(4000);
                driver.Wait(By.Id("mat-input-0"));
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]);

                //var data = GetDataFromExcel();

                driver.FindElement(By.Id("mat-input-1")).Click();
                {
                    var element = driver.FindElement(By.CssSelector(".loginbtn > .mat-focus-indicator"));
                    Actions builder = new Actions(driver);
                    builder.MoveToElement(element).Perform();
                }
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]);
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                driver.Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]")).Count > 0);
                //Thread.Sleep(5000);
                var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
                Assert.True(elements.Count > 0);
                driver.Wait(By.XPath("//mat-icon[contains(.,\'more_vert\')]"));
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Count > 0);
                //Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void GoodRegisterUser()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestStudentUserKata", "User123", "User123", "Kata", "Kata", "Kata", "test" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
                driver.SwitchTo().Frame(0);
                driver.Wait(By.Id("mat-input-0"));
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]); // login
                driver.FindElement(By.CssSelector(".ng-tns-c5-1 .mat-form-field-infix")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]); // password
                driver.FindElement(By.Id("mat-input-2")).Click();
                driver.FindElement(By.Id("mat-input-2")).SendKeys(item[2]);
                driver.FindElement(By.Id("mat-input-3")).Click();
                driver.FindElement(By.Id("mat-input-3")).SendKeys(item[3]); // last n
                driver.FindElement(By.Id("mat-input-4")).Click();
                driver.FindElement(By.Id("mat-input-4")).SendKeys(item[4]); // first n
                driver.FindElement(By.Id("mat-input-5")).Click();
                driver.FindElement(By.Id("mat-input-5")).SendKeys(item[5]); // father n
                driver.FindElement(By.CssSelector(".mat-select-value > .ng-tns-c6-7")).Click();
                driver.Wait(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]"));
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]")).Click();
                driver.Wait(By.CssSelector(".mat-select-placeholder"));
                driver.FindElement(By.CssSelector(".mat-select-placeholder")).Click();
                driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Кличка любимого животного? \')]")).Click();
                driver.FindElement(By.Id("mat-input-6")).Click();
                driver.FindElement(By.Id("mat-input-6")).SendKeys(item[6]);
                driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void ForgetPassword()
        {
            driver.Navigate().GoToUrl("https://educats.by/login");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 949);
            driver.FindElement(By.XPath("//a[contains(.,\'Забыли пароль?\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.FindElement(By.Id("mat-input-0")).SendKeys("TestStudentUser7");
            driver.FindElement(By.XPath("//span[contains(.,\'Секретный вопрос\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Кличка любимого животного?\')]")).Click();
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("test");
            driver.FindElement(By.XPath("//button[contains(.,\'Сбросить\')]")).Click();
            driver.Wait(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            var changePasswordForm = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            Assert.True(changePasswordForm.Count > 0);
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys("new123N");
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys("new123N");
            driver.FindElement(By.XPath("//button[contains(.,\'Сменить\')]")).Click();
            driver.Wait(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            var elements = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
        }
    }
}
