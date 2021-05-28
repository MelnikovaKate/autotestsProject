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
    [TestFixture()]
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
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void AddNewStudentInDB()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "Cat", "Cat", "Cat" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\' person_add_alt_1\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//span[contains(.,\' Тестовая \')]")).Click();
                Thread.Sleep(4000);
                js.ExecuteScript("window.scrollTo(0,26)");
                Thread.Sleep(4000);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(item[0]));
                //var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(item[0], StringComparison.Ordinal)); //
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                if (driver.FindElements(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon[contains(.,\'clear\')]")).Count > 0)
                {
                    driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon")).Click();
                    Thread.Sleep(10000);
                    var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
                    Assert.True(message.Count > 0);
                }
                else
                {
                    driver.Close(); // TODO
                }
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

      [Test]
        public void CloseAccessStudentInSistem()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "Cat", "Cat", "Cat" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\' person_add_alt_1\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//span[contains(.,\' Тестовая \')]")).Click();
                Thread.Sleep(4000);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(item[0]));
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                if (driver.FindElements(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon[@ng-reflect-message=\'Закрыть доступ\']")).Count > 0)
                    driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon")).Click();
                Thread.Sleep(10000);
                var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Подтверждение отменено\')]"));
                Assert.True(message.Count > 0);
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }
    }
}
