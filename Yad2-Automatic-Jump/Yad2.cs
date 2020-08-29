using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yad2_Automatic_Jump
{
    public class Yad2 : IDisposable
    {
        IEnumerable<string> orderIds;
        string username;
        string password;

        IWebDriver driver { get; set; }        
        public string ErrorMessahe { get; set; }
        

        public Yad2()
        {
            driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            driver.Url = "https://my.yad2.co.il/login.php/";

            Init();
        }

        public void Start(IEnumerable<string> OrderIds)
        {
            LoginPage();
            

            foreach (var orderId in OrderIds)
            {
                MyOrderPage();
                MyPersonalAreaPage(orderId);
            }            
        }

        private void Init()
        {
            try
            {
                username = ConfigurationManager.AppSettings.Get("Username");
                password = ConfigurationManager.AppSettings.Get("Password");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Configuration in appSettings is empty.");
                throw ex;
            }            
        }

        private void LoginPage()
        {
            //driver.FindElement(By.XPath("//*[@class='icon y2i_user']/parent::*")).Click();

            IsLoginPage();

            var userNameElement = driver.FindElement(By.XPath("//*[@name='UserName']"));
            var passwordElement = driver.FindElement(By.XPath("//*[@name='password']"));
            var sendButtonElement = driver.FindElement(By.XPath("//*[@id='submitLogonForm']"));

            userNameElement.SendKeys(username);
            passwordElement.SendKeys(password);
            sendButtonElement.Click();
        }

        public void MyOrderPage()
        {
            driver.Url = $"https://my.yad2.co.il/newOrder/index.php?action=personalAreaIndex";
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));                                                                         
            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='https://my.yad2.co.il/newOrder/index.php?action=personalAreaFeed&CatID=3&SubCatID=0']")));

            var nadlanLink = driver.FindElement(By.XPath("//a[@href='https://my.yad2.co.il/newOrder/index.php?action=personalAreaFeed&CatID=3&SubCatID=0']"));
            nadlanLink.Click();
        }

        public void MyPersonalAreaPage(string orderId)
        {
            var orderIdNameXPath = string.Format("//tr[@data-orderid='{0}']", orderId);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.FindElement(By.XPath(orderIdNameXPath)));
            Console.WriteLine("1");
            var rowElement = driver.FindElement(By.XPath(orderIdNameXPath));
            var tdElement = rowElement.FindElements(By.XPath($"{orderIdNameXPath}//td"));
            tdElement[2].Click();
            

            var iframeElement = driver.FindElement(By.XPath("//iframe[starts-with(@src,'//my.yad2.co.il/newOrder/index.php?action=personalAreaViewDetails')]"));
            var iframeSrc = iframeElement.GetAttribute("src");
            driver.Url = iframeSrc;
            Console.WriteLine(iframeSrc);
            //driver.SwitchTo().Frame(iframeElement);

            //Get Iframe url  "//my.yad2.co.il/newOrder/index.php?action=personalAreaViewDetails&CatID=2&SubCatID=2&OrderID=39234209"
            // open new url driver
            // click on jump button
            Console.WriteLine("3");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.FindElement(By.Id("bounceRatingOrderBtn")));
            Console.WriteLine("4");
            var jumpButton = driver.FindElement(By.Id("bounceRatingOrderBtn"));
            Console.WriteLine("5");
            jumpButton.Click();
            Console.WriteLine("finish");
        }

        private bool IsLoginPage()
        {            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.FindElement(By.XPath("//*[@name='UserName']")).Displayed);

            var isLoginPageDisplayed = driver.FindElement(By.XPath("//*[@name='UserName']")).Displayed;

            if (!isLoginPageDisplayed)
            {
                throw new Exception("Login page not found! Please try again later.");
            }

            return isLoginPageDisplayed;
        }

        public void Dispose()
        {
            driver.Close();
            driver.Dispose();
        }
    }
}
