using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRM.Automation.Framework.Core.Helpers
{
    public class WaitHelper
    {
        private readonly WebDriverWait _wait;

        public WaitHelper(IWebDriver driver, int timeoutInSeconds = 10)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public IWebElement WaitForElement(By by)
        {
            return _wait.Until(d => d.FindElement(by));
        }

        public IWebElement WaitForClickable(By by)
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
        }

        public bool WaitForVisible(By by)
        {
            try
            {
                _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
