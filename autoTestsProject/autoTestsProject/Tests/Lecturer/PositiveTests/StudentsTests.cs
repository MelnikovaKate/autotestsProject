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

namespace autoTestsProject.Tests.Lecturer.PositiveTests
{
    [TestFixture(), Order(4)]
    public class StudentsTests
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
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test]
        //[TestCase("TestUser2000")]
        [TestCase("TestLogin")]
        [TestCase("Tes")]
        [TestCase("Some_TestUser")]
        public void AddNewStudentInSystem(string surnameStudent)
        {
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
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(surnameStudent));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon"));
            driver.Wait(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
            var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("TestLogin")]
        [TestCase("Tes")]
        [TestCase("Some_TestUser")]
        public void CloseAccessStudentInSystem(string surnameStudent)
        {
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
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(surnameStudent));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            if (driver.FindElements(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon[@ng-reflect-message=\'Закрыть доступ\']")).Count > 0)
            {
                driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon"));
                driver.Wait(By.XPath("//snack-bar-container[contains(.,\'Подтверждение отменено\')]"));
                var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Подтверждение отменено\')]"));
                Assert.True(message.Count > 0);
            }  
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
