using OpenQA.Selenium;


namespace OrangeHRM.Automation.Framework.Core.Helpers
{
    public class ElementHelper
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        public ElementHelper(IWebDriver driver)
        {
            _driver = driver;
            _js = (IJavaScriptExecutor)driver;
        }

        public void ScrollToElement(IWebElement element)
        {
            _js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public void HighlightElement(IWebElement element)
        {
            var originalStyle = element.GetAttribute("style");
            _js.ExecuteScript(
                "arguments[0].setAttribute('style', 'background: yellow; border: 2px solid red;');",
                element);

            Thread.Sleep(300);
            _js.ExecuteScript($"arguments[0].setAttribute('style', '{originalStyle}');", element);
        }

        public bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

