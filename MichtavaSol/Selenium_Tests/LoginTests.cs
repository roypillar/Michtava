using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Common;

namespace Frontend_Tests
{

    [TestClass]
    public class LoginTests
    {

        [TestMethod]
        public void testTeacherLogin()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(GlobalConstants.testingUrlRoot);
            IWebElement userNameInput = driver.FindElement(By.Id("loginTB"));
            userNameInput.SendKeys("hongster");
            IWebElement passwordInput = driver.FindElement(By.Id("passwordTB"));
            passwordInput.SendKeys("111");
            passwordInput.SendKeys(Keys.Enter);

            //Assert.AreEqual(פה צריך לוודא שהוא באמת התחבר, למשל לשאול האם האלמנט שכותב "שלום <שם משתמש>" כותב את השם משתמש באמת.)


        }
    }
}
