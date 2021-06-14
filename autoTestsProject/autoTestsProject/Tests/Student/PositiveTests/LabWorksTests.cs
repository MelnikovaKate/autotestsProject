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

namespace autoTestsProject.Tests.Student.PositiveTests
{
    [TestFixture(), Order(2)]
    public class LabWorksTests
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
            driver.Login(Defaults.StudentLogin, Defaults.StudentPassword);
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        [TestCase("Ф")]
        [TestCase("Тестовая лабораторная работа с очень длинным комментарием тестовая лабораторная работа с очень длинным " +
            "комментарием тестовая лабораторная работа с очень длинным комментарием тестовая лабораторная работа с очень длинным")]
        [TestCase("Какой-то длинноватый комментарий для лабораторной работы какой-то длинноватый комментарий для лабораторной работы")]
        public void AddLabworkStudent(string comments)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();

            driver.Wait(By.XPath("//button[contains(.,\'Добавить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();

            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.ClickJS(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.Wait(By.XPath("//span[contains(.,\'Лабораторная для теста\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Лабораторная для теста\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(comments);

            driver.Wait(By.XPath("//span[contains(.,\'Добавить файл\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();

            var exePath = AppDomain.CurrentDomain.BaseDirectory; //path to exe file
            var path = Path.Combine(exePath, "FilesStudent/test.docx");

            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys(path);

            driver.Wait(By.XPath($"//tr[@class=\'mdc-data-table__row\']/td[contains(.,\'Прикрепленный файл\')]"));

            driver.Wait(By.XPath("//button[contains(.,\'Отправить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.LogOut();
        }

        [Test, Order(2)]
        [TestCase("Ф", "Отредактированная работа Ф №1")]
        [TestCase("Тестовая лабораторная работа с очень длинным комментарием тестовая лабораторная работа с очень длинным " +
            "комментарием тестовая лабораторная работа с очень длинным комментарием тестовая лабораторная работа с очень длинным", "Отредактированная Тестовая лабораторная работа с очень длинным комментарием №2")]
        [TestCase("Какой-то длинноватый комментарий для лабораторной работы какой-то длинноватый комментарий для лабораторной работы", "Отредактированная Какой-то длинноватый комментарий для лабораторной работы №3")]
        public void EditLabworkStudent(string comments, string editComment)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();
  
            driver.Wait(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Редактировать лабораторную работу']"));
            driver.ClickJS(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Редактировать лабораторную работу']"));

            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(editComment);
            driver.FindElement(By.XPath("//span[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("Отредактированная работа Ф №1")]
        [TestCase("Отредактированная Тестовая лабораторная работа с очень длинным комментарием №2")]
        [TestCase("Отредактированная Какой-то длинноватый комментарий для лабораторной работы №3")]
        public void DeleteLabworkStudent(string comments)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();

            driver.Wait(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Удалить лабораторную работу']"));
            driver.ClickJS(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Удалить лабораторную работу']"));

            driver.Wait(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.ClickJS(By.XPath("//button[contains(.,\'Удалить\')]"));

            Thread.Sleep(1000);

            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{comments}\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
