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

        public static void WaitForPageStability(IWebDriver driver, int timeoutInSeconds = 10)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                // Wait for document ready state
                wait.Until(d => jsExecutor.ExecuteScript("return document.readyState").Equals("complete"));

                // Wait for jQuery if present
                bool jQueryExists = (bool)jsExecutor.ExecuteScript("return typeof jQuery != 'undefined'");
                if (jQueryExists)
                {
                    wait.Until(d => (bool)jsExecutor.ExecuteScript("return jQuery.active == 0"));
                }

                // Wait for any Angular if present
                bool angularExists = (bool)jsExecutor.ExecuteScript("return typeof angular != 'undefined'");
                if (angularExists)
                {
                    wait.Until(d => (bool)jsExecutor.ExecuteScript("return angular.element(document).injector().get('$http').pendingRequests.length === 0"));
                }

                // Check for any ongoing animations
                bool animationsComplete = (bool)jsExecutor.ExecuteScript(@"
                var animating = false;
                if (document.querySelector(':animated')) animating = true;
                return !animating;
                ");

                if (!animationsComplete)
                {
                    Thread.Sleep(500); // Wait for animations to complete
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Warning: Page stability check failed - {ex.Message}");
            }
        }
    }
}
