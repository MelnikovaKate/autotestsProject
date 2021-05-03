﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace autoTestsProject
{
    [TestFixture()]
    public class Test
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

        [Test]
        public void goodLoginUser()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "kate", "10039396" }, new string[] { "danilyuk", "kostya2478_KEY" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);
                Thread.Sleep(4000);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]);
                driver.FindElement(By.Id("mat-input-1")).Click();
                {
                    var element = driver.FindElement(By.CssSelector(".loginbtn > .mat-focus-indicator"));
                    Actions builder = new Actions(driver);
                    builder.MoveToElement(element).Perform();
                }
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]);
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(5000);
                var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
                Assert.True(elements.Count > 0);
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void errorLoginUser()
        {
            driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 919);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("u");
            driver.FindElement(By.Id("mat-input-1")).Click();
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo("u"));
            }
            var elements = driver.FindElements(By.CssSelector(".ng-tns-c87-0 > .mat-form-field-infix > .mat-warn .mat-input-element, .mat-form-field-invalid .mat-input-element "));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.Id("mat-input-1")).SendKeys("u");
            driver.FindElement(By.CssSelector(".col-second")).Click();
            var elems = driver.FindElements(By.CssSelector(".ng-tns-c87-1 > .mat-form-field-infix > .mat-warn .mat-input-element, .mat-form-field-invalid .mat-input-element "));
            Assert.True(elems.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodAddSubjectLecturer()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestSubject", "TS" }, new string[] { "TestSubject2", "TS2" }, new string[] { "TestSubject3", "TS3" } };
            foreach (var item in dataArray) {
                var rowsCount = 0;
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.LinkText("Предметы")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//button[contains(.,\'Управление предметом\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(5000);
                rowsCount = driver.FindElements(By.CssSelector(".mat-row")).Count;
                driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]);
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]);
                driver.FindElement(By.CssSelector("#mat-checkbox-3 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-5 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-7 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-9 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-4 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-6 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector("#mat-checkbox-8 .mat-checkbox-inner-container")).Click();
                driver.FindElement(By.CssSelector(".color-picker:nth-child(2)")).Click();
                driver.FindElement(By.CssSelector(".saturation-lightness")).Click();
                driver.FindElement(By.CssSelector(".mat-select-arrow-wrapper")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.CssSelector("#mat-option-128 > .mat-option-pseudo-checkbox")).Click();
                driver.FindElement(By.CssSelector(".cdk-overlay-transparent-backdrop")).Click();
                Assert.That(driver.FindElement(By.XPath("//mat-dialog-container[@id=\'mat-dialog-0\']/app-news-popover/div/div[2]/button[2]")).Text, Is.EqualTo("Сохранить"));
                driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
                driver.SwitchTo().DefaultContent();
                Thread.Sleep(2000);
                var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
                Assert.True(message.Count > 0);
                Thread.Sleep(4000);
                var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
                Assert.True(elements.Count > 0);
                Thread.Sleep(5000);
                driver.SwitchTo().Frame(0);
                Assert.True(driver.FindElements(By.CssSelector(".mat-row")).Count != rowsCount);
                var els = driver.FindElements(By.XPath($"//td[contains(.,\'{item[0]}\')]"));
                Assert.True(els.Count > 0);
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void goodEditSubjectLecturer()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestSubject", "NewTestSubject", "NTS" }, new string[] { "TestSubject2", "NewTestSubject2", "NTS2" }, new string[] { "TestSubject3", "NewTestSubject3", "NTS3" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.LinkText("Предметы")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//button[contains(.,\'Управление предметом\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(3000);
                var elements = driver.FindElements(By.XPath("//button[@mattooltip=\'Редактировать предмет\']"));
                Assert.True(elements.Count > 0);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(item[0], StringComparison.Ordinal));
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

                driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/button[@mattooltip=\'Редактировать предмет\']")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.Id("mat-input-0")).Click();
                {
                    string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                    Assert.That(value, Is.EqualTo(item[0]));
                }
                Thread.Sleep(3000);
                driver.FindElement(By.Id("mat-input-0")).Clear();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[1]);
                {
                    string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                    Assert.That(value, Is.EqualTo(item[1]));
                }
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).Clear();
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[2]);
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
                driver.SwitchTo().DefaultContent();
                Thread.Sleep(2000);
                var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
                Assert.True(message.Count > 0);
                driver.SwitchTo().Frame(0);
                Thread.Sleep(4000);
                var els = driver.FindElements(By.XPath($"//td[contains(.,\'{item[1]}\')]"));
                Assert.True(els.Count > 0);
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void goodDeleteSubjectLecturer()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestSubject", "NewTestSubject", "NTS" }, new string[] { "TestSubject2", "NewTestSubject2", "NTS2" }, new string[] { "TestSubject3", "NewTestSubject3", "NTS3" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.LinkText("Предметы")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//button[contains(.,\'Управление предметом\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(3000);
                var elements = driver.FindElements(By.XPath("//button[@mattooltip=\'Удалить предмет\']"));
                Assert.True(elements.Count > 0);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(item[1], StringComparison.Ordinal));
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/button[@mattooltip=\'Удалить предмет\']")).Click();
                Thread.Sleep(3000);
                //var elems = driver.FindElements(By.XPath("//mat-dialog-container[@id=\'mat-dialog-0\']/app-delete-popover/h2[contains(.,\'Удаление предмета\')]"));
                //Assert.True(elems.Count > 0);
                var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{item[1]}\"?\')]"));
                Assert.True(message.Count > 0);
                driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();
                Thread.Sleep(4000);
                //var els = driver.FindElements(By.XPath($"//td[contains(.,\'{item[1]}\')]"));
                //Assert.True(els.Count == 0);
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void goodRegisterUser()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestStudentUser4", "User123", "User123", "Kate", "Kate", "Kate", "test" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.XPath("//a[contains(.,\'Регистрация\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(3000);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys(item[0]); // login
                driver.FindElement(By.CssSelector(".ng-tns-c5-1 .mat-form-field-infix")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys(item[1]); // password
                driver.FindElement(By.Id("mat-input-2")).Click();
                driver.FindElement(By.Id("mat-input-2")).SendKeys(item[2]);
                driver.FindElement(By.Id("mat-input-3")).Click();
                driver.FindElement(By.Id("mat-input-3")).SendKeys(item[3]); // last n
                driver.FindElement(By.Id("mat-input-4")).Click();
                driver.FindElement(By.Id("mat-input-4")).SendKeys(item[4]); // first n
                driver.FindElement(By.Id("mat-input-5")).Click();
                driver.FindElement(By.Id("mat-input-5")).SendKeys(item[5]); // father n
                driver.FindElement(By.CssSelector(".mat-select-value > .ng-tns-c6-7")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Тестовая \')]")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector(".mat-select-placeholder")).Click();
                driver.FindElement(By.XPath("//mat-option/span[contains(.,\' Кличка любимого животного? \')]")).Click();
                driver.FindElement(By.Id("mat-input-6")).Click();
                driver.FindElement(By.Id("mat-input-6")).SendKeys(item[6]);
                driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'Зарегистрироваться\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void addNewStudentInDB()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "Cat", "Cat", "Cat" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("http://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\' person_add_alt_1\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//span[contains(.,\' Тестовая \')]")).Click();
                Thread.Sleep(4000);
                js.ExecuteScript("window.scrollTo(0,26)");
                Thread.Sleep(4000);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(item[0]));
                //var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(item[0], StringComparison.Ordinal)); //
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                if(driver.FindElements(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon[contains(.,\'clear\')]")).Count > 0)
                {
                    driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon")).Click();
                    Thread.Sleep(10000);
                    var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Студент успешно подтвержден\')]"));
                    Assert.True(message.Count > 0);
                }
                else
                {
                    driver.Close(); // TODO
                }
                //driver.FindElement(By.XPath("//tr[33]/td[3]/mat-icon")).Click();
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        string DuplicateWord(string word, int countOfRepeats)
        {
            var result = new StringBuilder();

            for (int i = 0; i < countOfRepeats; i++)
            {
                result.Append(word);
            }
            return result.ToString();
        }

    }
}
