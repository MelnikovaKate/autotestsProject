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
    [TestFixture(), Order(2)]
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

        [Test, Order(1)]
        [TestCase("")]
        public void ErrorAddNewTestWithAllEmptyData(string emptyTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

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

        [Test, Order(2)]
        [TestCase("Простое название для теста 2021", "")]
        public void ErrorEditTestWithEmptyData(string oldTestName, string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{oldTestName}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{oldTestName}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]")).Click();
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

        [Test, Order(3)]
        [TestCase("Название правильное, но тест не пройдет")]
        public void ErrorAddNewTestWithoutTestType(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

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

        [Test, Order(4)]
        [TestCase("Плохое название теста плохое название теста плохое название теста плохое название теста плохое " +
            "название теста плохое название теста плохое название теста плохое название теста плохое название теста " +
            "плохое название теста плохое название теста плохое название теста", -10, -10)]
        public void ErrorAddNewTestWithAllBadData(string badTestName, int countQuestion, int badTime)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

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

        [Test, Order(5)]
        [TestCase("Простое название для теста 2021","Плохое название теста плохое название теста плохое название теста плохое название теста плохое " +
            "название теста плохое название теста плохое название теста плохое название теста плохое название теста " +
            "плохое название теста плохое название теста плохое название теста", -10, -10)]
        public void ErrorEditNewTestWithAllBadData(string oldNameTest,string badTestName, int countQuestion, int badTime)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{oldNameTest}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{oldNameTest}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]")).Click();
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


        [Test, Order(6)]
        [TestCase("Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи", " Тест для контроля знаний ")]
        [TestCase("Простое название для теста 2021", " Тест для самоконтроля ")]
        [TestCase("Четвертый новый тест", " Предтест для обучения в ЭУМК ")]
        [TestCase("И последнее хорошее название для теста 2021, но немного длинное, чем обычно " +
                 "хорошее назва-ние для теста 2021, но не-много длинное, чем обычно", " Тест для обучения в ЭУМК ")]
        public void ErrorAddNewTestWithIdenticalName(string testName, string testType)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

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

        [Test, Order(7)]
        [TestCase("Простое название для теста 2021", "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи")]
        [TestCase("Простое название для теста 2021", "Четвертый новый тест")]
        [TestCase("Простое название для теста 2021", "И последнее простое название для теста 2021, но немного длинное, чем обычно " +
                 "простое назва-ние для теста 2021, но не-много длинное, чем обычно")]
        public void ErrorEditNewTestWithIdenticalName(string oldTestName, string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{oldTestName}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{oldTestName}\')]/../mat-cell/mat-icon[contains(.,\'edit \')]")).Click();
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


        [Test, Order(8)]
        [TestCase("Четвертый новый тест")]
        public void ErrorDeleteTest(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            
            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Удалить тест\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Удалить тест\']")).Click();
            driver.ClickJS(By.XPath("//button[contains(.,\'Да\')]"));
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить тест\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить тест\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(9)]
        [TestCase("Простое название для теста 2021")]
        public void ErrorDoingTest(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
 
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            driver.Wait(By.XPath("//button[contains(.,\'done_outline Ответить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
        
            driver.Wait(By.XPath("//snack-bar-container[contains(.,\'Выберите вариант ответа\')]"));
            var errorMessage = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(errorMessage.Count > 0);
         
            driver.Wait(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «{testName}» завершен\')]"));
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «{testName}» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
