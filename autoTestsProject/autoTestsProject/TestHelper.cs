﻿using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace autoTestsProject
{
    public static class TestHelper
    {
        public static void Login(this IWebDriver driver, string login, string password)
        {
            driver.Navigate().GoToUrl(Defaults.Url);
            //driver.Manage().Window.Size = new System.Drawing.Size(1680, 1050);
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("mat-input-0")).Click();
            driver.FindElement(By.Id("mat-input-0")).SendKeys(login);
            driver.FindElement(By.Id("mat-input-1")).Click();
            driver.FindElement(By.Id("mat-input-1")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@type=\'submit\']")).Click();
        }

        public static void LogOut(this IWebDriver driver)
        {
            driver.Wait(By.XPath("//mat-icon[contains(.,\'more_vert\')]"));
            driver.FindElement(By.XPath("//mat-icon[contains(.,\'more_vert\')]")).Click();
            driver.Wait(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'exit_to_appВыйти\')]")).Click();
            driver.Close();
        }

        public static void Wait(this IWebDriver driver, By param)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            wait.Until(d => driver.FindElements(param).Count > 0);
        }

        public static void GoToSubjects(this IWebDriver driver)
        {
            driver.Wait(By.XPath("//a[contains(@href, \'/web/viewer\')]"));
            driver.FindElement(By.XPath("//a[contains(@href, \'/web/viewer\')]")).Click(); //By.LinkText("Предметы")
        }

        public static void GoToManagementSubject(this IWebDriver driver)
        {
            driver.Wait(By.XPath("//button[contains(.,\'Управление предметом\')]"));
            driver.FindElement(By.XPath("//button[contains(.,\'Управление предметом\')]")).Click();
        }

        public static void GoToChooseSubject(this IWebDriver driver)
        {
            driver.Wait(By.XPath("//h2[contains(.,\'Выберите предмет\')]"));
            driver.FindElement(By.XPath("//h2[contains(.,\'Выберите предмет\')]")).Click();
        }

        public static void GoToChoosenSubject(this IWebDriver driver, string subjectName)
        {
            driver.Wait(By.XPath($"//a[contains(.,\'{subjectName}\')]"));
            Thread.Sleep(5000);
            driver.FindElement(By.XPath($"//a[contains(.,\'{subjectName}\')]")).Click();
        }

        public static void GoToModulus(this IWebDriver driver, string modulusName)
        {
            driver.Wait(By.XPath($"//a[contains(.,\'{modulusName}\')]"));
            driver.FindElement(By.XPath($"//a[contains(.,\'{modulusName}\')]")).Click();
        }
    }
}
