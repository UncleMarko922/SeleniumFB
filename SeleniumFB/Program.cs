using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium;

namespace SeleniumFB
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = Environment.GetEnvironmentVariable("FbUser");
            string pass = Environment.GetEnvironmentVariable("FbPass");

            const string loginButton = "loginbutton";
            const string userAvatar = "_2s25";
            const string friendsBtn = "_6-6";
            const string friendsName = "uiProfileBlockContent";
            const string friendFullName = "fsl";
            const string driverPath = "C:\\users\\marko\\onedrive\\documents\\visual studio 2015\\Projects\\SeleniumFB\\SeleniumFB\\drivers\\chrome";

           

            var friendList = new List<FriendStatus>();

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");

            IWebDriver driver = new ChromeDriver(driverPath, options);

            driver.Url = "http://facebook.com";
            
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            driver.FindElement(By.Id("email")).SendKeys(user);
            driver.FindElement(By.Id("pass")).SendKeys(pass);

            driver.FindElement(By.Id(loginButton)).Click();
            driver.FindElement(By.ClassName(userAvatar)).Click();
            string friendsUrl = driver.Url + "/friends";
            driver.Url = friendsUrl;

            Stopwatch s = new Stopwatch();
            s.Start();

            while (s.Elapsed < TimeSpan.FromSeconds(30)) { 
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            }

            s.Stop();
            IList<IWebElement> friends = driver.FindElements(By.ClassName(friendsName));
            

            for (int i = 0; i < friends.Count; i++)
            {
                string fullName = friends[i].FindElement(By.ClassName(friendFullName)).Text;
                FriendStatus friendStatus = new FriendStatus();
                friendStatus.name = fullName;
                try
                { 
                    friends[i].FindElement(By.ClassName("_39g5"));
                }
                catch(NoSuchElementException)
                {
                    friendStatus.active = false;
                }
                friendList.Add(friendStatus);
            }

            int deactivatedCounter = 0;
            foreach(FriendStatus friend in friendList)
            {
                if(friend.active != true)
                {
                    deactivatedCounter++;
                }
            }
            Console.WriteLine("Total friends: " + friendList.Count);
            Console.WriteLine("Deactivated friends: " + deactivatedCounter);
            Console.ReadKey();
        }
    }
}
