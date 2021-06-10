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

        [Test, Order(1)]
        [TestCase("П", "П", "0", "0")]
        [TestCase("Тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень длинным " +
                  "названием тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень " +
                  "длинным названием тестовый предмет с очень-очень длинным названием тестовый пр", "ТПСООДН202", "0", "0")]
        [TestCase("Нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021", "НТП", "0", "0")]
        [TestCase("Тестовый_предмет Базы данных", "ТПБД", "0", "0")]
        public void AddSubjectWithOnlyRequaredData(string fullSubjectName, string shortSubjectName, string countGroups, string countStudents)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            var allSubjects = driver.FindElements(By.CssSelector(".mat-row"));
            var subject = allSubjects.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName));
            var idRowOfSubject = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(subject);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']/span[contains(.,\'{countGroups}\')]"));
            var elem = driver.FindElements(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\'][contains(.,\'{countGroups}\')]"));
            Assert.True(elem.Count > 0);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\'][contains(.,\'{countStudents}\')]"));
            elem = driver.FindElements(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\'][contains(.,\'{countStudents}\')]"));
            Assert.True(elem.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("Тестовый предмет необходимый для кнопки закрытия", "ТСНДКЗ")]
        public void CloseAddSubject(string fullSubjectName, string shortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.Id("mat-input-0"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);
            driver.Wait(By.XPath("//mat-icon[contains(.,\'close\')]"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().Frame(0);

            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("П", "Д", "Д")]
        [TestCase("Тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень длинным " +
                  "названием тестовый предмет с очень-очень длинным названием тестовый предмет с очень-очень " +
                  "длинным названием тестовый предмет с очень-очень длинным названием тестовый пр", "Новый " +
                  "тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень длинным " +
                  "названием новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень " +
                  "длинным названием новый тестовый предмет с очень-очен", "НТПСООДН21")]
        [TestCase("Нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021", "Измененный нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021", "НИТП")]
        [TestCase("Тестовый_предмет Базы данных", "Отредактированный тестовый_предмет для изучения Базы данных", "ОТПБД")]
        public void EditSubjectWithOnlyRequaredData(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldFullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']")).Click();

            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo(newFullSubjectName));
            }
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShortSubjectName);
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

        [Test, Order(4)]
        [TestCase("Д", "10701117")]
        [TestCase("Новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень длинным " +
                  "названием новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень " +
                  "длинным названием новый тестовый предмет с очень-очен", "10701117")]
        [TestCase("Измененный нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021", "10701117")]
        [TestCase("Отредактированный тестовый_предмет для изучения Базы данных", "10701117")]
        public void EditSubjectAddGroup(string fullSubjectName, string newNameGroup)
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

            driver.Wait(By.XPath("//mat-select/div/div"));
            driver.FindElement(By.XPath("//mat-select/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{newNameGroup}\')]"));
            Thread.Sleep(1000);
            driver.FindElement(By.XPath($"//span[contains(.,\'{newNameGroup}\')]")).Click();
            driver.FindElement(By.XPath("//div[2]/div[3]")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            var allSubjects = driver.FindElements(By.CssSelector(".mat-row"));
            var subject = allSubjects.FirstOrDefault(x => x.Text.Contains(fullSubjectName));
            var idRowOfSubject = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(subject);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']"));
            var group = driver.FindElement(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']")).GetAttribute("ng-reflect-message");
            Assert.True(group.Contains(newNameGroup));

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(5)]
        [TestCase("Д", "У", "У")]
        [TestCase("Новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень длинным " +
                  "названием новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень " +
                  "длинным названием новый тестовый предмет с очень-очен", "Новый тестовый предмет с очень-очень длинным названием " +
                  "который необходимо удалить новый тестовый предмет с очень-очень длинным названием который необходимо удалить новый " +
                  "тестовый предмет с очень-очень длинным названием который необходимо удалить новый тес", "НТПСООДНДУ")]
        [TestCase("Измененный нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021", "Нормальный тестовый предмет для удаления", "НТПУ")]
        [TestCase("Отредактированный тестовый_предмет для изучения Базы данных", "Тестовый предмет для изучения БД для удаления", "ТСБДУ")]
        public void CloseEditSubject(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);

            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(oldFullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Редактировать предмет\']")).Click();

            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'close\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            Assert.True(message.Count == 0);
            driver.SwitchTo().Frame(0);
            var els = driver.FindElements(By.XPath($"//td[@class=\'mat-cell cdk-column-name mat-column-name ng-star-inserted\'][contains(.,\'{newFullSubjectName}\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(6)]
        [TestCase("Д")]
        [TestCase("Новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень длинным " +
                  "названием новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень" +
                  " длинным названием новый тестовый предмет с очень-очен")]
        [TestCase("Измененный нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021")]
        [TestCase("Отредактированный тестовый_предмет для изучения Базы данных")]
        public void CancelDeleteSubject(string fullSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']")).Click();
            Thread.Sleep(1000);
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.ClickJS(By.XPath("//button[contains(.,\'Отмена\')]")); 

            var els = driver.FindElements(By.XPath($"//td[@class=\'mat-cell cdk-column-name mat-column-name\'][contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(7)]
        [TestCase("Д")]
        [TestCase("Новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень длинным " +
                  "названием новый тестовый предмет с очень-очень длинным названием новый тестовый предмет с очень-очень" +
                  " длинным названием новый тестовый предмет с очень-очен")]
        [TestCase("Измененный нормальный тестовый предмет, у которого очень-очень длинное предлинное название для " +
                  "тестирования создания предмета 2021")]
        [TestCase("Отредактированный тестовый_предмет для изучения Базы данных")]
        public void DeleteSubject(string fullSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']")).Click();
            Thread.Sleep(1000);
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.ClickJS(By.XPath("//button[contains(.,\'Удалить\')]"));

            Thread.Sleep(8000);
            var els = driver.FindElements(By.XPath($"//td[@class=\'mat-cell cdk-column-name mat-column-name ng-star-inserted\'][contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(8)]
        [TestCase("М", "М", "Тестовая")]
        [TestCase("Предмет с группой и дефолтными модулями предмет с группой и дефолтными модулями предмет с группой и " +
                  "дефолтными модулями предмет с группой и дефолтными модулями предмет с группой и дефолтными модулями " +
                  "предмет с группой и дефолтными модулями предмет с группо", "TS11", "Тестовая")]
        [TestCase("Предмет для тестовой группы", "ПСГИДМ2021", "Тестовая")]
        [TestCase("Просто тестовый предмет", "ПТП", "Тестовая")]
        public void AddSubjectWithGroup(string fullSubjectName, string shortSubjectName, string groupName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);

            driver.Wait(By.XPath("//mat-select/div/div"));
            driver.FindElement(By.XPath("//mat-select/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            Thread.Sleep(1000);
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.FindElement(By.XPath("//div[2]/div[3]")).Click();
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            var allSubjects = driver.FindElements(By.CssSelector(".mat-row"));
            var subject = allSubjects.FirstOrDefault(x => x.Text.Contains(fullSubjectName));
            var idRowOfSubject = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(subject);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']/span"));
            var elem = driver.FindElement(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']")).Text;
            Assert.True(int.Parse(elem) > 0);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\']"));
            elem = driver.FindElement(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\']")).Text;
            Assert.True(int.Parse(elem) > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();

        }

        [Test, Order(9)]
        [TestCase("М")]
        [TestCase("Предмет с группой и дефолтными модулями предмет с группой и дефолтными модулями предмет с группой и " +
                 "дефолтными модулями предмет с группой и дефолтными модулями предмет с группой и дефолтными модулями " +
                 "предмет с группой и дефолтными модулями предмет с группо")]
        [TestCase("Предмет для тестовой группы")]
        [TestCase("Просто тестовый предмет")]
        public void DeleteSubjectWithGroup(string fullSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']")).Click();
            Thread.Sleep(1000);
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();

            Thread.Sleep(8000);
            var els = driver.FindElements(By.XPath($"//td[@class=\'mat-cell cdk-column-name mat-column-name ng-star-inserted\'][contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(10)]
        [TestCase("К", "К", "Тестовая", "Лабораторные работы")]
        [TestCase("Очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный пр", "ООДПСМ2021", "Тестовая", "Лекции")]
        [TestCase("Новый тестовый предмет НПО", "НТПНПО", "Тестовая", "Тестирование знаний")]
        [TestCase("ДП Тестовый предмет", "ДПТП", "Тестовая", "Тестирование знаний")]
        public void AddSubjectWithModulus(string fullSubjectName, string shortSubjectName, string groupName, string modulus)
        {

            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shortSubjectName);

            driver.Wait(By.XPath("//mat-select/div/div"));
            driver.FindElement(By.XPath("//mat-select/div/div")).Click();
            driver.Wait(By.XPath($"//span[contains(.,\'{groupName}\')]"));
            Thread.Sleep(1000);
            driver.FindElement(By.XPath($"//span[contains(.,\'{groupName}\')]")).Click();
            driver.FindElement(By.XPath("//div[2]/div[3]")).Click();

            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();

            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count > 0);
            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            var allSubjects = driver.FindElements(By.CssSelector(".mat-row"));
            var subject = allSubjects.FirstOrDefault(x => x.Text.Contains(fullSubjectName));
            var idRowOfSubject = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(subject);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']/span"));
            var elem = driver.FindElement(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-groups mat-column-groups ng-star-inserted\']")).Text;
            Assert.True(int.Parse(elem) > 0);

            driver.Wait(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\']"));
            elem = driver.FindElement(By.XPath($"//tr[{idRowOfSubject + 1}]/td[@class=\'mat-cell cdk-column-students mat-column-students ng-star-inserted\']")).Text;
            Assert.True(int.Parse(elem) > 0);

            driver.SwitchTo().DefaultContent();

            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(fullSubjectName);

            driver.Wait(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            var findModulus = driver.FindElements(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            Assert.True(findModulus.Count > 0);


            driver.LogOut();
        }
   

        [Test, Order(11)]
        [TestCase("К", "Лекции")]
        [TestCase("Очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный пр", "Лабораторные работы")]
        [TestCase("Новый тестовый предмет НПО", "ЭУМК")]
        [TestCase("ДП Тестовый предмет", "Лабораторные работы")]
        public void EditSubjectAddNewModulus(string fullSubjectName, string modulus)
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

            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();

            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            Assert.True(message.Count > 0);

            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(fullSubjectName);

            driver.Wait(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            var findModulus = driver.FindElements(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            Assert.True(findModulus.Count > 0);

            driver.LogOut();
        }

       

        [Test, Order(12)]
        [TestCase("К")]
        [TestCase("Очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями очень-очень длинный предмет с модулями " +
                  "очень-очень длинный пр")]
        [TestCase("Новый тестовый предмет НПО")]
        public void DeleteSubjectWithModulus(string fullSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.StartsWith(fullSubjectName, StringComparison.Ordinal));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);

            driver.Wait(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']"));
            driver.FindElement(By.XPath($"//tr[{idRowOfElem + 1}]/td/button[@ng-reflect-message=\'Удалить предмет\']")).Click();
            Thread.Sleep(1000);
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();

            Thread.Sleep(8000);
            var els = driver.FindElements(By.XPath($"//td[@class=\'mat-cell cdk-column-name mat-column-name ng-star-inserted\'][contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        //
        //[Test]
        //[TestCase("Лекции")]
        //[TestCase("Практические занятия")]
        //[TestCase("Лабораторные работы")]
        //[TestCase("Курсовые проекты/работы")]
        //[TestCase("ЭУМК")]
        //[TestCase("Интерактивный учебник")]
        public void GoodEditSubjectChangeModulusThroughSetting(string modulus)
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
            Thread.Sleep(2000);
            driver.Wait(By.XPath("//a[contains(.,\'Настройки\')]"));
            driver.FindElement(By.XPath("//a[contains(.,\'Настройки\')]")).Click();
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();
            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            Thread.Sleep(1000);
            driver.Navigate().Refresh();
            Thread.Sleep(1500);
            var notFindElements = driver.FindElements(By.XPath($"//a[contains(.,\'{modulus}\')]"));
            Assert.True(notFindElements.Count == 0);
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

        // тест будет ломаться на GoToChoosenSubject(), т.к. не меняется название
        //[Test]
        //[TestCase("NewTestSubject1", "NewTestSubject11", "NST11")]
        //[TestCase("NewTestSubject2", "NewTestSubject12", "NST12")]
        //[TestCase("NewTestSubject3", "NewTestSubject13", "NST13")]
        public void GoodEditSubjectChangeNameAndAbbreviationThroughSetting(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(oldFullSubjectName);
            driver.GoToModulus(Defaults.ModulusSetting);

            Thread.Sleep(2000);
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();

            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            {
                string value = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("value");
                Assert.That(value, Is.EqualTo(oldFullSubjectName));
            }
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            driver.FindElement(By.XPath("//input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(newFullSubjectName);
            {
                string value = driver.FindElement(By.Id("mat-input-0")).GetAttribute("value");
                Assert.That(value, Is.EqualTo(newFullSubjectName));
            }
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(newShortSubjectName);
            driver.Wait(By.XPath("//button[contains(.,\'Сохранить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно отредактирован \')]"));
            Assert.True(message.Count > 0);

            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(newFullSubjectName); // не перейдет, т.к. название не меняется

            driver.LogOut();
        }


        // тест будет ломаться на последнем Assert, т.к. кнопка "Отмена" не закрывает форму
        //[Test]
        //[TestCase("NewTestSubject", "NTS", "Лекции")]
        public void GoodCancelEditSubjectThroughSetting(string newFullSubjectName, string newShortSubjectName, string modulus)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusSetting);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();

            driver.Wait(By.XPath($"//div/input[@name=\'name\']"));
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).SendKeys(newFullSubjectName);

            driver.Wait(By.XPath($"//div/input[@name=\'abbreviation\']"));
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).SendKeys(newShortSubjectName);

            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();
            driver.Wait(By.XPath("//span[contains(.,\'Отмена\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Отмена\')]")).Click();

            Thread.Sleep(1000);
            var notFindElements = driver.FindElements(By.XPath($"//mat-dialog-container/app-subject-management[contains(.,\' Создание предмета \')]"));
            Assert.True(notFindElements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        //[Test]
        //[TestCase("NewTestSubject1ForClose", "NTS1ForClose", "Лекции")]
        public void GoodCloseEditSubjectThroughSetting(string newFullSubjectName, string newShortSubjectName, string modulus)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusSetting);

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]"));
            driver.FindElement(By.XPath("//mat-grid-tile/figure/div[contains(.,\'Редактирование предмета\')]")).Click();

            driver.Wait(By.XPath($"//div/input[@name=\'name\']"));
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).Clear();
            driver.FindElement(By.XPath($"//div/input[@name=\'name\']")).SendKeys(newFullSubjectName);

            driver.Wait(By.XPath($"//div/input[@name=\'abbreviation\']"));
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).Clear();
            driver.FindElement(By.XPath($"//div/input[@name=\'abbreviation\']")).SendKeys(newShortSubjectName);

            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.Wait(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]"));
            driver.FindElement(By.XPath($"//div/mat-checkbox/label/span[contains(.,\'{modulus}\')]")).Click();
            driver.Wait(By.XPath("//mat-icon[contains(.,\'close\')]"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();

            Thread.Sleep(1000);
            var notFindElements = driver.FindElements(By.XPath($"//mat-dialog-container/app-subject-management[contains(.,\' Создание предмета \')]"));
            Assert.True(notFindElements.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
