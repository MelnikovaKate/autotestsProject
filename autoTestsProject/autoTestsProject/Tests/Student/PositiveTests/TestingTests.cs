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
    [TestFixture(), Order(12)]
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

        [TestCase("И")]
        public void DoingTest(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            for (int i = 1; i <= 6; i++)
            {
                driver.Wait(By.ClassName("question-answers-block-container"));
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
            var str = $"Тест на тему «{testName}» завершен";
            driver.Wait(By.XPath($"//app-test-result/div/div[contains(.,\'{str}\')]"));
            Assert.That(driver.FindElement(By.XPath($"//app-test-result/div/div[contains(.,\'{str}\')]")).Text, Is.EqualTo($"Тест на тему «{testName}» завершен"));
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
    }
}
