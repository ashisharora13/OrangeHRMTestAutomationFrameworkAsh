using OpenQA.Selenium;
using OrangeHRM.Automation.Framework.Core.Browser;
using OrangeHRM.Automation.Framework.Core.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace OrangeHRM.Automation.Framework.Core.Base
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;
        protected ConfigurationManager Config;
        protected string TestResultsPath;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            try
            {
                // Initialize configuration
                Config = new ConfigurationManager();

                // Set up test results directory
                TestResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResults");
                if (!Directory.Exists(TestResultsPath))
                {
                    Directory.CreateDirectory(TestResultsPath);
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to initialize configuration: {ex.Message}");
                throw;
            }
        }

        [SetUp]
        public void Setup()
        {
            try
            {
                InitializeDriver();
                NavigateToBaseUrl();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to setup test: {ex.Message}");
                throw;
            }
        }

        private void InitializeDriver()
        {
            var browserFactory = new BrowserFactory();
            Driver = browserFactory.CreateDriver(Config.Browser);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.DefaultTimeout);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Config.DefaultTimeout);
        }

        private void NavigateToBaseUrl()
        {
            if (string.IsNullOrEmpty(Config.BaseUrl))
            {
                throw new ArgumentException("BaseUrl is not configured in appsettings.json");
            }
            Driver.Navigate().GoToUrl(Config.BaseUrl);
        }

        [TearDown]
        public void Cleanup()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                TakeScreenshot();
            }
        }

        protected string? TakeScreenshot()
        {
            try
            {
                // Add a small delay to ensure page is fully rendered
                Thread.Sleep(500); // 500ms delay

                // Wait for page load
                var jsExecutor = (IJavaScriptExecutor)Driver;
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(driver => jsExecutor.ExecuteScript("return document.readyState").Equals("complete"));

                // Wait for jQuery if it exists
                bool jQueryExists = (bool)jsExecutor.ExecuteScript("return typeof jQuery != 'undefined'");
                if (jQueryExists)
                {
                    wait.Until(driver => (bool)jsExecutor.ExecuteScript("return jQuery.active == 0"));
                }

                // Scroll to top of page to ensure consistent screenshots
                jsExecutor.ExecuteScript("window.scrollTo(0, 0);");

                // Take the screenshot
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var fileName = $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMddHHmmss}.png";
                var screenshotDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotDir);
                var filePath = Path.Combine(screenshotDir, fileName);

                screenshot.SaveAsFile(filePath);
                TestContext.Progress.WriteLine($"Screenshot saved: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to capture screenshot: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            try
            {
                Driver?.Quit();
                Driver?.Dispose();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to dispose driver: {ex.Message}");
            }
        }

        // Helper methods
        protected void RefreshPage() => Driver.Navigate().Refresh();
        protected void NavigateBack() => Driver.Navigate().Back();
        protected void NavigateForward() => Driver.Navigate().Forward();
        protected string GetCurrentUrl() => Driver.Url;
        protected void ClearCookies() => Driver.Manage().Cookies.DeleteAllCookies();

        protected void ExecuteJavaScript(string script, params object[] args)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
        }
    }
}
