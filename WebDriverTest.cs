using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

namespace Lesson_6_WebDriver
{
    public class WebDriverTest
    {
        
        private readonly string baseUrl = "https://mail.yandex.com";
        private IWebDriver driver;
        private string userName = "vadim.kuryan.vka";
        private string userPassword = "Vka_6463296";

        [SetUp]
        public void Setup()
        {
            string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void LoginAndSendMailTest()
        {
            driver.Navigate().GoToUrl(baseUrl);

            //Login to the mail box.
            IWebElement loginButton = driver.FindElement(By.XPath("//*[text()='Log in']/ancestor::button"));
            loginButton.Click();

            IWebElement loginName = driver.FindElement(By.CssSelector("[name = 'login']"));
            loginName.SendKeys(userName);

            IWebElement loginUser = driver.FindElement(By.CssSelector("[id = 'passp:sign-in']"));
            loginUser.Click();

            WebElementVisible(By.CssSelector("[name = 'passwd']")).SendKeys(userPassword);

            IWebElement submit = driver.FindElement(By.CssSelector("[id = 'passp:sign-in']"));
            submit.Click();

            //Assert, that the login is successful.
            Assert.AreEqual(userName, WebElementVisible(By.CssSelector("[class = 'user-account__name']")).Text);

            //Create a new mail (fill addressee, subject and body fields).
            IWebElement writeNewMail = driver.FindElement(By.CssSelector("[class = 'Button2 Button2_type_link Button2_view_action Button2_size_m Layout-m__compose--pTDsx qa-LeftColumn-ComposeButton ComposeButton-m__root--fP-o9']"));
            writeNewMail.Click();

            IWebElement mailTo = driver.FindElement(By.XPath("//*[@id='compose-field-1']"));
            mailTo.SendKeys("dragnir@tut.by");

            IWebElement mailSubject = driver.FindElement(By.XPath("//*[@id='compose-field-subject-4']"));
            mailSubject.SendKeys("TestSubject");

            IWebElement mailBody = driver.FindElement(By.XPath("//*[@id='cke_1_contents']/div"));
            mailBody.SendKeys("TestBody");

            IWebElement sendLater = driver.FindElement(By.XPath("//*[@class='Button2-Icon new__icon--1nFOX']/ancestor::button"));
            sendLater.Click();

            //Save the mail as a draft.
            IWebElement sendLaterToday = driver.FindElement(By.XPath("//div[@class='ComposeTimeOptions-Label']"));
            sendLaterToday.Click();

            IWebElement draftFolder = driver.FindElement(By.XPath("//a[@href='#draft']"));
            draftFolder.Click();

            //Verify, that the mail presents in ‘Drafts’ folder.


            Thread.Sleep(10000);

            Assert.Pass();
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
    }
}