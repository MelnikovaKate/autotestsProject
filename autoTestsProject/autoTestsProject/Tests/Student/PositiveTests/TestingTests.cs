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
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        //подумать на счет количества вопросов
        [Test]
        public void DoingTestStudent()
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
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Test")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            Dictionary<string, string> questions = new Dictionary<string, string>
            {
                {"Сколько будет 3+2?", "5"},
                {"Спутник Земли?", "Луна"},
                {"На какой свет светофора необходимо переходить дорогу?", "Зеленый"},
            };

            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(3000);
                var divTextQuestion = driver.FindElement(By.ClassName("question-question-text"));
                var textQuestion = divTextQuestion.Text;
                var answer = questions[textQuestion];
                Thread.Sleep(2000);
                var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{answer}\')]"));
                var label = div.FindElement(By.XPath(".."));
                label.FindElement(By.ClassName("mat-radio-label-content")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            }
            Thread.Sleep(2000);
            Assert.That(driver.FindElement(By.XPath("//app-test-result/div/div[contains(.,\' Тест на тему «Test» завершен \')]")).Text, Is.EqualTo("Тест на тему «Test» завершен"));
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }
    }
}
