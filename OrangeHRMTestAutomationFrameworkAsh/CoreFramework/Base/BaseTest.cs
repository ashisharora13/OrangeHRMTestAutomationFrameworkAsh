using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OrangeHRM.Automation.Framework.Core.Browser;
using OrangeHRM.Automation.Framework.Core.Configuration;
using OrangeHRM.Automation.Framework.Core.Reporting;

namespace OrangeHRM.Automation.Framework.Core.Base
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver Driver;
        protected Configuration.ConfigurationManager Config = new Configuration.ConfigurationManager();
        protected string TestResultsPath;
        protected TestReporter TestReporter;
        protected BrowserFactory BrowserFactory;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            try
            {
                TestResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResults");
                if (!Directory.Exists(TestResultsPath))
                {
                    Directory.CreateDirectory(TestResultsPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize configuration: {ex.Message}");
                throw;
            }
        }

        [SetUp]
        public void Setup()
        {
            try
            {
                InitializeDriver();
                TestReporter = new TestReporter(TestContext.CurrentContext.Test.Name, TestResultsPath, Driver);
                NavigateToBaseUrl();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to setup test: {ex.Message}");
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

        protected void CaptureStep(string stepDescription)
        {
            TestReporter.CaptureStepScreen(stepDescription);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    CaptureStep("Test Failed - Final State");
                }

                // Generate HTML report
                TestReporter.GenerateHtmlReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed in TearDown: {ex.Message}");
            }
            finally
            {
                Dispose();
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
                Console.WriteLine($"Failed to dispose driver: {ex.Message}");
            }

            // ... (rest of your existing code remains the same)
        }
    }
}
