using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeHRM.Automation.Framework.Core.Browser;
using OrangeHRM.Automation.Framework.Core.Configuration;
using OrangeHRM.Automation.Framework.Core.Helpers;
using NUnit.Framework;
using System;
using System.IO;

namespace OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base
{

    public class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;
        protected readonly WaitHelper WaitHelper;
        protected readonly ElementHelper ElementHelper;
        protected readonly int DefaultTimeout = 10;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultTimeout));
            WaitHelper = new WaitHelper(driver, DefaultTimeout);
            ElementHelper = new ElementHelper(driver);
        }

        #region Element Interaction Methods
        protected IWebElement WaitAndFindElement(By by)
        {
            try
            {
                return WaitHelper.WaitForElement(by);
            }
            catch (WebDriverTimeoutException)
            {
                throw new ElementNotVisibleException($"Element with locator {by} was not found within {DefaultTimeout} seconds");
            }
        }

        protected IList<IWebElement> WaitAndFindElements(By by)
        {
            try
            {
                return Wait.Until(d => d.FindElements(by));
            }
            catch (WebDriverTimeoutException)
            {
                return new List<IWebElement>();
            }
        }

        protected void WaitAndClick(By by)
        {
            try
            {
                var element = WaitHelper.WaitForClickable(by);
                ElementHelper.ScrollToElement(element);
                ElementHelper.HighlightElement(element);
                element.Click();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to click element with locator {by}. Error: {ex.Message}");
            }
        }

        protected void WaitAndSendKeys(By by, string text)
        {
            try
            {
                var element = WaitAndFindElement(by);
                element.Clear();
                ElementHelper.HighlightElement(element);
                element.SendKeys(text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send keys to element with locator {by}. Error: {ex.Message}");
            }
        }

        protected string WaitAndGetText(By by)
        {
            try
            {
                return WaitAndFindElement(by).Text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get text from element with locator {by}. Error: {ex.Message}");
            }
        }

        protected string WaitAndGetAttribute(By by, string attribute)
        {
            try
            {
                return WaitAndFindElement(by).GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get attribute {attribute} from element with locator {by}. Error: {ex.Message}");
            }
        }
        #endregion

        #region Verification Methods
        protected bool IsElementDisplayed(By by, int timeoutInSeconds = 5)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(d => d.FindElement(by).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        protected bool IsElementEnabled(By by)
        {
            try
            {
                return WaitAndFindElement(by).Enabled;
            }
            catch
            {
                return false;
            }
        }

        protected bool IsElementSelected(By by)
        {
            try
            {
                return WaitAndFindElement(by).Selected;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region JavaScript Methods
        protected void ClickUsingJavaScript(By by)
        {
            var element = WaitAndFindElement(by);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }

        protected void ScrollIntoView(By by)
        {
            var element = WaitAndFindElement(by);
            ElementHelper.ScrollToElement(element);
        }

        protected string GetPageTitle()
        {
            return Driver.Title;
        }

        protected void WaitForPageLoad()
        {
            var js = (IJavaScriptExecutor)Driver;
            Wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
        #endregion

        #region Select Methods
        protected void SelectByText(By by, string text)
        {
            var element = WaitAndFindElement(by);
            var select = new OpenQA.Selenium.Support.UI.SelectElement(element);
            select.SelectByText(text);
        }

        protected void SelectByValue(By by, string value)
        {
            var element = WaitAndFindElement(by);
            var select = new OpenQA.Selenium.Support.UI.SelectElement(element);
            select.SelectByValue(value);
        }

        protected void SelectByIndex(By by, int index)
        {
            var element = WaitAndFindElement(by);
            var select = new OpenQA.Selenium.Support.UI.SelectElement(element);
            select.SelectByIndex(index);
        }
        #endregion

        #region Error Handling
        protected void RetryClick(By by, int maxAttempts = 3)
        {
            var attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    WaitAndClick(by);
                    return;
                }
                catch (Exception)
                {
                    attempts++;
                    if (attempts == maxAttempts)
                        throw;
                    Thread.Sleep(1000); // Wait 1 second between attempts
                }
            }
        }

        protected bool WaitForElementToDisappear(By by, int timeoutInSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(d => !d.FindElement(by).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }
        #endregion
    }
}
