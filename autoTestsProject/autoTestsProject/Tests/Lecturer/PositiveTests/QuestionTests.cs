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
    [TestFixture(), Order(8)]
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
        [TestCase("Простое название для теста 2021", " С одним вариантом ", "Тест", "Хорошо", "Плохо", "Никак")]
        [TestCase("Простое название для теста 2021", " С одним вариантом ", "Как твои дела?", "Хорошо", "Плохо", "Никак")]
        [TestCase("Простое название для теста 2021", " С одним вариантом ", "Б", "Хорошо", "Плохо", "Никак")]
        [TestCase("Простое название для теста 2021", " С одним вариантом ", "Очень-очень длинный предлинный даже сверхдлинный " +
                  "текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный " +
                  "предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже_вот", "Хорошо", "Плохо", "Никак")]
        public void AddQuestionWithOneAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
            driver.ClickJS(By.XPath("//button[contains(.,\'Сохранить\')]"));
            //driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("Простое название для теста 2021", " С одним вариантом ", "Как твои дела?", "Хорошо", "Плохо", "Никак")]
        public void CancelAddQuestionWithOneAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " С несколькими вариантами ", "Что ты любишь больше всего любишь есть?", "Холодник", "Бананы", "Дыня")]
        public void AddQuestionWithMoreAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " С несколькими вариантами ", "Что ты любишь больше всего любишь есть?", "Холодник", "Бананы", "Дыня")]
        public void CancelAddQuestionWithMoreAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " Ввод с клавиатуры ", "Как называется спутник Земли?", "Луна")]
        public void AddQuestionWithEnterAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
            
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " Ввод с клавиатуры ", "Как называется спутник Земли?", "Луна")]
        public void CancelAddQuestionWithEnterAnswer(string testName, string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " Последовательность элементов ", "Поставить в порядке убывания числа, которые приведены ниже...", "1", "2", "3")]
        public void AddQuestionWithOrderedAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
           
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021", " Последовательность элементов ", "Поставить в порядке убывания числа, которые приведены ниже...", "1", "2", "3")]
        public void CancelAddQuestionWithOrderedAnswers(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);
           
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
        [TestCase("Простое название для теста 2021")]
        public void DoingTest(string testName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать тест\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать тест\']"));
            driver.Wait(By.Id("mat-input-2"));
            var inputQuestion = driver.FindElement(By.Id("mat-input-2"));
            var value = inputQuestion.GetAttribute("ng-reflect-value");
            driver.FindElement(By.XPath("//button[contains(.,\'Закрыть\')]")).Click();
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']"));
            driver.Wait(By.XPath("//button[contains(.,\'Далее\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            for (int i = 1; i <= int.Parse(value); i++)
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

        [Test, Order(10)]
        [TestCase("Простое название для теста 2021", "Очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный" +
                  " даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный " +
                  "даже_вот", "Измененный_ длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса " +
                  "2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже_вот")]
        [TestCase("Простое название для теста 2021", "Б", "Е")]
        public void EditTextQuestion(string testName, string oldTextQuestion, string newTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTextQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit\')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTextQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit\')]"));
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
            driver.LogOut();
        }

        [Test, Order(9)]
        [TestCase("Простое название для теста 2021", "Очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный" +
                  " даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный " +
                  "даже_вот", "Измененный_ длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный " +
                  "предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже_вот")]
        [TestCase("Простое название для теста 2021", "Б", "Е")]
        public void CancelEditTextQuestion(string testName, string oldTextQuestion, string newTextQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\\']"));

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTextQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{oldTextQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));

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
        [TestCase("Простое название для теста 2021", "Е", "Вариант для удаления")]
        public void DeleteAnswerInQuestion(string testName, string textQuestion, string valueForDelete)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'edit \')]"));

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
        [TestCase("Простое название для теста 2021", "Е")]
        [TestCase("Простое название для теста 2021", "Измененный_ длинный предлинный " +
                  "даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный " +
                  "предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже_вот")]
        [TestCase("Простое название для теста 2021", "Что ты любишь больше всего любишь есть?")]
        [TestCase("Простое название для теста 2021", "Как называется спутник Земли?")]
        [TestCase("Простое название для теста 2021", "Поставить в порядке убывания числа, которые приведены ниже...")]
        [TestCase("Простое название для теста 2021", "Как твои дела?")]
        public void CancelDeleteQuestion(string testName,string textQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));

            driver.Wait(By.XPath("//span[contains(.,\'Закрыть\')]"));
            driver.ClickJS(By.XPath("//span[contains(.,\'Закрыть\')]"));

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            var deleteQuestion = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            Assert.True(deleteQuestion.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(13)]
        [TestCase("Простое название для теста 2021", "Е")]
        [TestCase("Простое название для теста 2021", "Измененный_ длинный предлинный " +
                  "даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный " +
                  "предлинный даже сверхдлинный текст для вопроса 2021 очень-очень длинный предлинный даже_вот")]
        public void DeleteQuestion(string testName, string textQuestion)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{textQuestion}\')]/mat-cell[3]/mat-icon[contains(.,\'delete\')]"));

            driver.Wait(By.XPath("//button[contains(.,\'Да\')]"));
            driver.ClickJS(By.XPath("//button[contains(.,\'Да\')]"));

            driver.Wait(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            var deleteQuestion = driver.FindElements(By.XPath($"//mat-cell[contains(.,\'{textQuestion}\')]"));
            Assert.True(deleteQuestion.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(14)]
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

        [Test, Order(15)]
        [TestCase("И", " С одним вариантом ", "Как твои дела?", "Хорошо", "Плохо", "Никак")]
        public void AddQuestionWithOneAnswerInTestForStudents(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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
            driver.ClickJS(By.XPath("//button[contains(.,\'Сохранить\')]"));
            //driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            driver.Wait(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]"));
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(16)]
        [TestCase("И", " С несколькими вариантами ", "Что ты любишь больше всего любишь есть?", "Холодник", "Бананы", "Дыня")]
        public void AddQuestionWithMoreAnswersInTestForStudents(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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


        [Test, Order(17)]
        [TestCase("И", " Ввод с клавиатуры ", "Как называется спутник Земли?", "Луна")]
        public void AddQuestionWithEnterAnswerInTestForStudents(string testName, string typeQuestion, string textQuestion, string firstAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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

        [Test, Order(18)]
        [TestCase("И", " Последовательность элементов ", "Поставить в порядке убывания числа, которые приведены ниже...", "1", "2", "3")]
        public void AddQuestionWithOrderedAnswersInTestForStudents(string testName, string typeQuestion, string textQuestion, string firstAnswer, string secondAnswer, string thirdAnswer)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusName);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            driver.ClickJS(By.XPath($"//mat-table/mat-row[contains(.,\'{testName}\')]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']"));
            Thread.Sleep(3000);
            driver.Wait(By.XPath("//button/span[contains(.,'Добавить вопрос')]"));
            driver.FindElement(By.XPath("//button/span[contains(.,'Добавить вопрос')]")).Click();
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

    }
}
