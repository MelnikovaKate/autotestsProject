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
    [TestFixture(), Order(11)]
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
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            //Thread.Sleep(3000);
            driver.Wait(By.CssSelector(".mat-row"));
            //var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            //var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Простое название для теста 2021")); // название теста
            //var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            //Thread.Sleep(5000);
            //driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            driver.Wait(By.XPath($"//mat-cell[contains(.,\'Простое название для теста 2021\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.FindElement(By.XPath($"//mat-cell[contains(.,\'Простое название для теста 2021\')]/../mat-cell/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            //Thread.Sleep(2000);
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));

            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            //Dictionary<string, string> questions = new Dictionary<string, string>
            //{
            //    {"Сколько будет 3+2?", "5"},
            //    {"Спутник Земли?", "Луна"},
            //    {"На какой свет светофора необходимо переходить дорогу?", "Зеленый"},
            //};

            //for (int i = 1; i <= 3; i++)
            //{
            //    //Thread.Sleep(3000);
            //    driver.Wait(By.ClassName("question-question-text"));
            //    var divTextQuestion = driver.FindElement(By.ClassName("question-question-text"));
            //    var textQuestion = divTextQuestion.Text;
            //    var answer = questions[textQuestion];
            //    //Thread.Sleep(2000);
            //    driver.Wait(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{answer}\')]"));
            //    var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{answer}\')]"));
            //    var label = div.FindElement(By.XPath(".."));
            //    label.FindElement(By.ClassName("mat-radio-label-content")).Click();
            //    driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            //}
            //Thread.Sleep(2000);

            for (int i = 1; i <= 6; i++)
            {
                //Thread.Sleep(3000);
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

            driver.Wait(By.XPath("//app-test-result/div/div[contains(.,\' Тест на тему «Простое название для теста 2021» завершен \')]"));
            Assert.That(driver.FindElement(By.XPath("//app-test-result/div/div[contains(.,\' Тест на тему «Простое название для теста 2021» завершен \')]")).Text, Is.EqualTo("Тест на тему «Простое название для теста 2021» завершен"));
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
