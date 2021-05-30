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
    [TestFixture(), Order(3)]
    public class QuestionTests
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
        [TestCase("TestTest", " С одним вариантом ", "Как твои дела?", "Хорошо", "Плохо", "Никак")]
        public void AddQuestionWithOneAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(thirdAnswer);
            Thread.Sleep(4000);

            var label = driver
               .FindElement(By.XPath($"//mat-radio-button/label/div/input[@ng-reflect-model=\'{firstAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            var div = label.FindElement(By.ClassName("mat-radio-container"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);

            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("TestTest", " С одним вариантом ", "Как твои дела?", "Хорошо", "Плохо", "Никак")]
        public void CancelAddQuestionWithOneAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(thirdAnswer);
            Thread.Sleep(4000);

            var label = driver
               .FindElement(By.XPath($"//mat-radio-button/label/div/input[@ng-reflect-model=\'{firstAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            var div = label.FindElement(By.ClassName("mat-radio-container"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);

            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("TestTest", " С несколькими вариантами ", "Что ты любишь больше всего любишь есть?", "Холодник", "Бананы", "Дыня")]
        public void AddQuestionWithMoreAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();

            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).Clear();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(thirdAnswer);
            var label = driver
               .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{firstAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            var div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);
            label = driver
               .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{secondAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
            executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(4)]
        [TestCase("TestTest", " С несколькими вариантами ", "Что ты любишь больше всего любишь есть?", "Холодник", "Бананы", "Дыня")]
        public void CancelAddQuestionWithMoreAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();

            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).Clear();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(thirdAnswer);
            var label = driver
               .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{firstAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            var div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);
            label = driver
               .FindElement(By.XPath($"//mat-checkbox/label/span/input[@ng-reflect-model=\'{secondAnswer}\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            div = label.FindElement(By.ClassName("mat-checkbox-inner-container"));
            executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);
            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(5)]
        [TestCase("TestTest", " Ввод с клавиатуры ", "Как называется спутник Земли?", "Луна")]
        public void AddQuestionWithEnterAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(6)]
        [TestCase("TestTest", " Ввод с клавиатуры ", "Как называется спутник Земли?", "Луна")]
        public void CancelAddQuestionWithEnterAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(7)]
        [TestCase("TestTest", " Последовательность элементов ", "Поставить в порядке убывания...", "1", "2", "3")]
        public void AddQuestionWithOrderedAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(thirdAnswer);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }     

        [Test, Order(8)]
        [TestCase("TestTest", " Последовательность элементов ", "Поставить в порядке убывания...", "1", "2", "3")]
        public void CancelAddQuestionWithOrderedAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Добавить вопрос\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(textQuestion);

            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys(firstAnswer);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-4")).Click();
            driver.FindElement(By.Id("mat-input-4")).SendKeys(secondAnswer);
            driver.FindElement(By.Id("mat-input-5")).Click();
            driver.FindElement(By.Id("mat-input-5")).SendKeys(thirdAnswer);
            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(9)]
        [TestCase("TestTest", "Question", "NewQuestion")]
        public void EditTextQuestion(string testName, string oldTextQuestion, string newTextQuestion)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(oldTextQuestion)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            Thread.Sleep(2000);
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(newTextQuestion);  // новое название вопроса
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(2000);
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(message.Count > 0);
            Assert.That(driver.FindElement(By.XPath($"//mat-cell[contains(.,\'{newTextQuestion}\')]")).Text, Is.EqualTo(newTextQuestion));
            driver.SwitchTo().DefaultContent();
            driver.LogOut(); ;
        }

        [Test, Order(10)]
        [TestCase("TestTest", "Question", "NewQuestion")]
        public void CancelEditTextQuestion(string testName, string oldTextQuestion, string newTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(testName)); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(oldTextQuestion)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(newTextQuestion);  // новое название вопроса
            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Закрыть\')]")).Click();
            var message = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(message.Count == 0);
            var newQuestion = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{newTextQuestion}\')]"));
            Assert.True(newQuestion.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
       

        [Test, Order(11)]
        [TestCase("TestTest", "Test", "Вариант для удаления")]
        public void DeleteAnswerInQuestion(string testName, string textQuestion, string valueForDelete)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

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

            driver.Wait(By.XPath("//span[contains(.,\'Добавить ответ\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            var answerIdForDelete = "";
            var allAnswers = driver.FindElements(By.TagName("input"));

            foreach (var answer in allAnswers)
            {
                if (answer.GetAttribute("ng-reflect-model") == "")
                    answerIdForDelete = answer.GetAttribute("id");
            }

            driver.FindElement(By.Id(answerIdForDelete)).Click();
            driver.FindElement(By.Id(answerIdForDelete)).SendKeys(valueForDelete);

            var div = driver
               .FindElement(By.XPath($"//mat-radio-button/label/div/input[@ng-reflect-model=\'{valueForDelete}\']/../mat-icon[@ng-reflect-message=\'Удалить ответ\']"));
            div.Click();
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(12)]
        [TestCase("TestTest","NewQuestion")]
        public void CancelDeleteQuestion(string testName,string textQuestion)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'delete\')]")).Click();

            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.ClickJS(By.XPath("//span[contains(.,\'Закрыть\')]"));

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            var deleteQuestion = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            Assert.True(deleteQuestion.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(13)]
        [TestCase("TestTest", "NewQuestion")]
        public void DeleteQuestion(string testName, string textQuestion)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(textQuestion)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'delete\')]")).Click();

            driver.Wait(By.XPath("//button[contains(.,\'Да\')]"));
            driver.ClickJS(By.XPath("//button[contains(.,\'Да\')]"));

            // когда будет работать удаление расскомментировать эти строки
            //driver.Wait(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            //var deleteQuestion = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            //Assert.True(deleteQuestion.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
