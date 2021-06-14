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

namespace autoTestsProject.Tests.Student.NegativeTests
{
    [TestFixture(), Order(11)]
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
        public void ErrorAddLabworkWithoutFile()
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
            driver.ClickJS(By.XPath("//span[contains(.,\'Лабораторная для теста\')]"));

            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();

            elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
            Assert.True(elements.Count == 0);

            driver.LogOut();
        }

        [Test, Order(2)]
        public void ErrorAddLabworkWithoutNamelabwork()
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

            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys("Test lab");

            driver.Wait(By.XPath("//span[contains(.,\'Добавить файл\')]"));
            driver.FindElement(By.XPath("//span[contains(.,\'Добавить файл\')]")).Click();

            var exePath = AppDomain.CurrentDomain.BaseDirectory; //path to exe file
            var path = Path.Combine(exePath, "FilesStudent/test.docx");

            driver.FindElement(By.XPath("//input[@type=\'file\']")).SendKeys(path);

            driver.Wait(By.XPath($"//tr[@class=\'mdc-data-table__row\']/td[contains(.,\'Прикрепленный файл\')]"));

            driver.Wait(By.XPath("//button[contains(.,\'Отправить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Отправить работу\')]")).Click();
 
            driver.SwitchTo().DefaultContent();
            driver.Wait(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Произошла ошибка \')]"));
            var elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Произошла ошибка \')]"));
            Assert.True(elements.Count > 0);
            driver.LogOut();
        }


        //[TestCase("Лабораторная работа для удаления")]
        public void AddLabworkStudent(string comments)
        {
            //driver.GoToSubjects();
            //driver.GoToChooseSubject();
            //driver.GoToChoosenSubject(Defaults.SubjectName);
            //Thread.Sleep(1000);
            //driver.GoToModulus(Defaults.ModulusLabWorks);

            //driver.SwitchTo().Frame(0);

            //driver.Wait(By.XPath("//button[5]/span[3]"));
            //driver.FindElement(By.XPath("//button[5]/span[3]")).Click();

            Thread.Sleep(1000);

            driver.Wait(By.XPath("//button[contains(.,\'Добавить работу\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Добавить работу\')]")).Click();

            driver.FindElement(By.XPath("//span[contains(.,\' \')]")).Click();
            driver.Wait(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));
            driver.ClickJS(By.XPath("//mat-select[@id=\'mat-select-0\']/div/div"));

            driver.Wait(By.XPath("//span[contains(.,\'Лабораторная для теста\')]"));
            driver.ClickJS(By.XPath("//span[contains(.,\'Лабораторная для теста\')]"));

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

            //driver.LogOut();
        }

        [Test, Order(4)]
        [TestCase("Лабораторная работа для удаления")]
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

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();

            Thread.Sleep(1000);
            driver.Wait(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Удалить лабораторную работу']"));
            driver.ClickJS(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Удалить лабораторную работу']"));

            driver.Wait(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            var message = driver.FindElements(By.XPath($"//mat-dialog-container[contains(.,\'Вы действительно хотите удалить работу?\')]"));
            Assert.True(message.Count > 0);
            driver.Wait(By.XPath("//button[contains(.,\'Удалить\')]"));
            driver.ClickJS(By.XPath("//button[contains(.,\'Удалить\')]"));
            Thread.Sleep(1000);
            //Thread.Sleep(1000);

            var els = driver.FindElements(By.XPath($"//td[contains(.,\'{comments}\')]"));
            Assert.True(els.Count == 0);
            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }

        [Test, Order(3)]
        [TestCase("Лабораторная работа для удаления")]
        public void ErrorEditLabworkWithoutFile(string comments)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusLabWorks);

            driver.SwitchTo().Frame(0);

            driver.Wait(By.XPath("//button[5]/span[3]"));
            driver.FindElement(By.XPath("//button[5]/span[3]")).Click();

            Thread.Sleep(1000);
            AddLabworkStudent(comments);
            Thread.Sleep(1000);

            driver.Wait(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Редактировать лабораторную работу']"));
            driver.ClickJS(By.XPath($"//td[contains(.,\'{comments}\')]/../td/div/button[@mattooltip='Редактировать лабораторную работу']"));

            Thread.Sleep(1000);

            driver.Wait(By.XPath("//button[@ng-reflect-message=\'Удалить файл\']"));
            driver.ClickJS(By.XPath("//button[@ng-reflect-message=\'Удалить файл\']"));

            //Thread.Sleep(3000);
            driver.Wait(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            var elements = driver.FindElements(By.XPath("//button[@disabled=\'true\']/span[contains(.,\' Отправить работу \')]"));
            Assert.True(elements.Count > 0);
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'close\')]")).Click();
            driver.SwitchTo().DefaultContent();

            Thread.Sleep(3000);

            elements = driver.FindElements(By.XPath("//div[@id=\'toast-container\']/div/div[contains(.,\' Файл(ы) успешно отправлен(ы) \')]"));
            Assert.True(elements.Count == 0);

            driver.LogOut();
        }

    }
}
