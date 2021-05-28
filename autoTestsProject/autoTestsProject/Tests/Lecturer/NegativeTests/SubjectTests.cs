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
    [TestFixture()]
    public class SubjectTests
    {
        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
            InitializeData();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ClearData();
            driver.Quit();
        }

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
            //InitializeData();
            // здесь делаю добавление
        }

        [TearDown]
        protected void TearDown()
        {
            //ClearData();
            // здесь делаю удаление
            driver.Quit();
        }

        public void InitializeData()
        {
            //[TestCase("Тестовый предмет для изучения БД", "ТПБД", "0", "0")]
            //public void AddSubjectLecturerWithOnlyNameAndAbbreviation(string fullSubjectName, string shortSubjectName, string countGroups, string countStudents)
            //{
             List<string> data = new List<string> { "Новый предмет для ошибки", "НПДО"};
                driver.GoToSubjects();
                driver.GoToManagementSubject();

                driver.SwitchTo().Frame(0);
                driver.Wait(By.CssSelector(".mat-row"));
                driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
                driver.Wait(By.Id("mat-input-0"));
                driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(data[0]);
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
                driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(data[1]);
                driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
                driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
                driver.SwitchTo().DefaultContent();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
            //}
        }

        public void ClearData()
        {
            driver = new ChromeDriver();
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
            List<string> data = new List<string> { "Новый предмет для ошибки"};

            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(data[0], StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            Thread.Sleep(2000);
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']")).Click();
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{data[0]}\"?\')]"));
            Thread.Sleep(2000);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        //[Test]
        //[TestCase("","")]
        //[TestCase("Тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень длинным " +
        //          "названием тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень " +
        //          "длинным названием тестовый предмет с очень-очень длинным названием тестовый пре", "ТПСООДН2021")]
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
            //driver.LogOut();
        }


        //[Test]
        //[TestCase("", "АТП")]
        //[TestCase("Тестовый предмет, который не будет создан", "")]
        //[TestCase("Тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень длинным " +
        //          "названием тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень " +
        //          "длинным названием тестовый предмет с очень-очень длинным названием тестовый пре", "ТПСООДН")]
        //[TestCase("Тестовый предмет с некорректной аббревиатурой", "ТПСООДН2021")]
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

        [Test] // !!!!
        [TestCase("Новый предмет для ошибки", "НПДО")]
        public void ErrorAddSubjectWithExistingData(string fullSubjectName, string shortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
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

        //[Test]
        //[TestCase("1TestSubject", "", "")]
        public void ErrorEditSubjectWithoutRequaredData(string fullSubjectName, string newFullSubjectName, string newShotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']")).Click();


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
            //Assert.True(inputTestname.ToLower());
            Assert.AreEqual("true", inputTestname);
            var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            Assert.AreEqual("true", inputTestabbreviation);
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно изменен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        //[Test]
        //[TestCase("", "ES")]
        //[TestCase("ErrorSubjectName", "")]
        //[TestCase(driver.DuplicateWord("LongNameSubjectT", 16),"LNST")]
        //[TestCase("ErrorSubject", )]
        public void ErrorEditSubjectWithoutOneRequaredData(string fullSubjectName, string shotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']")).Click();
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
            if (fullSubjectName.Length == 0 || fullSubjectName.Length > 256)
            {
                var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
                Assert.True(true, inputTestname);
            }
            if (shotSubjectName.Length == 0 || fullSubjectName.Length > 10)
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

        //[Test] // !!!!
        //[TestCase("TestSubject", "Т", "Т")]
        //[TestCase("2TestSubject", "2TS")]
        //[TestCase("3TestSubject", "3TS")]
        public void ErrorEditSubjectWithExistingData(string oldnameSubject,string fullSubjectName, string shotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldnameSubject, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shotSubjectName);
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
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}

