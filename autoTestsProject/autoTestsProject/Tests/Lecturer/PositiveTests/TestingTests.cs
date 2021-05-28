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
        [TestCase(" Тест для контроля знаний ", "Новый тест 1", "Тесты для контроля знаний", 1)]
        [TestCase(" Тест для самоконтроля ", "Новый тест 2", "Тесты для самоконтроля", 2)]
        [TestCase(" Предтест для обучения в ЭУМК ", "Новый тест 3", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase(" Тест для обучения в ЭУМК ", "Новый тест 4", "Тесты для обучения в ЭУМК", 4)]
        [TestCase(" Тест для обучения с искусственной нейронной сетью ", "Новый тест 5", "Тесты для обучения с искусстве", 5)]
        public void GoodAddNewTest(string testType, string testName, string titleTest, int numberDiv)
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
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]")).Click();
            driver.Wait(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            var elements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase(" Тест для контроля знаний ", "SuperTestForCancelTest", "Тесты для контроля знаний", 1)]
        [TestCase(" Тест для самоконтроля ", "SuperTest2ForCancelTest", "Тесты для самоконтроля", 2)]
        [TestCase(" Предтест для обучения в ЭУМК ", "SuperTest3ForCancelTest", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase(" Тест для обучения в ЭУМК ", "SuperTest4ForCancelTest", "Тесты для обучения в ЭУМК", 4)]
        [TestCase(" Тест для обучения с искусственной нейронной сетью ", "SuperTest5ForCancelTest", "Тесты для обучения с искусстве", 5)]
        public void GoodCancelAddNewTest(string testType, string testName, string titleTest, int numberDiv)
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
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]"));
            Assert.True(elements.Count == 0);
            elements = driver.FindElements(By.XPath($"//div[{numberDiv}]/app-main-table-tests[@ng-reflect-title=\'{titleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("Тесты для контроля знаний", 1, " Тест для самоконтроля ", "Новый тест 1", "Измененный новый тест")]
        [TestCase("Тесты для самоконтроля", 2, " Предтест для обучения в ЭУМК ", "Новый тест 2", "Измененый новый тест немного длинее")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, " Тест для обучения в ЭУМК ", "Новый тест 3", "Еще один новый тест")]
        [TestCase("Тесты для обучения в ЭУМК", 4, " Тест для обучения с искусственной нейронной сетью ", "Новый тест 4", "Четвертый новый тест")]
        [TestCase("Тесты для обучения с искусстве", 5, " Тест для контроля знаний ", "Новый тест 5", "И последний новый тест")]
        public void GoodEditTestName(string titleTest, int numberDiv, string delete, string oldTestName, string newTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

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

        [Test]
        [TestCase("Тесты для контроля знаний", 1, "Измененный новый тест", "NewSuperTestForCancelTest")]
        [TestCase("Тесты для самоконтроля", 2, "Измененый новый тест немного длинее", "NewSuperTest2ForCancelTest")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, "Еще один новый тест", "NewSuperTest3ForCancelTest")]
        [TestCase("Тесты для обучения в ЭУМК", 4, "Четвертый новый тест", "NewSuperTest4ForCancelTest")]
        [TestCase("Тесты для обучения с искусстве", 5, "И последний новый тест", "NewSuperTest5ForCancelTest")]
        public void GoodCancelEditTestName(string titleTest, int numberDiv, string oldTestName, string newTestName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

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


        [Test]
        [TestCase("Тесты для контроля знаний", 1, " Тест для самоконтроля ", "Измененный новый тест", "Тесты для самоконтроля", 2)]
        [TestCase("Тесты для самоконтроля", 2, " Предтест для обучения в ЭУМК ", "Измененый новый тест немного длинее", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase("Предтесты для обучения в ЭУМК", 3, " Тест для обучения в ЭУМК ", "Еще один новый тест", "Тесты для обучения в ЭУМК", 4)]
        [TestCase("Тесты для обучения в ЭУМК", 4, " Тест для обучения с искусственной нейронной сетью ", "етвертый новый тест", "Тесты для обучения с искусстве", 5)]
        [TestCase("Тесты для обучения с искусстве", 5, " Тест для контроля знаний ", "И последний новый тест", "Тесты для контроля знаний", 1)]
        public void GoodEditTestType(string oldTestType, int oldNumberDiv, string newTestType, string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

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

        [Test]
        [TestCase("Тесты для контроля знаний", 1, " Тест для самоконтроля ", "Измененный новый тест", "Тесты для самоконтроля", 2)]
        [TestCase("Тесты для самоконтроля", 2, " Предтест для обучения в ЭУМК ", "Измененый новый тест немного длинее", "Предтесты для обучения в ЭУМК", 3)]
        [TestCase("Предтесты для обучения в ЭУМК", 3, " Тест для обучения в ЭУМК ", "Еще один новый тест", "Тесты для обучения в ЭУМК", 4)]
        [TestCase("Тесты для обучения в ЭУМК", 4, " Тест для обучения с искусственной нейронной сетью ", "етвертый новый тест", "Тесты для обучения с искусстве", 5)]
        [TestCase("Тесты для обучения с искусстве", 5, " Тест для контроля знаний ", "И последний новый тест", "Тесты для контроля знаний", 1)]
        public void GoodCancelEditTestType(string oldTestType, int oldNumberDiv, string newTestType, string testName, string newTitleTest, int NewNumberDiv)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

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
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]"));
            Assert.True(message.Count == 0);
            var elements = driver.FindElements(By.XPath($"//div[{NewNumberDiv}]/app-main-table-tests[@ng-reflect-title=\'{newTitleTest}\']/div[2]/mat-table/mat-row/mat-cell[contains(.,\'{testName}\')]"));
            Assert.True(elements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("Тесты для контроля знаний", 1, "lock ")]
        [TestCase("Тесты для самоконтроля", 2, "lock_open ")]
        [TestCase("Предтесты для обучения в ЭУМК", 3, "lock ")]
        [TestCase("Тесты для обучения в ЭУМК", 4, "lock_open ")]
        [TestCase("Тесты для обучения с искусстве", 5, "lock ")]
        public void DefaultAccessForStudentForDifferentTestType(string testType, int numberBlockWithTests, string valueAccess)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row"));
            var allTestsCount = driver.FindElements(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row")).Count();
            var specialAccessTestsCount = driver.FindElements(By.XPath($"//div[{numberBlockWithTests}]/app-main-table-tests[@ng-reflect-title=\'{testType}\']/div[2]/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'{valueAccess}\')]")).Count();

            Assert.True(allTestsCount == specialAccessTestsCount);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        // проверка на наличие кнопки "Перейти к вопросам"
        [Test]
        [TestCase("Teste")]
        [TestCase("TestTest")]
        public void AccessDoingTest(string nameTest)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(nameTest));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            if (questionsForFind.Count == 0)
            {
                driver.FindElement(By.ClassName("question-page__backlink-first")).Click();
                var btnDoingTest = driver.FindElements(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                Assert.True(btnDoingTest.Count == 0);
            }
            else
            {
                driver.FindElement(By.XPath("//div[contains(.,'< К тестам')]")).Click();
                driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                var btnDoingTest = driver.FindElements(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
                Assert.True(btnDoingTest.Count > 0);
            }

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        public void DoingTest()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать тест\']")).Click();
            Thread.Sleep(2000);
            var inputQuestion = driver.FindElement(By.Id("mat-input-2"));
            var value = inputQuestion.GetAttribute("ng-reflect-value");
            driver.FindElement(By.XPath("//button[contains(.,\'Закрыть\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            for (int i = 1; i <= int.Parse(value); i++)
            {
                Thread.Sleep(3000);
                var questionAnswersBlockContainer = driver.FindElement(By.ClassName("question-answers-block-container"));

                var questionType = DefineQuestionType(questionAnswersBlockContainer);

                switch (questionType)
                {
                    case QuestionTypeEnum.Input:
                        driver.FindElement(By.ClassName("mat-input-element")).SendKeys("test");
                        break;
                    case QuestionTypeEnum.DropList:
                        break;
                    case QuestionTypeEnum.Checkbox:
                        driver.FindElement(By.ClassName("mat-checkbox")).Click();
                        break;
                    case QuestionTypeEnum.RadioButton:
                        driver.FindElement(By.ClassName("mat-radio-container")).Click();
                        break;
                }
                driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            }
            var str = "Тест на тему «TestTest» завершен";
            driver.Wait(By.XPath($"//app-test-result/div/div[contains(.,\'{str}\')]"));
            Assert.That(driver.FindElement(By.XPath($"//app-test-result/div/div[contains(.,\'{str}\')]")).Text, Is.EqualTo("Тест на тему «TestTest» завершен"));
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        public IDictionary<string, QuestionTypeEnum> QuestionTypes = new Dictionary<string, QuestionTypeEnum>()
        {
            { "mat-input-element", QuestionTypeEnum.Input },
            { "cdk-drop-list", QuestionTypeEnum.DropList },
            { "question-answers-block-container-checkboxes", QuestionTypeEnum.Checkbox },
            { "mat-radio-group", QuestionTypeEnum.RadioButton },
        };

        public QuestionTypeEnum DefineQuestionType(IWebElement element)
        {
            QuestionTypeEnum questionType = QuestionTypeEnum.NoType;

            foreach (var questionTypePair in QuestionTypes)
            {
                var elements = element.FindElements(By.ClassName(questionTypePair.Key));

                if (elements.Count != 0)
                {
                    questionType = questionTypePair.Value;
                }
            }

            return questionType;
        }

        [Test]
        [TestCase("NewTestTest", " Тестовая ", "Cat")]
        public void GoodAddAccessToStudentInTest(string testName, string groupName, string userName)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
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

        [Test]
        [TestCase("NewTestTest", "Тестовая")]
        public void GoodAddAccessToStudentsInTest(string testName, string groupName)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.Wait(By.ClassName("students-table-buttons-item-add")); //tyt
            driver.FindElement(By.ClassName("students-table-buttons-item-add")).Click();
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            driver.Wait(By.CssSelector(".mat-row"));
            var countUsersForFind = div.FindElements(By.CssSelector(".mat-row")).Count;
            driver.Wait(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock_open \')]"));
            var countAccessBtn = driver.FindElements(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock_open \')]")).Count;
            Assert.True(countUsersForFind == countAccessBtn);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("NewTestTest", "Тестовая")]
        public void GoodCloseAccessToStudentsInTest(string testName, string groupName)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            driver.Wait(By.XPath("//mat-select[@id='mat-select-0']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.Wait(By.ClassName("students-table-buttons-item-resources")); //tyt
            driver.FindElement(By.ClassName("students-table-buttons-item-resources")).Click();
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            driver.Wait(By.CssSelector(".mat-row"));
            var countUsersForFind = div.FindElements(By.CssSelector(".mat-row")).Count;
            driver.Wait(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock \')]"));
            var countNotAccessBtn = driver.FindElements(By.XPath($"//div[@class=\'students-table-list\']/mat-table/mat-row/mat-cell/mat-icon[contains(.,\'lock \')]")).Count;
            Assert.True(countUsersForFind == countNotAccessBtn);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        // поиск студента в группе в окне Доступность теста
        [Test]
        [TestCase("NewTestTest", " Тестовая ", "Cat")]
        public void GoodSearchStudentInAccessForm(string testName, string groupName, string userName)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
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

        [Test]
        [TestCase("NewTestTest", " Тестовая ", "Cat")]
        public void CloseAccessStudentInTest(string testName, string groupName, string userName)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
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
    }
}
