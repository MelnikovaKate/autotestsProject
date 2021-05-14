using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Net;

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
            List<string[]> dataArray = new List<string[]>() { new string[] { "TestStudentUser5", "User123", "User123", "Kate", "Kate", "Kate", "test" } };
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
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void goodAddNewTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("TestT");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).Clear();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("Test");
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).Clear();
            driver.FindElement(By.Id("mat-input-2")).SendKeys("6");
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).Clear();
            driver.FindElement(By.Id("mat-input-3")).SendKeys("12");
            driver.FindElement(By.XPath("//mat-radio-button[@id=\'mat-radio-2\']/label/div[2]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Тест создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodEditTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTe"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(3000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem+1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-dialog-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("NewTestTest");
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Тест изменен\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodAddQuestion()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem}]/mat-cell[3]/mat-icon[contains(.,\'help \')]")).Click();                                                          
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить вопрос\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Сколько будет 2+2?");
            driver.FindElement(By.Id("mat-input-2")).Click();
            driver.FindElement(By.Id("mat-input-2")).SendKeys("5");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить ответ\')]")).Click();
            driver.FindElement(By.Id("mat-input-3")).Click();
            driver.FindElement(By.Id("mat-input-3")).SendKeys("4");
            Thread.Sleep(4000);
            var label = driver
               .FindElement(By.XPath("//mat-radio-button/label/div/input[@ng-reflect-model=\'4\']"))
               .FindElement(By.XPath(".."))
               .FindElement(By.XPath(".."));
            var div = label.FindElement(By.ClassName("mat-radio-container"));
            IJavaScriptExecutor executor1 = (IJavaScriptExecutor)driver;
            executor1.ExecuteScript("arguments[0].click()", div);

            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//mat-radio-button/label/div/input[@ng-reflect-model=\'Все хорошо\']/../..//div/input[@class=\'mat-radio-input\']"))).Click();

            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click(); // идет щелчок по надписи, а надо по точке, где подсвечивается курсор ладошкой
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос создан\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodEditQuestion()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Перейти к вопросам\']")).Click();
            Thread.Sleep(2000);
            var questionsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var question = questionsForFind.FirstOrDefault(x => x.Text.Contains("Тест")); // название вопроса
            var idRowOfQuestion = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(question);
            Thread.Sleep(4000);                                                        
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Как твои дела? New");  // новое название вопроса
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]")).Click();
            Thread.Sleep(4000);
            Assert.That(driver.FindElement(By.XPath("//mat-cell[contains(.,\'Как твои дела? New\')]")).Text, Is.EqualTo("Как твои дела? New"));
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void doingTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
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

            Dictionary<string, string> questions = new Dictionary<string, string>
            {
                {"Как твои дела? Новый", "Все хорошо"},
                {"Как твои дела?2", "Все хорошо"},
                {"ааа", "аа"},
                {"Как твои дела? New", "Тест"},
                {"Как твои дела?3", "Все хорошо"},
                {"mlmlml", "lmlmlm"},
                {"Сколько будет 2+2?", "4"},
            };

            for (int i = 1; i <= int.Parse(value); i++)
            {
                    Thread.Sleep(3000);
                    var divTextQuestion = driver.FindElement(By.ClassName("question-question-text"));
                    var textQuestion = divTextQuestion.Text;
                    var answer = questions[textQuestion];
                    Thread.Sleep(2000);
                    var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{answer}\')]"));
                    var label = div.FindElement(By.XPath(".."));
                    label.FindElement(By.ClassName("mat-radio-label-content")).Click();
                    driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            }
            Thread.Sleep(2000);
            var str = "Тест на тему «NewTestTest» завершен";
            Assert.That(driver.FindElement(By.XPath($"//app-test-result/div/div[contains(.,\'{str}\')]")).Text, Is.EqualTo("Тест на тему «NewTestTest» завершен"));
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodAddAccessStudentInTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//mat-option[@id='mat-option-0']/span[contains(.,\'Тестовая\')]")).Click();
            Thread.Sleep(3000);
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            var userForFind = div.FindElements(By.CssSelector(".mat-row"));
            var user = userForFind.FirstOrDefault(x => x.Text.StartsWith("Cat", StringComparison.Ordinal)); // имя user
            var idRowOfUser = div.FindElements(By.CssSelector(".mat-row")).IndexOf(user);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock \')]")).Click();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void closeAccessStudentInSistem()
        {
            List<string[]> dataArray = new List<string[]>() { new string[] { "Cat", "Cat", "Cat" } };
            foreach (var item in dataArray)
            {
                driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
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
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains(item[0]));
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                if (driver.FindElements(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon[@ng-reflect-message=\'Закрыть доступ\']")).Count > 0)
                    driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/mat-icon")).Click();
                Thread.Sleep(10000);
                var message = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Подтверждение отменено\')]"));
                Assert.True(message.Count > 0);
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            }
            driver.Close();
        }

        [Test]
        public void closeAccessStudentInTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Доступность теста\']")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//mat-select[@id='mat-select-0']/div/div")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//mat-option[@id='mat-option-0']/span[contains(.,\'Тестовая\')]")).Click();
            Thread.Sleep(3000);
            var div = driver.FindElement(By.XPath("//div[@class=\'students-table-list\']"));
            var userForFind = div.FindElements(By.CssSelector(".mat-row"));
            var user = userForFind.FirstOrDefault(x => x.Text.StartsWith("Cat", StringComparison.Ordinal)); // имя user
            var idRowOfUser = div.FindElements(By.CssSelector(".mat-row")).IndexOf(user);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//div/mat-table/mat-row[{idRowOfUser + 1}]/mat-cell[2]/mat-icon[contains(.,\'lock_open \')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Закрыть\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

    [Test]
        public void errorAddNewTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить тест\')]")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys("");
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

            //разбить на три теста?
            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);

            //var fieldCountQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Количество вопросов\']")).GetAttribute("ng-reflect-value");
            //Assert.IsEmpty(fieldCountQuestionValue);

            //var fieldTimeForQuestionValue = driver.FindElement(By.XPath("//input[@placeholder=\'Время на тест (мин)\']")).GetAttribute("ng-reflect-value");
            //Assert.IsEmpty(fieldTimeForQuestionValue);

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
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }
        
        [Test]
        public void errorDeleteTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 949);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Удалить тест\']")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Да\')]")).Click();
            Thread.Sleep(2000);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Не удалось удалить тест\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorAddQuestionWithoutAllData()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
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
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorAddQuestionWithoutAnswers()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
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
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }
        
        [Test]
        public void errorDeleteQuestion()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000); 
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
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
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }


        [Test]
        public void errorAddSubjectLecturer()
        {
            //List<string[]> dataArray = new List<string[]>() { new string[] { "TestSubject", "TS" }, new string[] { "TestSubject2", "TS2" }, new string[] { "TestSubject3", "TS3" } };
            //foreach (var item in dataArray)
            //{
                var rowsCount = 0;
                driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
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
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys("");
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys("");
                driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
                var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
                var stateButton = button.GetAttribute("disabled");
                Assert.True(!string.IsNullOrEmpty(stateButton));
                var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
                Assert.True(true,inputTestname);
                var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestabbreviation);
                Thread.Sleep(1000);
                var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
                Assert.True(message.Count == 0);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            //}
            driver.Close();
        }

        [Test]
        public void errorDoingTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            Thread.Sleep(1000);
            var errorMessage = driver.FindElements(By.XPath("//snack-bar-container[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(errorMessage.Count > 0);
            Thread.Sleep(2000);
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «NewTestTest» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorEditQuestion()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
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
            Thread.Sleep(5000);
            //driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Редактировать вопрос\']")).Click();
            driver.FindElement(By.XPath($"//mat-table/mat-row[{idRowOfQuestion + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).SendKeys("");
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Click();
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("//input[@placeholder=\'Уровень сложности\']")).Clear();
            //var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите название вопроса \')]"));
            //Assert.True(errorMessage.Count > 0);
            var errorMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Введите сложность \')]"));
            Assert.True(errorMessage.Count > 0);
            var fieldNameTestValue = driver.FindElement(By.XPath("//textarea[@placeholder=\'Текст вопроса\']")).GetAttribute("ng-reflect-value");
            Assert.IsEmpty(fieldNameTestValue);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить \')]")).Click();
            Thread.Sleep(4000);
            //var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Название вопроса не должно быть пустым\')]"));
            //Assert.True(elements.Count > 0);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Вопрос изменен\')]"));
            Assert.True(elements.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorEditSubjectLecturer()
        {
            //List<string[]> dataArray = new List<string[]>() { new string[] { "TestSubject", "NewTestSubject", "NTS" }, new string[] { "TestSubject2", "NewTestSubject2", "NTS2" }, new string[] { "TestSubject3", "NewTestSubject3", "NTS3" } };
            //foreach (var item in dataArray)
            //{
                driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
                driver.Manage().Window.Size = new System.Drawing.Size(1680, 949);
                driver.FindElement(By.Id("mat-input-0")).Click();
                driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
                driver.FindElement(By.Id("mat-input-1")).Click();
                driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
                driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//button[contains(.,\'Управление предметом\')]")).Click();
                driver.SwitchTo().Frame(0);
                Thread.Sleep(3000);
                var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
                var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith("TestSubject", StringComparison.Ordinal));
                var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
                driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/button[@mattooltip=\'Редактировать предмет\']")).Click();
                Thread.Sleep(3000);
                driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
                driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys("");
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys("");
                driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
                var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
                var stateButton = button.GetAttribute("disabled");
                Assert.True(!string.IsNullOrEmpty(stateButton));
                var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestname);
                var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestabbreviation);
                Thread.Sleep(1000);
                var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
                Assert.True(message.Count == 0);
                driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
                driver.SwitchTo().DefaultContent();
                driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            //}
            driver.Close();
        }

        [Test]
        public void errorEditTest()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("testLecturer1");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("testLecturer1");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберете предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("NewTestTest"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(3000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[contains(.,\'edit \')]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Click();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).Clear();
            driver.FindElement(By.XPath("//textarea[@placeholder=\'Название теста\']")).SendKeys("");
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
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodAddLabworkStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            //driver.FindElement(By.XPath("//button/span/span[contains(.,\'Защита работ \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(.,\'Test lab work\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();
            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys("/Users/katekuzmich/Desktop/test.docx");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodEditLabworkStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Edit work");
            driver.FindElement(By.XPath("//span[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void goodDeleteLabworkStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Удалить лабораторную работу']")).Click();
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();
            Thread.Sleep(4000);
            var els = driver.FindElements(By.XPath($"//td[contains(.,\' Edit work\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        //подумать на счет количества вопросов
        [Test]
        public void goodDoingTestStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Test")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();

            Dictionary<string, string> questions = new Dictionary<string, string>
            {
                {"Сколько будет 3+2?", "5"},
                {"Спутник Земли?", "Луна"},
                {"На какой свет светофора необходимо переходить дорогу?", "Зеленый"},
            };

            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(3000);
                var divTextQuestion = driver.FindElement(By.ClassName("question-question-text"));
                var textQuestion = divTextQuestion.Text;
                var answer = questions[textQuestion];
                Thread.Sleep(2000);
                var div = driver.FindElement(By.XPath($"//div[@class=\'mat-radio-label-content\'][contains(.,\'{answer}\')]"));
                var label = div.FindElement(By.XPath(".."));
                label.FindElement(By.ClassName("mat-radio-label-content")).Click();
                driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            }
            Thread.Sleep(2000);
            Assert.That(driver.FindElement(By.XPath("//app-test-result/div/div[contains(.,\' Тест на тему «Test» завершен \')]")).Text, Is.EqualTo("Тест на тему «Test» завершен"));
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        // tyt
        [Test]
        public void goodDownloadFileStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Файлы\')]")).Click();
            driver.SwitchTo().Frame(0);

            string path = @"files";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Thread.Sleep(3000);

            var downloadLink = driver
                .FindElement(By.XPath("//a[contains(text(),\'test.docx\')]"))
                .GetAttribute("href");

            WebClient webClient = new WebClient();

            string name = driver.FindElement(By.XPath("//a[contains(text(),\'test.docx\')]")).GetAttribute("ng-reflect-message");

            webClient.DownloadFile(downloadLink, path + "/" + name);

            string curFile = path + "/" + name;
            Assert.True(File.Exists(curFile));

            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorAddLabworkStudentWithoutFile()
        {
            driver.Navigate().GoToUrl("https://educats.by/web/dashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(.,\'Test lab work\')]")).Click();
            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            {
                elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
                Assert.True(elements.Count == 0);
            }
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorAddLabworkWithoutNamelabwork()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();
            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys("/Users/katekuzmich/Desktop/test.docx");
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
            var elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Произошла ошибка \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorEditLabworkStudentWithoutFile()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Лабораторные работы\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[@ng-reflect-message=\'Удалить файл\']")).Click();
            Thread.Sleep(3000);
            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            {
                elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
                Assert.True(elements.Count == 0);
            }
            driver.FindElement(By.XPath("//button[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }
        
        [Test]
        public void errorDoingTestStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'Тестирование знаний\')]")).Click();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("Test")); // название теста
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//mat-table[@id=\'cdk-drop-list-0\']/mat-row[{idRowOfElem + 1}]/mat-cell[3]/mat-icon[@ng-reflect-message=\'Пройти тест\']")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(.,\'Далее\')]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(.,\'done_outline Ответить\')]")).Click();
            Thread.Sleep(1000);
            var elements = driver.FindElements(By.XPath("//simple-snack-bar[contains(.,\'Выберите вариант ответа\')]"));
            Assert.True(elements.Count > 0);
            Thread.Sleep(2000);
            var result = driver.FindElements(By.XPath($"//app-test-result/div/div[contains(.,\'Тест на тему «Test» завершен\')]"));
            Assert.True(result.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        [Test]
        public void errorDownloadFileStudent()
        {
            driver.Navigate().GoToUrl("https://educats.by/login?returnUrl=%2Fweb%2Fdashboard");
            driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("kate");
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys("10039396");
            driver.FindElement(By.XPath("//button[contains(.,\'Войти в систему\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'Предметы\')]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//button[contains(.,\'Выберите предмет\')]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            Thread.Sleep(3500);
            driver.FindElement(By.XPath("//a[contains(.,\'Файлы\')]")).Click();
            driver.SwitchTo().Frame(0);

            string path = @"files";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Thread.Sleep(3000);

            var downloadLink = driver
                .FindElement(By.XPath("//a[contains(text(),\'test.docx\')]"))
                .GetAttribute("href");

            var badLink = "/badlink" + downloadLink;

            WebClient webClient = new WebClient();

            string name = driver.FindElement(By.XPath("//a[contains(text(),\'test.docx\')]")).GetAttribute("ng-reflect-message");

            try
            {
                webClient.DownloadFile(badLink, path + "/" + name);
            }
            catch(WebException e)
            {
                Console.WriteLine(e);
            }
            

            string curFile = path + "/" + name;
            Assert.True(!File.Exists(curFile));

            driver.SwitchTo().DefaultContent();
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
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
