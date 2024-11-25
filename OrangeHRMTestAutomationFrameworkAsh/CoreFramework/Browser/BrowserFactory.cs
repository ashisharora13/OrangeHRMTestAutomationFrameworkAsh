using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace OrangeHRM.Automation.Framework.Core.Browser
{
    public class BrowserFactory : IBrowserFactory
    {

            public IWebDriver CreateDriver(string browserType)
            {
                return browserType.ToLower() switch
                {
                    "chrome" => new ChromeDriver(),
                    "firefox" => new FirefoxDriver(),
                    "edge" => new EdgeDriver(),
                    _ => throw new ArgumentException($"Browser type '{browserType}' is not supported.")
                };
            }

        private IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            return new ChromeDriver(options);
        }

        private IWebDriver CreateFirefoxDriver()
        {
            var options = new FirefoxOptions();
            options.AddArgument("--start-maximized");
            return new FirefoxDriver(options);
        }

        private IWebDriver CreateEdgeDriver()
        {
            var options = new EdgeOptions();
            options.AddArgument("--start-maximized");
            return new EdgeDriver(options);
        }
    }

    public interface IBrowserFactory
    {
        IWebDriver CreateDriver(string browserType);
    }
}
