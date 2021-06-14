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
    [TestFixture(), Order(1)]
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
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
        }

        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        public void InitializeData(string fullSubjectName, string shortSubjectName)
        {
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
        }

        public void ClearData(string fullSubjectName)
        {
            Thread.Sleep(1000);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            Thread.Sleep(2000);
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            driver.ClickJS(By.XPath("//div[@class=\'mat-dialog-actions\']/button[contains(.,\'Удалить\')]"));
            Thread.Sleep(1000);
        }

        [Test, Order(1)]
        [TestCase("", "")]
        public void ErrorAddSubjectWithoutRequaredData(string fullsSubjectName, string shortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//button[contains(.,'Добавить предмет')]"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullsSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestabbreviation);
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("Новый предмет для ошибки", "НПДО", "", "")]
        public void ErrorEditSubjectWithoutRequaredData(string oldfFullSubjectName, string oldShotSubjectName, string newFullSubjectName, string newShotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            InitializeData(oldfFullSubjectName, oldShotSubjectName);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldfFullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));

            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys("t");
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys("t");
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShotSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");

            Assert.AreEqual("true", inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", inputTestabbreviation);
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно изменен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [Test, Order(3)]
        [TestCase("", "АТП")]
        [TestCase("Тестовый предмет, который не будет создан", "")]
        public void ErrorAddSubjectWithoutOneRequaredData(string fullSubjectName, string shotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//button[contains(.,'Добавить предмет')]"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shotSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestabbreviation);
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        [Test, Order(4)]
        [TestCase("Новый предмет для ошибки", "", "ES")]
        [TestCase("Новый предмет для ошибки", "ErrorSubjectName", "")]
        public void ErrorEditSubjectWithoutOneRequaredData(string oldFullSubjectName, string newFullSubjectName, string newShotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldFullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
           
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys("t");
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys("t");
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(Keys.Backspace);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShotSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            if (newFullSubjectName.Length == 0)
            {
                var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestname);
            }
            if (newShotSubjectName.Length == 0)
            {
                var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestabbreviation);
            }
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(5)]
        [TestCase("Новый предмет для ошибки", "НПДО")]
        public void ErrorAddSubjectWithExistingData(string fullSubjectName, string shortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);

            Thread.Sleep(1000);
            driver.Wait(By.CssSelector(".mat-row"));
            driver.Wait(By.XPath("//button[contains(.,'Добавить предмет')]"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();

            var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestabbreviation);
            var errorrMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с таким названием уже существует \')]"));
            Assert.True(errorrMessage.Count > 0);
            var message = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с такой аббревиатурой уже существует \')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
       
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(6)]
        [TestCase("Новый предмет для ошибки", "Что-то новое", "ЧТН")]
        public void ErrorEditSubjectWithExistingData(string oldSubjectName,string newFullSubjectName, string newShotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            InitializeData(newFullSubjectName, newShotSubjectName);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.ClickJS(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
          
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShotSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            var button = driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]"));
            var stateButton = button.GetAttribute("disabled");
            Assert.True(!string.IsNullOrEmpty(stateButton));
            var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.True(true, inputTestabbreviation);
            var errorrMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с таким названием уже существует \')]"));
            Assert.True(errorrMessage.Count > 0);
            var message = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с такой аббревиатурой уже существует \')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            Thread.Sleep(1000);
            ClearData(oldSubjectName);
            Thread.Sleep(3000);
            ClearData(newFullSubjectName);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}

