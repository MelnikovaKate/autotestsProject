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
            driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);
        }

        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test]
        [TestCase("TestSubject1", "TS1", "0", "0")]
        [TestCase("TestSubject2", "TS2", "0", "0")]
        [TestCase("TestSubject3", "TS3", "0", "0")]
        public void GoodAddSubjectLecturerWithOnlyNameAndAbbreviation(string fullSubjectName, string shortSubjectName, string countGroups, string countStudents)
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
            var subject = allSubjects.FirstOrDefault(x => x.Text.Contains(fullSubjectName));
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

        [Test]
        [TestCase("TestSubject11", "TS11", "Тестовая")]
        [TestCase("TestSubject12", "TS12", "Тестовая")]
        [TestCase("TestSubject13", "TS13", "Тестовая")]
        public void GoodAddSubjectLecturerWithOnlyDefaultModulus(string fullSubjectName, string shortSubjectName, string groupName)
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

        [Test]
        [TestCase("1TestSubject", "1TS", "Тестовая", "Лабораторные работы")]
        [TestCase("2TestSubject", "2TS", "Тестовая", "Лекции")]
        [TestCase("3TestSubject", "3TS", "Тестовая", "Тестирование знаний")]
        public void GoodAddSubjectLecturerWithModulus(string fullSubjectName, string shortSubjectName, string groupName, string modulus)
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

        [TestCase("TestSubjectForClose", "TS1ForClose")]
        public void GoodCloseAddSubject(string fullSubjectName, string shortSubjectName)
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

        [Test]
        [TestCase("TestSubject1", "NewTestSubject1", "NTS1")]
        [TestCase("TestSubject2", "NewTestSubject2", "NTS2")]
        [TestCase("TestSubject3", "NewTestSubject3", "NTS3")]
        public void GoodEditSubjectNameAndAbbreviation(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
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
            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath($"//td[contains(.,\'{newFullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{newFullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("NewTestSubject1", "10701117")]
        [TestCase("NewTestSubject2", "10701117")]
        [TestCase("NewTestSubject3", "10701117")]
        public void GoodEditSubjectGroup(string fullSubjectName, string newNameGroup)
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

        [Test]
        [TestCase("1TestSubject", "Тестовая", "Лекции")]
        [TestCase("2TestSubject", "Тестовая", "Лабораторные работы")]
        [TestCase("3TestSubject", "Тестовая", "ЭУМК")]
        public void GoodEditSubjectAddNewModulus(string fullSubjectName, string groupName, string modulus)
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

        [TestCase("NewTestSubject1", "NewTestSubject1ForClose", "NTS1ForClose")]
        public void GoodCloseEditSubject(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
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
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{newFullSubjectName}\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
        //
        [Test]
        [TestCase("Лекции")]
        [TestCase("Практические занятия")]
        [TestCase("Лабораторные работы")]
        [TestCase("Курсовые проекты/работы")]
        [TestCase("ЭУМК")]
        [TestCase("Интерактивный учебник")]
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
        [Test]
        [TestCase("NewTestSubject1", "NewTestSubject11", "NST11")]
        [TestCase("NewTestSubject2", "NewTestSubject12", "NST12")]
        [TestCase("NewTestSubject3", "NewTestSubject13", "NST13")]
        public void GoodEditSubjectChangeNameAndAbbreviationThroughSetting(string oldFullSubjectName, string newFullSubjectName, string newShortSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(oldFullSubjectName);
            driver.GoToModulus(Defaults.modulusSetting);

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
        [Test]
        [TestCase("NewTestSubject", "NTS", "Лекции")]
        public void GoodCancelEditSubjectThroughSetting(string newFullSubjectName, string newShortSubjectName, string modulus)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusSetting);

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

        [Test]
        [TestCase("NewTestSubject1ForClose", "NTS1ForClose", "Лекции")]
        public void GoodCloseEditSubjectThroughSetting(string newFullSubjectName, string newShortSubjectName, string modulus)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.subjectName);
            driver.GoToModulus(Defaults.modulusSetting);

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


        [Test]
        [TestCase("NewTestSubject1")]
        [TestCase("NewTestSubject2")]
        [TestCase("NewTestSubject3")]
        public void GoodDeleteSubject(string fullSubjectName)
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
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();

            Thread.Sleep(8000);
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count == 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test]
        [TestCase("NewTestSubject1")]
        [TestCase("NewTestSubject2")]
        [TestCase("NewTestSubject3")]
        public void GoodCancelDeleteSubject(string fullSubjectName)
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
            driver.Wait(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));

            var message = driver.FindElements(By.XPath($"//mat-dialog-container[@id='mat-dialog-0']/app-delete-popover/app-popover-dialog/div/div/h2/p[contains(.,\'Вы действительно хотите удалить предмет \"{fullSubjectName}\"?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Отмена\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Отмена\')]")).Click();

            driver.Wait(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{fullSubjectName}\')]"));
            Assert.True(els.Count > 0);

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }


        //[Test]
        //public void ErrorAddSubjectLecturer()
        //{
        //    var rowsCount = 0;
        //    driver.Login(Defaults.LecturerLogin, Defaults.LecturerPassword);

        //    driver.GoToSubjects();
        //    driver.GoToManagementSubject();

        //    driver.SwitchTo().Frame(0);
        //    Thread.Sleep(5000);
        //    rowsCount = driver.FindElements(By.CssSelector(".mat-row")).Count;
        //    driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
        //    Thread.Sleep(3000);
        //    driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
        //    driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys("");
        //    driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
        //    driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys("");
        //    driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
        //    var button = driver.FindElement(By.XPath("//button[contains(.,\' Сохранить \')]"));
        //    var stateButton = button.GetAttribute("disabled");
        //    Assert.True(!string.IsNullOrEmpty(stateButton));
        //    var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
        //    Assert.True(true, inputTestname);
        //    var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
        //    Assert.True(true, inputTestabbreviation);
        //    Thread.Sleep(1000);
        //    var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
        //    Assert.True(message.Count == 0);
        //    driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
        //    driver.SwitchTo().DefaultContent();
        //    driver.LogOut();
        //}

        [Test]
        public void ErrorAddSubjectWithoutRequaredData()
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//button[contains(.,'Добавить предмет')]"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
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
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет успешно добавлен \')]"));
            Assert.True(message.Count == 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        
        [Test]
        [TestCase("", "ES")]
        [TestCase("ErrorSubjectName", "")]
        //[TestCase(driver.DuplicateWord("LongNameSubjectT", 16),"LNST")]
        //[TestCase("ErrorSubject", )]
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
        [TestCase("1TestSubject", "1TS")]
        [TestCase("2TestSubject", "2TS")]
        [TestCase("3TestSubject", "3TS")]
        public void ErrorAddSubjectWithExistingData(string fullSubjectName, string shotSubjectName)
        {
            driver.GoToSubjects();
            driver.GoToManagementSubject();

            driver.SwitchTo().Frame(0);
            driver.Wait(By.XPath("//button[contains(.,'Добавить предмет')]"));
            driver.FindElement(By.XPath("//button[contains(.,'Добавить предмет')]")).Click();
            driver.Wait(By.XPath("//input[@name=\'name\']"));
            //driver.FindElement(By.XPath("//input[@name=\'name\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'name\']")).SendKeys(fullSubjectName);
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).Click();
            driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).SendKeys(shotSubjectName);
            driver.FindElement(By.XPath("//button[contains(.,\'Сохранить\')]")).Click();
            driver.SwitchTo().DefaultContent();
            //Thread.Sleep(1000);


            driver.Wait(By.XPath("//div/div/div/div[contains(.,\' Предмет c таким именем уже существует \')]")); // не ищет
            var message = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет c таким именем уже существует \')]"));
            Assert.True(message.Count > 0);

            //driver.Wait(By.XPath("//div[@class=\'toast-bottom-right toast-container\'][contains(.,\' Предмет c таким именем уже существует \')]"));
            //var errorMessage = driver.FindElements(By.XPath("//div[@class=\'toast-bottom-right toast-container\'][contains(.,\' Предмет c таким именем уже существует \')]"));
            //driver.Wait(By.XPath("//div[contains(.,\' Предмет c таким именем уже существует \')]"));
            //var errorMessage = driver.FindElements(By.XPath("//div[contains(.,\' Предмет c таким именем уже существует \')]"));

            //Assert.True(errorMessage.Count > 0);
            //var stateButton = button.GetAttribute("disabled");
            //Assert.True(!string.IsNullOrEmpty(stateButton));
            //var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            //Assert.True(true, inputTestname);
            //var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            //Assert.True(true, inputTestabbreviation);
            //var errorrMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с таким названием уже существует \')]"));
            //Assert.True(errorrMessage.Count > 0);
            //var message = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с такой аббревиатурой уже существует \')]"));
            //Assert.True(message.Count > 0);
            //driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            //driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test] 
        [TestCase("1TestSubject","","")]
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


        [Test]
        [TestCase("", "ES")]
        [TestCase("ErrorSubjectName", "")]
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
            if(fullSubjectName.Length == 0 || fullSubjectName.Length > 256)
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

        [Test] // !!!!
        [TestCase("1TestSubject", "1TS")]
        [TestCase("2TestSubject", "2TS")]
        [TestCase("3TestSubject", "3TS")]
        public void ErrorEditSubjectWithExistingData(string fullSubjectName, string shotSubjectName)
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

            var errorMessage = driver.FindElements(By.XPath("//div/div/div/div[contains(.,\' Предмет c таким именем уже существует \')]"));
            Assert.True(errorMessage.Count > 0);
            //var stateButton = button.GetAttribute("disabled");
            //Assert.True(!string.IsNullOrEmpty(stateButton));
            //var inputTestname = driver.FindElement(By.XPath("//input[@name=\'name\']")).GetAttribute("aria-invalid");
            //Assert.True(true, inputTestname);
            //var inputTestabbreviation = driver.FindElement(By.XPath("//input[@name=\'abbreviation\']")).GetAttribute("aria-invalid");
            //Assert.True(true, inputTestabbreviation);
            //var errorrMessage = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с таким названием уже существует \')]"));
            //Assert.True(errorrMessage.Count > 0);
            //var message = driver.FindElements(By.XPath("//mat-error[contains(.,\' Предмет с такой аббревиатурой уже существует \')]"));
            //Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}