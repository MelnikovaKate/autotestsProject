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

        [Test]
        [TestCase(" С одним вариантом ", "Как дела?", "Хорошо", "Плохо", "Никак")]
        public void goodAddQuestionWithOneAnswer(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" С несколькими вариантами ", "Что ты любишь?", "Холодник", "Бананы", "Дыня")]
        public void goodAddQuestionWithMoreAnswers(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" Ввод с клавиатуры ", "Спутник Земли?", "Луна")]
        public void goodAddQuestionWithEnterAnswer(string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" Последовательность элементов ", "Поставить в порядке убывания...", "1", "2", "3")]
        public void goodAddQuestionWithOrderedAnswers(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" С одним вариантом ", "Как дела?", "Хорошо", "Плохо", "Никак")]
        public void goodCancelAddQuestionWithOneAnswer(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" С несколькими вариантами ", "Что ты любишь?", "Холодник", "Бананы", "Дыня")]
        public void goodCancelAddQuestionWithMoreAnswers(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" Ввод с клавиатуры ", "Спутник Земли?", "Луна")]
        public void goodCancelAddQuestionWithEnterAnswer(string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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

        [Test]
        [TestCase(" Последовательность элементов ", "Поставить в порядке убывания...", "1", "2", "3")]
        public void goodCancelAddQuestionWithOrderedAnswers(string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
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



        [Test]
        [TestCase("Question", "NewQuestion")]
        public void goodCancelEditTextQuestion(string oldTextQuestion, string newTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest")); // название теста
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

        [Test]
        public void errorAddQuestionWithoutAllData()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem}]/mat-cell[3]/mat-icon[contains(.,\'help \')]")).Click();                                                          
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить вопрос\')]")).Click();

            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys("");
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

        [Test]
        public void errorAddQuestionWithoutAnswers()
        {
            driver.GoToSubjects();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);

            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem}]/mat-cell[3]/mat-icon[contains(.,\'help \')]")).Click();                                                          
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить вопрос\')]")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys("Test");
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys("2");
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(2000);
            var errorMessage = driver.FindElements(By.XPath("//span[contains(.,\'Проверьте варианты ответов. Они не должны быть пустыми\')]"));
            Assert.True(errorMessage.Count > 0);
            Thread.Sleep(1000);
            var goodMessage = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            Assert.True(goodMessage.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        public void errorDeleteQuestion()
        {
            driver.GoToSubjects();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);
            driver.SwitchTo().Frame(0);
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(2000);
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Тест")); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить вопрос\']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'Да\')]")).Click();
            Thread.Sleep(1000);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить вопрос\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [Test]
        [TestCase("TestTest","Test","")]
        public void errorEditQuestionWithBadTextQuestion(string testName,string textQuestion,string badTextQuestion)
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
            driver.Wait(By.XPath("//textarea[@placeholder=\'Текст вопроса\']"));
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys("t");
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys(Keys.Backspace);
            //driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys(badTextQuestion);
            var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите название вопроса \')]"));
            Assert.True(errorMessage.Count > 0);
            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить \')]")).Click();
            //driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Название вопроса не должно быть пустым\')]"));
            //var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Название вопроса не должно быть пустым\')]"));
            //Assert.True(elements.Count > 0);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("0")]
        [TestCase("-11")]
        [TestCase("11")]
        public void errorEditQuestionWithBadComplexityQuestion(string complexityQuestion)
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
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Тест")); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            driver.Wait(By.XPath("//input[@placeholder=\'Уровень сложности\']"));
            //driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            //driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Backspace);
            //driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Enter);         
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Clear();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(complexityQuestion);
            //driver.Wait(By.XPath("//mat-error[contains(.,\' Введите сложность \')]"));
            //var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите сложность \')]"));
            int errorMessagesCount = 0;
            if (int.Parse(complexityQuestion) <= 0)
            {
                driver.Wait(By.XPath("//mat-error[contains(.,\'Сложность вопроса должна быть больше нуля\')]"));
                errorMessagesCount = driver.FindElements(By.XPath("//mat-error[contains(.,\'Сложность вопроса должна быть больше нуля\')]")).Count;
            }
            else if(int.Parse(complexityQuestion) > 0)
            {
                driver.Wait(By.XPath("//mat-error[contains(.,\'Сложность вопроса не может быть больше 10\')]"));
                errorMessagesCount = driver.FindElements(By.XPath("//mat-error[contains(.,\'Сложность вопроса не может быть больше 10\')]")).Count;
            }
                      
            Assert.True(errorMessagesCount > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить \')]")).Click();
            //driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("Question", "NewQuestion")]
        public void goodEditTextQuestion(string oldTextQuestion, string newTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest")); // название теста
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

        [Test]
        [TestCase("Вариант для удаления")]
        public void GoodDeleteAnswerInQuestion(string valueForDelete)
        {
            driver.GoToSubjects();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("TestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Test")); // название вопроса
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

        [Test]
        public void ErrorEditQuestionsWithDifferentTestType(string testName, string questionText)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusName);

            driver.Wait(By.CssSelector(".mat-row"));
            var elementsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var element = elementsForFind.FirstOrDefault(x => x.Text.Contains(testName));
            var idRowOfElement = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(element);
            driver.Wait(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElement + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElement + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();

            driver.Wait(By.CssSelector(".mat-row"));
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains(questionText)); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            driver.Wait(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();



            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            //driver.Wait(By.XPath($"//span[contains(.,\'{typeQuestion}\')]"));
            //driver.FindElement(By.XPath($"//span[contains(.,\'{typeQuestion}\')]")).Click();
        }
    }
}
