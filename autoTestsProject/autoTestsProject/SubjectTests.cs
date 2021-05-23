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
    public class SubjectTests
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
        [TestCase("TestSubject1", "TS")]
        [TestCase("TestSubject2", "TS2")]
        [TestCase("TestSubject3", "TS3")]
        public void GoodAddSubjectLecturer(string fullSubjectName, string shortSubjectName)
        {
            var rowsCount = 0;

            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            rowsCount = driver.FindElements(By.CssSelector(".mat-row")).Count;
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(fullSubjectName);
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(shortSubjectName);
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
            driver.Wait(By.CssSelector("#mat-option-128 > .mat-option-pseudo-checkbox"));
            driver.FindElement(By.CssSelector("#mat-option-128 > .mat-option-pseudo-checkbox")).Click();
            driver.FindElement(By.CssSelector(".cdk-overlay-transparent-backdrop")).Click();
            Assert.That(driver.FindElement(By.XPath("//mat-dialog-container[@id=\'mat-dialog-0\']/app-news-popover/div/div[2]/button[2]")).Text, Is.EqualTo("Сохранить"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//a[contains(.,\'Предметы\')]"));
            var elements = driver.FindElements(By.XPath("//a[contains(.,\'Предметы\')]"));
            Assert.True(elements.Count > 0);
            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            Assert.True(driver.FindElements(By.CssSelector(".mat-row")).Count != rowsCount);
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("TestSubject1", "NewTestSubject1", "NTS1")]
        [TestCase("TestSubject2", "NewTestSubject2", "NTS2")]
        [TestCase("TestSubject3", "NewTestSubject3", "NTS3")]
        public void GoodEditSubjectLecturer(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
        {
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//button[@mattooltip=\'Редактировать предмет\']"));
            var elements = driver.FindElements(By.XPath("//button[@mattooltip=\'Редактировать предмет\']"));
            Assert.True(elements.Count > 0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldFullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/button[@mattooltip=\'Редактировать предмет\']")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Click();
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo(oldFullSubjectName));
            }
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(newFullSubjectName);
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo(newFullSubjectName));
            }
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).Clear();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(newShortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//td[contains(.,\'{newFullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{newFullSubjectName}\')]"));
            Assert.True(els.Count > 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("Лекции")]
        [TestCase("Практические занятия")]
        [TestCase("Лабораторные работы")]
        [TestCase("Курсовые проекты/работы")]
        [TestCase("ЭУМК")]
        [TestCase("Интерактивный учебник")]
        public void GoodEditSubjectChangeModulus(string modulus)
        {
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();

            driver.Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
            driver.Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            driver.Wait(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            var elements = driver.FindElements(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            Assert.True(elements.Count > 0);
            Thread.Sleep(1500);
            driver.Wait(By.XPath("//a[contains(.,\'Настройки\')]"));
            driver.FindElement(By.XPath("//a[contains(.,\'Настройки\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();
            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click(); // div[3]
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();

            //driver.Wait(By.XPath("//div[@id=\'toast-container\']/app-toast/div[contains(.,\'Предмет успешно отредактирован\')]"));
            //elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/app-toast/div[contains(.,\'Предмет успешно отредактирован\')]"));
            //Assert.True(elements.Count > 0);

            //driver.SwitchTo().DefaultContent();
            //driver.Wait(By.XPath("//a[contains(@href, \'/web/viewer\')]"));
            //driver.FindElement(By.XPath("//a[contains(@href, \'/web/viewer\')]")).Click();
            //driver.Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
            //driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
            //driver.Wait(By.XPath("//a[contains(.,\'TestSubject\')]"));
            //Thread.Sleep(5000);
            //driver.FindElement(By.XPath("//a[contains(.,\'TestSubject\')]")).Click();
            driver.Navigate().Refresh();
            //driver.Wait(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            elements = driver.FindElements(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            Assert.True(elements.Count == 0);
            driver.Wait(By.XPath("//a[contains(.,\'Настройки\')]"));
            driver.FindElement(By.XPath("//a[contains(.,\'Настройки\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();
            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.LogOut();
        }

        [Test]
        [TestCase("NewTestSubject1")]
        [TestCase("NewTestSubject2")]
        [TestCase("NewTestSubject3")]
        public void GoodDeleteSubjectLecturer(string fullSubjectName)
        {
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            Thread.Sleep(3000);
            var elements = driver.FindElements(By.XPath("//button[@mattooltip=\'Удалить предмет\']"));
            Assert.True(elements.Count > 0);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td[3]/button[@mattooltip=\'Удалить предмет\']")).Click();
            Thread.Sleep(3000);
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();
            Thread.Sleep(4000);
            //var els = driver.FindElements(By.XPath($"//td[contains(.,\'{item[1]}\')]"));
            //Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        public void ErrorAddSubjectLecturer()
        {
            var rowsCount = 0;
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();
            driver.GoToManagementSubject();

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
            Assert.True(true, inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestabbreviation);
            Thread.Sleep(1000);
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        public void ErrorEditSubjectLecturer()
        {
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

            driver.GoToSubjects();
            driver.GoToManagementSubject();

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
            driver.LogOut();
        }
    }
}