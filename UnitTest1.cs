using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Threading;

namespace NalogDemoVersionTests
{
    public class Tests
    {
        private IWebDriver driver;
        private readonly By _lkSignInButton = By.XPath("//a[@class='main-menu__enter']");
        private readonly By _closeWindowButton = By.XPath("//button[@class='Button_button__2Lf63 Button_button__medium__2mCsW Button_button__orange__3eFfI button__100']"); //Временное всплывающее окно
        private readonly By _demoVersionButton = By.XPath("//a[@class='button button__medium button__100 button__outline-white']");
        private readonly By _lkPhoto = By.XPath("//a[@class='src-modules-App-components-UserInfo-UserInfo-module__photo']");
        private readonly By _lkSum = By.XPath("//span[@class='nowrap main-page_title_sum']");
        private readonly By _lkSumFruc = By.XPath("//span[@class='main-page_title_sum_fractional']");

        private const string expectedMainPage = "https://www.nalog.gov.ru/rn58/";
        private const string expectedLoginPage = "https://lkfl2.nalog.ru/lkfl/login";
        private const string expectedDemoPage = "https://lkfl2.nalog.ru/lkfl-demo/";
        private const int expectedWidth = 30;
        private const int expectedHeight = 31;
        private const int expectedSum = 200000;
        private const int sleepTime = 500;

        [SetUp]
        public void Setup()
        {
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl("https://www.nalog.ru/");
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(expectedMainPage, driver.Url, "Не удалось открыть главную страницу сайта");

            var lkSignIn = driver.FindElement(_lkSignInButton);
            lkSignIn.Click();          

            //Закрытие всплывающего окна
            Thread.Sleep(sleepTime);
            var closeWindow = driver.FindElement(_closeWindowButton);
            closeWindow.Click();

            Assert.AreEqual(expectedLoginPage, driver.Url, "Не удалось открыть страницу входа в личный кабинет");

            Thread.Sleep(sleepTime);
            var demoVersion = driver.FindElement(_demoVersionButton);
            demoVersion.Click();

            Assert.AreEqual(expectedDemoPage, driver.Url, "Не открылась демо-версия кабинета");

            Thread.Sleep(500);
            var lkPhoto = driver.FindElement(_lkPhoto);
            var size = lkPhoto.Size;

            Assert.AreEqual(expectedWidth, size.Width, "Ширина иконки не соответствует заданному");
            Assert.AreEqual(expectedHeight, size.Height, "Высота иконки не соответствует заданному");

            var lkSum = driver.FindElement(_lkSum);
            var lkSumFruc = driver.FindElement(_lkSumFruc);
            var indexOfFruc = lkSum.Text.IndexOf(lkSumFruc.Text);
            var sum = Convert.ToDouble(lkSum.Text.Substring(0, indexOfFruc));

            Assert.IsTrue(sum < expectedSum, "Сумма больше 200000");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            var process = Process.GetProcessesByName("chrome");
            Assert.IsEmpty(process);
        }
    }
}