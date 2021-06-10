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
    [TestFixture(), Order(10)]
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
        public void AddLabworkStudent()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);
            //Thread.Sleep(4000);
            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            //driver.FindElement(By.XPath("//button/span/span[contains(.,\'Защита работ \')]")).Click();
            //Thread.Sleep(2000);
            driver.Wait(By.XPath("//button[contains(.,\'Добавить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();
            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            //Thread.Sleep(2000);
            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.FindElement(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div")).Click();
            //Thread.Sleep(5000);
            driver.Wait(By.XPath("//span[contains(.,\'Test lab work\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Test lab work\')]")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();
            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys("/Users/katekuzmich/Desktop/test.docx");
            //Thread.Sleep(5000);
            driver.Wait(By.XPath("//button[contains(.,\'Отправить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.LogOut();
        }

        [Test, Order(2)]
        public void EditLabworkStudent()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);
            //Thread.Sleep(4000);
            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            Thread.Sleep(2000);
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            //Thread.Sleep(4000);
            driver.Wait(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']"));
            driver.FindElement(By.XPath("//button[@mattooltip='Редактировать лабораторную работу']")).Click();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).Clear();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Edit work");
            driver.FindElement(By.XPath("//span[contains(.,\'Отправить работу\')]")).Click();
            Thread.Sleep(1000);
            driver.SwitchTo().DefaultContent();
            Assert.That(driver.FindElement(By.XPath("//div[@id=\'toast-container\']/div/div")).Text, Is.EqualTo("Файл(ы) успешно отправлен(ы)"));
            driver.LogOut();
        }

        [Test, Order(3)]
        public void DeleteLabworkStudent()
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click(); // разобраться с надписью Защита работ
            driver.Wait(By.CssSelector(".mat-row"));
            var elemsForFind = driver.FindElements(By.CssSelector(".mat-row"));
            var elem = elemsForFind.FirstOrDefault(x => x.Text.Contains("ЛР2"));
            var idRowOfElem = driver.FindElements(By.CssSelector(".mat-row")).IndexOf(elem);
            //Thread.Sleep(4000);
            driver.Wait(By.XPath("//button[@mattooltip='Удалить лабораторную работу']"));
            driver.FindElement(By.XPath("//button[@mattooltip='Удалить лабораторную работу']")).Click();
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            Assert.True(message.Count > 0);
            driver.FindElement(By.XPath("//button[contains(.,\'Удалить\')]")).Click();
            //Thread.Sleep(4000);
            driver.Wait(By.XPath($"//td[contains(.,\' Edit work\')]"));
            var els = driver.FindElements(By.XPath($"//td[contains(.,\' Edit work\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
