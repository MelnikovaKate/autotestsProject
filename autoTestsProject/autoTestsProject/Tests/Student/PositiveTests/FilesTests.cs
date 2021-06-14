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
    [TestFixture(), Order(4)]
    public class FilesTests
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

        [Test]
        [TestCase("test.docx")]
        public void DownloadFileStudent(string nameFile)
        {
            driver.GoToSubjects();
            driver.GoToChooseSubject();
            driver.GoToChoosenSubject(Defaults.SubjectName);
            Thread.Sleep(1000);
            driver.GoToModulus(Defaults.ModulusFiles);

            driver.SwitchTo().Frame(0);

            string path = @"downloadFiles";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            Thread.Sleep(3000);

            var downloadLink = driver
                .FindElement(By.XPath($"//a[contains(text(),\'{nameFile}\')]"))
                .GetAttribute("href");

            WebClient webClient = new WebClient();

            string name = driver.FindElement(By.XPath($"//a[contains(text(),\'{nameFile}\')]")).GetAttribute("ng-reflect-message");

            webClient.DownloadFile(downloadLink, path + "/" + name);

            string curFile = path + "/" + name;
            Assert.True(File.Exists(curFile));

            driver.SwitchTo().DefaultContent();
            driver.LogOut();
        }
    }
}
