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
    [TestFixture(), Order(7)]
    public class QuestionTests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;
        public static string CommonEmptyError = "Вопрос содержит пустые варианты ответа";
        public IDictionary<QuestionTypeEnum, string> QuestionTypeErrors = new Dictionary<QuestionTypeEnum, string>()
        {
            { QuestionTypeEnum.Input, "Вопрос содержит пустые варианты ответа" },
            { QuestionTypeEnum.DropList, "Последовательность должна состоять хотя бы из 2 элементов" },
            { QuestionTypeEnum.Checkbox, "Вопрос должен иметь хотя бы 3 варианта" },
            { QuestionTypeEnum.RadioButton, "Вопрос должен иметь хотя бы 2 варианта" },
        };

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
        [TestCase("Простое название для теста 2021", "")]
        public void ErrorAddQuestionWithoutAllData(string testName, string textQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
                                               
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//span[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.XPath("//textarea[@placeholder=\'Текст вопроса\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys(textQuestion);
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите название вопроса \')]"));
            Assert.True(errorMessage.Count > 0);
            errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите сложность \')]"));
            Assert.True(errorMessage.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("Простое название для теста 2021", "Новый вопрос")]
        public void ErrorAddQuestionWithoutAnswers(string testName, string textQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//span[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.XPath("//textarea[@placeholder=\'Текст вопроса\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys(textQuestion);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.Wait(By.XPath("//span[contains(.,\'Проверьте варианты ответов. Они не должны быть пустыми\')]"));
            var errorMessage = driver.FindElements(By.XPath("//span[contains(.,\'Проверьте варианты ответов. Они не должны быть пустыми\')]"));
            Assert.True(errorMessage.Count > 0);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(elements.Count == 0);
           
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        // ErrorAddQuestionWithOneAnswer
        [TestCase("С одним вариантом", "Вопрос должен иметь хотя бы 2 варианта", "")]
        // ErrorAddQuestionWithMoreAnswer
        [TestCase("С несколькими вариантами", "Вопрос должен иметь хотя бы 3 варианта", "fghjk")]
        // ErrorAddQuestionWithOrderedAnswers
        [TestCase("Последовательность элементов", "Последовательность должна состоять хотя бы из 2 элементов", "")]
        // ErrorAddQuestionWithEnterAnswer
        [TestCase("Ввод с клавиатуры", "Вопрос содержит пустые варианты ответа", "")]
        public void ErrorAddQuestionWithOneAnswer(string questionType,
            string expectedErrorText,
            string ngReflectModelValue)
        {
            var testName = "Простое название для теста 2021";
            var questionText = "Question text";
            var questionAnswer = "Question answer";
            var questionTextPlaceholder = "Текст вопроса";

            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(1000);
            driver.Wait(By.XPath("//span[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить вопрос\')]")).Click();

            var questionTextArea = By.XPath($"//textarea[@placeholder='{questionTextPlaceholder}']");
            driver.Wait(questionTextArea);
            driver.FindElement(questionTextArea).Click();
            driver.FindElement(questionTextArea).SendKeys(questionText);

            var matSelect = By.XPath("//mat-label[text()=\'Тип вопроса\']/../../../mat-select/div/div");
            driver.Wait(matSelect);
            driver.FindElement(matSelect).Click();
            var matSpan = By.XPath($"//span[@class='mat-option-text'and contains(.,\'{questionType}\')]");
            driver.Wait(matSpan);
            driver.FindElement(matSpan).Click();

            var questionAnswerInput = By.XPath($"//input[@ng-reflect-model=\'{ngReflectModelValue}\']");
            var questionAnswerInputDiv = driver.FindElement(questionAnswerInput);

            switch (questionType)
            {
                case "С одним вариантом":
                case "С несколькими вариантами":
                    var divClass = questionType == "С одним вариантом" ?
                        "mat-radio-inner-circle" :
                        "mat-checkbox-inner-container";

                    driver.Wait(questionAnswerInput);
                    driver.FindElement(questionAnswerInput).Click();
                    driver.FindElement(questionAnswerInput).SendKeys(questionAnswer);

                    var label = questionAnswerInputDiv
                        .FindElement(By.XPath(".."))
                        .FindElement(By.XPath(".."));
                    var div = label.FindElement(By.ClassName(divClass));
                    IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
                    executor1.ExecuteScript("arguments[0].click()", div);
                    break;
                case "Последовательность элементов":
                    driver.Wait(questionAnswerInput);
                    driver.FindElement(questionAnswerInput).Click();
                    driver.FindElement(questionAnswerInput).SendKeys(questionAnswer);
                    break;
            }

            var saveButton = By.XPath("//span[@class='mat-button-wrapper' and contains(.,'Сохранить')]/../../button[@color='primary']");
            driver.FindElement(saveButton).Click();

            var expectedError = By.XPath($"//span[contains(.,'{expectedErrorText}')]");
            driver.Wait(expectedError);
            var results = driver.FindElements(expectedError);
            Assert.True(results.Any());
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(4)]
        [TestCase("Простое название для теста 2021", "Тест", "")]
        public void ErrorEditQuestionWithBadTextQuestion(string testName, string textQuestion, string badTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.XPath("//textarea[@placeholder=\'Текст вопроса\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys("t");
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys(Keys.Backspace);
            var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите название вопроса \')]"));
            Assert.True(errorMessage.Count > 0);
            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить \')]")).Click();
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(5)]
        [TestCase("Простое название для теста 2021", "0")]
        [TestCase("Простое название для теста 2021", "-11")]
        [TestCase("Простое название для теста 2021", "11")]
        public void ErrorEditQuestionWithBadComplexityQuestion(string testName, string complexityQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Тест")); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.XPath("//input[@placeholder=\'Уровень сложности\']"));
        
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(complexityQuestion);

            int errorMessagesCount = 0;
            if (int.Parse(complexityQuestion) <= 0)
            {
                driver.Wait(By.XPath("//mat-error[contains(.,\'Сложность вопроса должна быть больше нуля\')]"));
                errorMessagesCount = driver.FindElements(By.XPath("//mat-error[contains(.,\'Сложность вопроса должна быть больше нуля\')]")).Count;
            }
            else if (int.Parse(complexityQuestion) > 0)
            {
                driver.Wait(By.XPath("//mat-error[contains(.,\'Сложность вопроса не может быть больше 10\')]"));
                errorMessagesCount = driver.FindElements(By.XPath("//mat-error[contains(.,\'Сложность вопроса не может быть больше 10\')]")).Count;
            }

            Assert.True(errorMessagesCount > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить \')]")).Click();

            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(6)]
        public void ErrorDeleteQuestion()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);
            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Простое название для теста 2021"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Тест")); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить вопрос\']"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить вопрос\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Да\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Да\')]")).Click();
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить вопрос\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить вопрос\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
