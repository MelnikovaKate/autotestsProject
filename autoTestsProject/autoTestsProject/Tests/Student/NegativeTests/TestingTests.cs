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

namespace autoTestsProject.Tests.Student.SNegativeTests
{
    [TestFixture(), Order(3)]
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
        [TestCase("И")]
        public void ErrorDoingTestStudent(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            driver.Wait(By.XPath("//button[contains(.,\'done_outline Ответить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
 
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(elements.Count > 0);

            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «{testName}» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


    }
}
