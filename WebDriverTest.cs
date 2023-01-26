using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace Lesson_6_WebDriver
{
    public class WebDriverTest
    {
        
        private readonly string baseUrl = "https://mail.yandex.com";
        private IWebDriver driver;
        private string userName = "vadim.kuryan.vka";
        private string userPassword = "Vka_6463296";
        private string mailAddress = "dragnir@tut.by";

        [SetUp]
        public void Setup()
        {
            string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            driver = new ChromeDriver(path + @"\drivers\", options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void LoginAndSendMailTest()
        {
            driver.Navigate().GoToUrl(baseUrl);

            IWebElement loginButton = driver.FindElement(By.XPath("//*[text()='Log in']/ancestor::button"));
            loginButton.Click();

            IWebElement loginName = driver.FindElement(By.CssSelector("[name = 'login']"));
            loginName.SendKeys(userName);

            IWebElement loginUser = driver.FindElement(By.CssSelector("[id = 'passp:sign-in']"));
            loginUser.Click();

            WebElementVisible(By.CssSelector("[name = 'passwd']")).SendKeys(userPassword);

            IWebElement submit = driver.FindElement(By.CssSelector("[id = 'passp:sign-in']"));
            submit.Click();

            Assert.AreEqual(userName, WebElementVisible(By.CssSelector("[class = 'user-account__name']")).Text);

            IWebElement writeNewMail = driver.FindElement(By.CssSelector("[class = 'Button2 Button2_type_link Button2_view_action Button2_size_m Layout-m__compose--pTDsx qa-LeftColumn-ComposeButton ComposeButton-m__root--fP-o9']"));
            writeNewMail.Click();

            IWebElement mailTo = driver.FindElement(By.XPath("//*[@id='compose-field-1']"));
            mailTo.SendKeys(mailAddress);

            IWebElement mailSubject = driver.FindElement(By.XPath("//*[@id='compose-field-subject-4']"));
            mailSubject.SendKeys("TestSubject");

            IWebElement mailBody = driver.FindElement(By.XPath("//*[@id='cke_1_contents']/div"));
            mailBody.SendKeys("TestBody");

            IWebElement sendLater = driver.FindElement(By.XPath("//*[@class='Button2-Icon new__icon--1nFOX']/ancestor::button"));
            sendLater.Click();

            IWebElement sendLaterToday = driver.FindElement(By.XPath("//div[@class='ComposeTimeOptions-Label']"));
            sendLaterToday.Click();

            WebElementVisible(By.XPath("//a[@href='#draft']")).Click();
            WebElementVisible(By.XPath("//button[@aria-describedby='tooltip-0-2']")).Click();

            string dratedMailPath = "//*[@title='TestSubject']";
            Assert.IsTrue(WebElementVisible(By.XPath(dratedMailPath)).Displayed);

            Assert.IsTrue(WebElementExist(By.XPath(dratedMailPath)).Displayed);
            WebElementClickable(By.XPath(dratedMailPath)).Click();

            Assert.IsTrue(WebElementVisible(By.XPath("//*[contains(text(),'dragnir')]")).Displayed);
            Assert.AreEqual("TestSubject", WebElementVisible(By.XPath("//*[@title='TestSubject']")).Text);
            Assert.IsTrue(WebElementExist(By.XPath("//*[text()='TestBody']")).Enabled);

            WebElementClickable(By.XPath("//button[@aria-disabled='false']")).Click();

            WebElementVisible(By.XPath("//a[@href='#draft']")).Click();
            WebElementVisible(By.XPath("//button[@aria-describedby='tooltip-0-2']")).Click();
            Assert.IsTrue(WebElementClickable(By.XPath("//*[contains(text(),'dragnir')]")).Displayed);

            WebElementVisible(By.XPath("//a[@href='#sent']")).Click();
            Assert.IsTrue(WebElementVisible(By.XPath(dratedMailPath)).Displayed);

            WebElementVisible(By.CssSelector("[class = 'user-account__name']")).Click();
            WebElementClickable(By.XPath(@"//a[@data-count='{""name"":""user-menu"",""id"":""exit""}']")).Click();

        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        public IWebElement WebElementVisible(By selector)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(selector));

            return driver.FindElement(selector);
        }

        public IWebElement WebElementClickable(By selector)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(selector));

            return driver.FindElement(selector);
        }

        public IWebElement WebElementExist(By selector)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));

            return driver.FindElement(selector);
        }
    }
}