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

namespace autoTestsProject.Tests.Lecturer.NegativeTests
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
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }


        [Test]
        [TestCase("Новый тест 1", " Тест для контроля знаний ")]
        [TestCase("Новый тест 2", " Тест для самоконтроля ")]
        [TestCase("Новый тест 3", " Предтест для обучения в ЭУМК ")]
        [TestCase("Новый тест 4", " Тест для обучения в ЭУМК ")]
        [TestCase("Новый тест 5", " Тест для обучения с искусственной нейронной сетью ")]
        public void ErrorAddNewTestWithIdenticalName(string testName, string testType)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(testName);
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).Clear();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("Test");
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).Clear();
            driver.FindElement(By.Id("mat-input-2")).SendKeys("6");
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).Clear();
            driver.FindElement(By.Id("mat-input-3")).SendKeys("12");
            driver.Wait(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{testType}\')]"));
            var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{testType}\')]"));
            var label = div.FindElement(By.XPath(".."));
            label.FindElement(By.ClassName("mat-radio-label-content")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();

            driver.Wait(By.XPath("//div[contains(.,\'Тест с таким названием уже существует в рамках данного предмета\')]"));
            var message = driver.FindElements(By.XPath("//div[contains(.,\'Тест с таким названием уже существует в рамках данного предмета\')]"));
            Assert.True(message.Count > 0);

            message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("")]
        public void ErrorAddNewTestWithAllEmptyData(string emptyTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys(emptyTestName);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();

            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);

            var fieldCountQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldCountQuestionValue);

            var fieldTimeForQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldTimeForQuestionValue);

            var elements = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите название теста \')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите количество вопросов \')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите время \')]"));
            Assert.True(elements.Count > 0);
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [TestCase("Плохое название теста плохое название теста плохое название теста плохое название теста плохое " +
            "название теста плохое название теста плохое название теста плохое название теста плохое название теста " +
            "плохое название теста плохое название теста плохое название теста", -10, -10)]
        public void ErrorAddNewTestWithAllBadData(string badTestName, int countQuestion, int badTime)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys(badTestName);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(countQuestion.ToString());
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(badTime.ToString());
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();

            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldNameTestValue);

            var fieldCountQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldCountQuestionValue);

            var fieldTimeForQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldTimeForQuestionValue);

            var elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Длина превышает 255 символов\')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Количество вопросов должно быть больше нуля\')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Время должно быть больше нуля\')]"));
            Assert.True(elements.Count > 0);
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [TestCase("Название правильное, но тест не пройдет")]
        public void ErrorAddNewTestWithoutTestType(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys(testName);

            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("false", fieldNameTestValue);

            driver.Wait(By.XPath("//button[contains(.,\' Сохранить \')]"));
            driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]")).Click();

            driver.Wait(By.XPath("//simple-snack-bar/span[contains(.,\'Выберите тип\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar/span[contains(.,\'Выберите тип\')]"));
            Assert.True(message.Count > 0);

            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [Test]
        [TestCase("NewTestTest")]
        public void ErrorDeleteTest(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName)); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить тест\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить тест\']")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Да\')]")).Click();
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить тест\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить тест\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        public void ErrorDoingTest()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            Thread.Sleep(1000);
            var errorMessage = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(errorMessage.Count > 0);
            Thread.Sleep(2000);
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «NewTestTest» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("")]
        public void ErrorEditTestWithEmptyData(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.XPath("//textarea[@placeholder=\'Название теста\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys(testName);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();

            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);
            var elements = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите количество вопросов \')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите время \')]"));
            Assert.True(elements.Count > 0);
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("Новый тест 1", " Тест для контроля знаний ")]
        [TestCase("Новый тест 2", " Тест для самоконтроля ")]
        [TestCase("Новый тест 3", " Предтест для обучения в ЭУМК ")]
        [TestCase("Новый тест 4", " Тест для обучения в ЭУМК ")]
        [TestCase("Новый тест 5", " Тест для обучения с искусственной нейронной сетью ")]
        public void ErrorEditNewTestWithIdenticalName(string testName, string testType)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(testName);
            driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();

            driver.Wait(By.XPath("//div[contains(.,\'Тест с таким названием уже существует в рамках данного предмета\')]"));
            var message = driver.FindElements(By.XPath("//div[contains(.,\'Тест с таким названием уже существует в рамках данного предмета\')]"));
            Assert.True(message.Count > 0);

            message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [TestCase("Плохое название теста плохое название теста плохое название теста плохое название теста плохое " +
            "название теста плохое название теста плохое название теста плохое название теста плохое название теста " +
            "плохое название теста плохое название теста плохое название теста", -10, -10)]
        public void ErrorEditNewTestWithAllBadData(string badTestName, int countQuestion, int badTime)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.XPath("//textarea[@placeholder=\'Название теста\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys(badTestName);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).SendKeys(countQuestion.ToString());
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).SendKeys(badTime.ToString());
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Описание теста\']")).Click();

            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldNameTestValue);

            var fieldCountQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldCountQuestionValue);

            var fieldTimeForQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", fieldTimeForQuestionValue);

            var elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Длина превышает 255 символов\')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Количество вопросов должно быть больше нуля\')]"));
            Assert.True(elements.Count > 0);
            elements = driver.FindElements(By.XPath("//mat-error[contains(.,\'Время должно быть больше нуля\')]"));
            Assert.True(elements.Count > 0);
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
