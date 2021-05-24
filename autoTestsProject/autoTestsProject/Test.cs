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

namespace autoTestsProject
{
    [TestFixture()]
    public class Test
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

        public void Wait(By param)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            wait.Until(d => driver.FindElements(param).Count > 0);
        }

        [Test]
        [TestCase("kate")]
        public void goodLoginUser(string name)
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "kate", "10039396" }, new string[] { "danilyuk", "kostya2478_KEY" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);

                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.Id("mat-input-0")).Count > 0);
                //Thread.Sleep(4000);
                Wait(By.Id("mat-input-0"));
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]);

                var data = GetDataFromExcel();

                driver.FindElement(By.Id("mat-input-1")).Click();
                {
                    var element = driver.FindElement(By.CssSelector(".loginbtn > .mat-focus-indicator"));
                    Actions builder = new Actions(driver);
                    builder.MoveToElement(element).Perform();
                }
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]);
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]")).Count > 0);
                //Thread.Sleep(5000);
                var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
                Assert.True(elements.Count > 0);
                Wait(By.XPath("//mat-icon[contains(.,\'more_vert\')]"));
                //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(d => driver.FindElements(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Count > 0);
                //Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        private IDictionary<string, string> GetDataFromExcel()
        {
            string filename = @"/Users/katekuzmich/Documents/4course/autotestsProject/autoTestsProject/data.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var values = new Dictionary<string, string>();

            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                for (int i = 1; i <= worksheet.Dimension.Rows; i++)
                {
                    values.Add(worksheet.Cells[$"A{i}"].Text, worksheet.Cells[$"B{i}"].Text);
                }
            }

            return values;
        }

        [Test]
        public void errorLoginUser()
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("u");
            driver.FindElement(By.Id("mat-input-1")).Click();
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo("u"));
            }
            var elements = driver.FindElements(By.CssSelector(".ng-tns-c87-0 > .mat-form-field-infix > .mat-warn .mat-input-element, .mat-form-field-invalid .mat-input-element "));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.Id("mat-input-1")).SendKeys("u");
            driver.FindElement(By.CssSelector(".col-second")).Click();
            var elems = driver.FindElements(By.CssSelector(".ng-tns-c87-1 > .mat-form-field-infix > .mat-warn .mat-input-element, .mat-form-field-invalid .mat-input-element "));
            Assert.True(elems.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodRegisterUser()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestStudentUserKata", "User123", "User123", "Kata", "Kata", "Kata", "test" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
                driver.SwitchTo().Frame(0);
                Wait(By.Id("mat-input-0"));
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
                Wait(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]"));
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]")).Click();
                Wait(By.CssSelector(".mat-select-placeholder"));
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
        public void addNewStudentInDB()
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

        
        

        
        //[Test]
        //[TestCase("Test", "С одним вариантом", " С несколькими вариантами ", "Холодник", "Бананы", "Дыня")]
        //public void goodEditQuestionWithOneAnswer(string textQuestion, string oldTypeQuestion, string newTypeQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        //{
        //    driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
        //    driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
        //    driver.FindElement(By.Id("mat-input-0")).Click();
        //    driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
        //    driver.FindElement(By.Id("mat-input-1")).Click();
        //    driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
        //    driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
        //    Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
        //    driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
        //    Thread.Sleep(5000);
        //    driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]"))?.Click();
        //    Wait(By.XPath("//a[contains(.,\'Тестирование знаний\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
        //    driver.SwitchTo().Frame(0);
        //    Wait(By.CssSelector(".mat-row"));
        //    var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
        //    var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
        //    Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
        //    driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
        //    Wait(By.CssSelector(".mat-row"));
        //    var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();           
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div/span[contains(.,\'{oldTypeQuestion}\')]"));
        //    //driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
        //    Wait(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]")).Text, Is.EqualTo(oldTypeQuestion));
        //    driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
        //    Wait(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]"));
        //    driver.FindElement(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]")).Click();
        //    driver.FindElement(By.Id("mat-input-3")).Click();
        //    driver.FindElement(By.Id("mat-input-3")).Clear();
        //    driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
        //    driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
        //    driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
        //    driver.FindElement(By.Id("mat-input-4")).Click();
        //    driver.FindElement(By.Id("mat-input-4")).SendKeys(secondAnswer);
        //    driver.FindElement(By.Id("mat-input-5")).Click();
        //    driver.FindElement(By.Id("mat-input-5")).SendKeys(thirdAnswer);
        //    var label = driver
        //       .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{firstAnswer}\']"))
        //       .FindElement(By.XPath(".."))
        //       .FindElement(By.XPath(".."));
        //    var div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
        //    IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
        //    executor1.ExecuteScript("arguments[0].click()", div);
        //    label = driver
        //       .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{secondAnswer}\']"))
        //       .FindElement(By.XPath(".."))
        //       .FindElement(By.XPath(".."));
        //    div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
        //    executor1 = (IJavaScriptExecutor)driver;
        //    executor1.ExecuteScript("arguments[0].click()", div);

        //    Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
        //    driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();
        //    questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    Wait(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]")).Text, Is.EqualTo(newTypeQuestion));
        //    var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
        //    Assert.True(message.Count == 0);
        //    driver.SwitchTo().DefaultContent();
        //    driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
        //    driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
        //    driver.Close();
        //}

        //[Test]
        //[TestCase("Test", " С несколькими вариантами ", " Ввод с клавиатуры ")]
        //public void goodEditQuestionWithMoreAnswer(string textQuestion, string oldTypeQuestion, string newTypeQuestion)
        //{
        //    driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
        //    driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
        //    driver.FindElement(By.Id("mat-input-0")).Click();
        //    driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
        //    driver.FindElement(By.Id("mat-input-1")).Click();
        //    driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
        //    driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
        //    Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
        //    driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
        //    Thread.Sleep(5000);
        //    driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]"))?.Click();
        //    Wait(By.XPath("//a[contains(.,\'Тестирование знаний\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
        //    driver.SwitchTo().Frame(0);
        //    Wait(By.CssSelector(".mat-row"));
        //    var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
        //    var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
        //    Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
        //    driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
        //    Wait(By.CssSelector(".mat-row"));
        //    var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
        //    Wait(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]")).Text, Is.EqualTo(oldTypeQuestion));
        //    Wait(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]"));
        //    driver.FindElement(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]")).Click();
        //    Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
        //    driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();
        //    questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    Wait(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]")).Text, Is.EqualTo(newTypeQuestion));
        //    var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
        //    Assert.True(message.Count == 0);
        //    driver.SwitchTo().DefaultContent();
        //    driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
        //    driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
        //    driver.Close();
        //}

        //[Test]
        //[TestCase("Test", " Ввод с клавиатуры ", " Последовательность элементов ")]
        //public void goodEditQuestionWithEnterAnswer(string textQuestion, string oldTypeQuestion, string newTypeQuestion)
        //{
        //    driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
        //    driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
        //    driver.FindElement(By.Id("mat-input-0")).Click();
        //    driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
        //    driver.FindElement(By.Id("mat-input-1")).Click();
        //    driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
        //    driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
        //    Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
        //    driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
        //    Thread.Sleep(5000);
        //    driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]"))?.Click();
        //    Wait(By.XPath("//a[contains(.,\'Тестирование знаний\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
        //    driver.SwitchTo().Frame(0);
        //    Wait(By.CssSelector(".mat-row"));
        //    var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
        //    var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
        //    Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
        //    driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
        //    Wait(By.CssSelector(".mat-row"));
        //    var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
        //    Wait(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]")).Text, Is.EqualTo(oldTypeQuestion));
        //    Wait(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]"));
        //    driver.FindElement(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]")).Click();
        //    Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
        //    driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();
        //    questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    Wait(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]")).Text, Is.EqualTo(newTypeQuestion));
        //    var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
        //    Assert.True(message.Count == 0);
        //    driver.SwitchTo().DefaultContent();
        //    driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
        //    driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
        //    driver.Close();
        //}

        //[Test]
        //[TestCase("Test", " Последовательность элементов ", " С одним вариантом ")]
        //public void goodEditQuestionWithOrderedAnswer(string textQuestion, string oldTypeQuestion, string newTypeQuestion)
        //{
        //    driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
        //    driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
        //    driver.FindElement(By.Id("mat-input-0")).Click();
        //    driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
        //    driver.FindElement(By.Id("mat-input-1")).Click();
        //    driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
        //    driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
        //    Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
        //    driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
        //    Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
        //    Thread.Sleep(5000);
        //    driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]"))?.Click();
        //    Wait(By.XPath("//a[contains(.,\'Тестирование знаний\')]"));
        //    driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
        //    driver.SwitchTo().Frame(0);
        //    Wait(By.CssSelector(".mat-row"));
        //    var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
        //    var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
        //    Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
        //    driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
        //    Wait(By.CssSelector(".mat-row"));
        //    var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
        //    Wait(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//span[contains(.,\'{oldTypeQuestion}\')]")).Text, Is.EqualTo(oldTypeQuestion));
        //    Wait(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]"));
        //    driver.FindElement(By.XPath($"//span[contains(.,\'{newTypeQuestion}\')]")).Click();
        //    Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
        //    driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();
        //    questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
        //    question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
        //    idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
        //    Thread.Sleep(4000);
        //    Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
        //    driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
        //    Wait(By.Id("mat-input-0"));
        //    Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
        //    Wait(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]"));
        //    Assert.That(driver.FindElement(By.XPath($"//mat-select/span[contains(.,\'{newTypeQuestion}\')]")).Text, Is.EqualTo(newTypeQuestion));
        //    var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
        //    Assert.True(message.Count == 0);
        //    driver.SwitchTo().DefaultContent();
        //    driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
        //    driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
        //    driver.Close();
        //}


        

        [Test]
        public void closeAccessStudentInSistem()
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

        
        
        

        [Test]
        public void goodAddLabworkStudent()
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
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            //driver.FindElement(By.XPath("//button/span/span[contains(.,\'Защита работ \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(.,\'Test lab work\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();
            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys("/Users/katekuzmich/Desktop/test.docx");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodEditLabworkStudent()
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
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Edit work");
            driver.FindElement(By.XPath("//span[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodDeleteLabworkStudent()
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
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Удалить лабораторную работу']")).Click();
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();
            Thread.Sleep(4000);
            var els = driver.FindElements(By.XPath($"//td[contains(.,\' Edit work\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        //подумать на счет количества вопросов
        [Test]
        public void goodDoingTestStudent()
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

        // tyt
        [Test]
        public void goodDownloadFileStudent()
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
            driver.FindElement(By.XPath("//a[contains(.,\'Файлы\')]")).Click();
            driver.SwitchTo().Frame(0);

            string path = @"files";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Thread.Sleep(3000);

            var downloadLink = driver
                .FindElement(By.XPath("//a[contains(text(),\'test.docx\')]"))
                .GetAttribute("href");

            WebClient webClient = new WebClient();

            string name = driver.FindElement(By.XPath("//a[contains(text(),\'test.docx\')]")).GetAttribute("ng-reflect-message");

            webClient.DownloadFile(downloadLink, path + "/" + name);

            string curFile = path + "/" + name;
            Assert.True(File.Exists(curFile));

            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorAddLabworkStudentWithoutFile()
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
        public void errorAddLabworkWithoutNamelabwork()
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
        public void errorEditLabworkStudentWithoutFile()
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
        
        [Test]
        public void errorDoingTestStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Test")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            Thread.Sleep(1000);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(elements.Count > 0);
            Thread.Sleep(2000);
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «Test» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorDownloadFileStudent()
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
            driver.FindElement(By.XPath("//a[contains(.,\'Файлы\')]")).Click();
            driver.SwitchTo().Frame(0);

            string path = @"files";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Thread.Sleep(3000);

            var downloadLink = driver
                .FindElement(By.XPath("//a[contains(text(),\'test.docx\')]"))
                .GetAttribute("href");

            var badLink = "/badlink" + downloadLink;

            WebClient webClient = new WebClient();

            string name = driver.FindElement(By.XPath("//a[contains(text(),\'test.docx\')]")).GetAttribute("ng-reflect-message");

            try
            {
                webClient.DownloadFile(badLink, path + "/" + name);
            }
            catch(WebException e)
            {
                Console.WriteLine(e);
            }
            

            string curFile = path + "/" + name;
            Assert.True(!File.Exists(curFile));

            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
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
            Wait(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            var changePasswordForm = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\' Сменить пароль \')]"));
            Assert.True(changePasswordForm.Count > 0);
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys("new123N");
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys("new123N");
            driver.FindElement(By.XPath("//button[contains(.,\'Сменить\')]")).Click();
            Wait(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            var elements = driver.FindElements(By.XPath("//mat-dialog-container[contains(.,\'Пароль успешно изменен.\')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
            driver.FindElement(By.Id("mat-dialog-1")).Click();
        }

       

        string DuplicateWord(string word, int countOfRepeats)
        {
            var result = new StringBuilder();

            for (int i = 0; i < countOfRepeats; i++)
            {
                result.Append(word);
            }
            return result.ToString();
        }

        

    }
}
