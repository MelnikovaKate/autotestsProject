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
    [TestFixture(), Order(14)]
    public class TestingTests
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
            driver.Login(Defaults.StudentLogin, Defaults.StudentPassword);
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void ErrorDoingTestStudent()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            //Thread.Sleep(3000);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Простое название для теста 2021")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            //Thread.Sleep(5000);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            //Thread.Sleep(3000);
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();
            //Thread.Sleep(2000);
            driver.Wait(By.XPath("//button[contains(.,\'done_outline Ответить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            //Thread.Sleep(1000);
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(elements.Count > 0);
            //Thread.Sleep(2000);
            driver.Wait(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «Test» завершен\')]"));
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «Test» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
