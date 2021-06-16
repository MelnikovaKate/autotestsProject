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
        [TestCase(" Тест для контроля знаний ", "В", "Тесты для контроля знаний", 1)]
        [TestCase(" Тест для самоконтроля ", "Очень-очень длинное предлинное название теста для тестирования длины " +
                  "названия теста очень-очень длинное предлинное название теста для тестирования длины названия теста " +
                  "очень-очень длинное предлинное название теста для тестирования длины названия тест_2021", "Тесты для самоконтроля", 2)]
        [TestCase(" Предтест для обучения в ЭУМК ", "Хорошее название для теста 2021, но немного длинное, чем обычно " +
                  "Хорошее назва-ние для теста 2021, но не-много длинное, чем обычно", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase(" Тест для обучения в ЭУМК ", "Новый тест 4", "Тесты для обучения в ЭУМК", 4)]
        [TestCase(" Тест для обучения с искусственной нейронной сетью ", "Еще одно простое название для теста 2021", "Тесты для обучения с искусстве", 5)]
        public void AddNewTest(string testType, string testName, string titleTest, int numberDiv)
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
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]")).Click();
            driver.Wait(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var elements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase(" Тест для контроля знаний ", "Д", "Тесты для контроля знаний", 1)]
        [TestCase(" Тест для самоконтроля ", "Очень-очень длинное предлинное название теста для тестирования длины " +
                  "названия теста очень-очень длинное предлинное название теста для тестирования длины названия теста " +
                  "очень-очень длинное предлинное название теста для тестирования длины названия тест_тест", "Тесты для самоконтроля", 2)]
        [TestCase(" Предтест для обучения в ЭУМК ", "Хорошее название для теста, но немного длинное, чем обычно " +
                  "хорошее назва-ние для теста, но не-много длинное, чем обычно", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase(" Тест для обучения в ЭУМК ", "Новый тест 444", "Тесты для обучения в ЭУМК", 4)]
        [TestCase(" Тест для обучения с искусственной нейронной сетью ", "Простое название для теста", "Тесты для обучения с искусстве", 5)]
        public void CancelAddNewTest(string testType, string testName, string titleTest, int numberDiv)
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
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(elements.Count == 0);
            elements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("Тесты для контроля знаний", 1, "В", "Ф")]
        [TestCase("Тесты для самоконтроля", 2, "Очень-очень длинное предлинное название теста для тестирования длины " +
                 "названия теста очень-очень длинное предлинное название теста для тестирования длины названия теста " +
                 "очень-очень длинное предлинное название теста для тестирования длины названия тест_2021", "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, "Хорошее название для теста 2021, но немного длинное, чем обычно " +
                 "Хорошее назва-ние для теста 2021, но не-много длинное, чем обычно", "И последнее хорошее название для теста 2021, но немного длинное, чем обычно " +
                 "хорошее назва-ние для теста 2021, но не-много длинное, чем обычно")]
        [TestCase("Тесты для обучения в ЭУМК", 4, "Новый тест 4", "Четвертый новый тест")]
        [TestCase("Тесты для обучения с искусстве", 5, "Еще одно простое название для теста 2021", "Простое название для теста 2021")]
        public void EditTestName(string titleTest, int numberDiv, string oldTestName, string newTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{oldTestName}\')]"));
            var oldElements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{oldTestName}\')]"));
            Assert.True(oldElements.Count > 0);
            var el = driver.FindElement(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTestName}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            el.Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-dialog-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(newTestName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{newTestName}\')]"));
            var newElements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{newTestName}\')]"));
            Assert.True(newElements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(4)]
        [TestCase("Тесты для контроля знаний", 1, "Ф", "Г")]
        [TestCase("Тесты для самоконтроля", 2, "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи", "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины наз2021")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, "И последнее хорошее название для теста 2021, но немного длинное, чем обычно " +
                 "хорошее назва-ние для теста 2021, но не-много длинное, чем обычно", "И последнее хорошее название для теста 2021, но немного длинное, чем обычно " +
                 "хорошее назва-ние для теста 2021, но не-много длинное, чем обычно для отмены")]
        [TestCase("Тесты для обучения в ЭУМК", 4, "Четвертый новый тест", "Четвертый новый тест для отмены")]
        [TestCase("Тесты для обучения с искусстве", 5, "Простое название для теста 2021", "Еще одно простое название для теста 2021 для отмены")]
        public void CancelEditTestName(string titleTest, int numberDiv, string oldTestName, string newTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{oldTestName}\')]"));
            var oldElements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{oldTestName}\')]"));
            Assert.True(oldElements.Count > 0);
            var el = driver.FindElement(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTestName}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            el.Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-dialog-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(newTestName);
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(message.Count == 0);
            var newElements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{newTestName}\')]"));
            Assert.True(newElements.Count == 0);
            oldElements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{oldTestName}\')]"));
            Assert.True(oldElements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [Test, Order(6)]
        [TestCase("Тесты для контроля знаний", 1, " Тест для самоконтроля ", "Ф", "Тесты для самоконтроля", 2)] // "Предтесты для обучения в ЭУМК", 3,
        [TestCase("Тесты для самоконтроля", 2, " Тест для обучения в ЭУМК ", "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи", "Тесты для обучения в ЭУМК", 4)]
        [TestCase("Тесты для обучения в ЭУМК", 4, " Тест для обучения с искусственной нейронной сетью ", "Четвертый новый тест", "Тесты для обучения с искусстве", 5)]
        [TestCase("Тесты для обучения с искусстве", 5, " Тест для контроля знаний ", "Простое название для теста 2021", "Тесты для контроля знаний", 1)]
        public void EditTestType(string oldTestType, int oldNumberDiv, string newTestType, string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{oldNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{oldTestType}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var oldElements = driver.FindElements(By.XPath($"//div[{oldNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{oldTestType}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(oldElements.Count > 0);
            var el = driver.FindElement(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            el.Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-dialog-0")).Click();
            driver.Wait(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{newTestType}\')]"));
            var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{newTestType}\')]"));
            var label = div.FindElement(By.XPath(".."));
            label.FindElement(By.ClassName("mat-radio-label-content")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var elements = driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(5)] 
        [TestCase("Тесты для контроля знаний", 1, " Тест для обучения с искусственной нейронной сетью ", "Ф", "Тесты для обучения с искусстве", 5)] 
        [TestCase("Тесты для самоконтроля", 2, " Предтест для обучения в ЭУМК ", "Измененное очень-очень " +
                 "длинное предлинное название теста для тестирования длины названия теста очень-очень длинное предлинное название теста для " +
                 "тестирования длины названия теста очень-очень длинное предлинное название теста для тестирования длины названи", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase("Тесты для обучения в ЭУМК", 4, " Тест для обучения с искусственной нейронной сетью ", "Четвертый новый тест", "Тесты для обучения с искусстве", 5)] 
        [TestCase("Тесты для обучения с искусстве", 5, " Тест для обучения в ЭУМК ", "Простое название для теста 2021", "Тесты для самоконтроля", 2)]
        public void CancelEditTestType(string oldTestType, int oldNumberDiv, string newTestType, string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//div[{oldNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{oldTestType}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var oldElements = driver.FindElements(By.XPath($"//div[{oldNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{oldTestType}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(oldElements.Count > 0);
            var el = driver.FindElement(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            el.Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-dialog-0")).Click();
            driver.Wait(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{newTestType}\')]"));
            var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{newTestType}\')]"));
            var label = div.FindElement(By.XPath(".."));
            label.FindElement(By.ClassName("mat-radio-label-content")).Click();
            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.ClickJS(By.XPath("//span[contains(.,\'Закрыть\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(message.Count == 0);
            var elements = driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(7)]
        [TestCase("Тесты для контроля знаний", 1, "lock ")]
        [TestCase("Тесты для самоконтроля", 2, "lock_open ")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, "lock ")]
        [TestCase("Тесты для обучения в ЭУМК", 4, "lock_open ")]
        [TestCase("Тесты для обучения с искусстве", 5, "lock ")]
        public void DefaultAccessForStudent(string testType, int numberBlockWithTests, string valueAccess)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row"));
            var allTestsCount = driver.FindElements(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row")).Count();
            var specialAccessTestsCount = driver.FindElements(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'{valueAccess}\')]")).Count();

            Assert.True(allTestsCount == specialAccessTestsCount);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        // проверка на наличие кнопки "Перейти к вопросам"
        [Test, Order(8)]
        [TestCase("Простое название для теста 2021")]
        public void AccessDoingTest(string nameTest)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{nameTest}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{nameTest}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            if (questionsForFind.Count == 0)
            {
                driver.FindElement(By.ClassName("question-page__backlink-first")).Click();
                var btnDoingTest = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{nameTest}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                Assert.True(btnDoingTest.Count == 0);
            }
            else
            {
                driver.FindElement(By.XPath("//div[contains(.,'< К тестам')]")).Click();
                driver.Wait(By.XPath($"//mat-cell[contains(.,\'{nameTest}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                var btnDoingTest = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{nameTest}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                Assert.True(btnDoingTest.Count > 0);
            }

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(9)]
        [TestCase("Простое название для теста 2021", "Тестовая")]
        public void AddAccessToStudentsInTest(string testName, string groupName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.ClickJS(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.Wait(By.ClassName("students-table-buttons-item-add")); 
            driver.ClickJS(By.ClassName("students-table-buttons-item-add"));
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            driver.Wait(By.CssSelector(".mat-row"));
            var countUsersForFind = div.FindElements(By.CssSelector(".mat-row")).Count;
            driver.Wait(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock_open \')]"));
            var countAccessBtn = driver.FindElements(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock_open \')]")).Count;
            Assert.True(countUsersForFind == countAccessBtn);
            driver.ClickJS(By.XPath("//mat-icon[contains(.,\'close\')]"));
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(10)]
        [TestCase("Простое название для теста 2021", "Тестовая")] // tyt
        public void CloseAccessToStudentsInTest(string testName, string groupName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.ClickJS(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.Wait(By.ClassName("students-table-buttons-item-resources"));
            driver.ClickJS(By.ClassName("students-table-buttons-item-resources"));
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            driver.Wait(By.CssSelector(".mat-row"));
            var countUsersForFind = div.FindElements(By.CssSelector(".mat-row")).Count;
            driver.Wait(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock \')]"));
            Thread.Sleep(1000);
            var countNotAccessBtn = driver.FindElements(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock \')]")).Count;
            Assert.True(countUsersForFind == countNotAccessBtn);
            driver.ClickJS(By.XPath("//mat-icon[contains(.,\'close\')]"));
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(11)]
        [TestCase("Простое название для теста 2021", " Тестовая ", "Cat")]
        public void AddAccessToStudentInTest(string testName, string groupName, string userName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.Wait(By.XPath("//div[@class=\'students-table-list\']"));
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            var userForFind = div.FindElements(By.CssSelector(".mat-row"));
            var user = userForFind.FirstOrDefault(x => x.Text.StartsWith(userName, StringComparison.Ordinal)); // имя user
            var idRowOfUser = div.FindElements(By.CssSelector(".mat-row")).IndexOf(user);
            driver.Wait(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock \')]"));
            driver.FindElement(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock \')]")).Click();
            driver.Wait(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell/mat-icon[contains(.,\'lock_open \')]"));
            var openLockBtn = driver.FindElements(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell/mat-icon[contains(.,\'lock_open \')]"));
            Assert.True(openLockBtn.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(12)]
        [TestCase("Простое название для теста 2021", " Тестовая ", "Cat")]
        public void CloseAccessStudentInTest(string testName, string groupName, string userName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.Wait(By.XPath("//div[@class=\'students-table-list\']"));
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            var userForFind = div.FindElements(By.CssSelector(".mat-row"));
            var user = userForFind.FirstOrDefault(x => x.Text.StartsWith(userName, StringComparison.Ordinal)); // имя user
            var idRowOfUser = div.FindElements(By.CssSelector(".mat-row")).IndexOf(user);
            driver.Wait(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock_open \')]"));
            driver.FindElement(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock_open \')]")).Click();
            driver.Wait(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell/mat-icon[contains(.,\'lock \')]"));
            var openLockBtn = driver.FindElements(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell/mat-icon[contains(.,\'lock \')]"));
            Assert.True(openLockBtn.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        

        

        // поиск студента в группе в окне Доступность теста
        [Test, Order(12)]
        [TestCase("Простое название для теста 2021", " Тестовая ", "Cat")]
        public void SearchStudentInAccessForm(string testName, string groupName, string userName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{testName}\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.Wait(By.XPath("//div[@class=\'students-table-list\']"));
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            var userForFind = div.FindElements(By.CssSelector(".mat-row"));
            driver.Wait(By.XPath("//input[@placeholder=\'Поиск\']"));
            driver.FindElement(By.XPath("//input[@placeholder=\'Поиск\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Поиск\']")).SendKeys("Cat");
            driver.Wait(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell[contains(.,\'{userName}\')]"));
            var searchingUser = driver.FindElements(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell[contains(.,\'{userName}\')]"));
            Assert.True(searchingUser.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(13)]
        [TestCase("Простое название для теста 2021", "Тесты для контроля знаний", 1)]
        public void CancelDeleteTest(string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'delete \')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'delete \')]"));

            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.ClickJS(By.XPath("//span[contains(.,\'Закрыть\')]"));

            driver.Wait(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var elements = driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(14)]
        [TestCase(" Тест для самоконтроля ", "И", "Тесты для самоконтроля", 2)]
        public void DeleteTest(string newTestType, string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'delete \')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[contains(.,\'delete \')]"));

            driver.Wait(By.XPath("//button[contains(.,\'Да\')]"));
            driver.ClickJS(By.XPath("//button[contains(.,\'Да\')]"));

            driver.Wait(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var elements = driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

    }
}
