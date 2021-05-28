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
    [TestFixture()]
    public class LabWorksTests
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
        public void ErrorAddLabworkStudentWithoutFile()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(.,\'Test lab work\')]")).Click();
            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            {
                elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
                Assert.True(elements.Count == 0);
            }
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void ErrorAddLabworkWithoutNamelabwork()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();
            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys("/Users/katekuzmich/Desktop/test.docx");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
            var elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Произошла ошибка \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void ErrorEditLabworkStudentWithoutFile()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[@ng-reflect-message=\'Удалить файл\']")).Click();
            Thread.Sleep(3000);
            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            {
                elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
                Assert.True(elements.Count == 0);
            }
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

    }
}
