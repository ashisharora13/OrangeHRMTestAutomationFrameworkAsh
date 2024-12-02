﻿using AventStack.ExtentReports;
using OrangeHRM.Automation.Framework.Core.Models;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.Reporting;

namespace OrangeHRM.Automation.Framework.Core.Base
{
    public class TestBase : BaseTest
    {
        protected DashboardPage? _dashboardPage;
        protected LoginPage _loginPage;
        protected static AzureDevOpsTestReporter _azureDevOpsReporter;
        private DateTime _testStartTime;
        private TestReportGenerator.TestStatus reportStatus;
        private string? message;

        [OneTimeSetUp]
        public async Task TestSuiteSetup()
        {
            try
            {
                TestReportGenerator.InitializeReport();

                // Validate Azure DevOps settings
                ValidateAzureDevOpsSettings();

                // Initialize Azure DevOps reporter
                _azureDevOpsReporter = new AzureDevOpsTestReporter(
                    Config.Url,
                    Config.PersonalAccessToken,
                    Config.ProjectName
                );

                //Create a test run

                await _azureDevOpsReporter.CreateTestRun(GetType().Name);

                TestContext.Progress.WriteLine("Test suite setup completed");
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to setup test suite: {ex.Message}");
                TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private void ValidateAzureDevOpsSettings()
        {
            if (string.IsNullOrEmpty(Config.Url))
                throw new InvalidOperationException("Azure DevOps URL is not configured in appsettings.json");

            if (string.IsNullOrEmpty(Config.PersonalAccessToken))
                throw new InvalidOperationException("Personal Access Token is not configured in appsettings.json");

            if (string.IsNullOrEmpty(Config.ProjectName))
                throw new InvalidOperationException("Project Name is not configured in appsettings.json");

            TestContext.Progress.WriteLine("Azure DevOps settings validated successfully");
        }


        [SetUp]
        public void TestSetup()
        {
            try
            {
                _testStartTime = DateTime.Now;
                var testName = TestContext.CurrentContext.Test.Name;
                var testCaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId")?.ToString();
                var author = TestContext.CurrentContext.Test.Properties.Get("Author")?.ToString();

                TestReportGenerator.StartTest(testName, testCaseId, author);

                _loginPage = new LoginPage(Driver);
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed in test setup: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public async Task TestCleanup()
        {
            try
            {
                var testResult = TestContext.CurrentContext.Result;
                var testCaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId")?.ToString();
                var endTime = DateTime.Now;
                //var message = string.Empty;
                //var reportStatus = OrangeHRM.Automation.Framework.Helpers.TestReportGenerator.TestStatus.Failed; // Default to failed

                // Create test result
                var result = new TestResult
                {
                      Outcome = testResult.Outcome.Status.ToString(),
                      Message = testResult.Message,
                      StartTime = _testStartTime,
                      EndTime = endTime,
                      Duration = (endTime - _testStartTime).TotalMilliseconds
                };

                // Update Azure DevOps
                if (!string.IsNullOrEmpty(testCaseId))
                {
                    await _azureDevOpsReporter.UpdateTestCase(
                        testCaseId,
                        testResult.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed,
                        testResult.Message
                    );

                    await _azureDevOpsReporter.AddTestResult(
                        testCaseId,
                        result,
                        TestContext.CurrentContext.TestDirectory
                    );
                }

                if (testResult.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
                {
                    reportStatus = TestReportGenerator.TestStatus.Passed;
                    message = "Test executed successfully";
                }
                else if (testResult.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    reportStatus = TestReportGenerator.TestStatus.Failed;
                    message = $"Test failed: {testResult.Message}\n";
                    if (testResult.StackTrace != null)
                    {
                        message += $"Stack trace: {testResult.StackTrace}";
                    }
                    var finalScreenshot = TakeScreenshot();
                    if (!string.IsNullOrEmpty(finalScreenshot))
                    {
                        TestReportGenerator.AddScreenshot(finalScreenshot, "Final State on Failure");
                    }
                }
                else
                {
                    reportStatus = TestReportGenerator.TestStatus.Skipped;
                    message = $"Test was skipped: {testResult.Message}";
                }

                TestReportGenerator.EndTest(reportStatus, message);

                // Rest of the cleanup code remains the same...
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed in test cleanup: {ex.Message}");
                throw;
            }
            finally
            {
                // Ensure browser is closed even if there's an error
                try
                {
                    Driver?.Quit();
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"Failed to quit driver: {ex.Message}");
                }
            }
        }

        [OneTimeTearDown]
        public void TestSuiteTearDown()
        {
            TestContext.Progress.WriteLine("Starting TestSuiteTearDown...");
            try
            {
                TestReportGenerator.FinalizeReport();
                TestContext.Progress.WriteLine("Report finalization completed");

                if (File.Exists(TestReportGenerator.ReportPath))
                {
                    TestContext.Progress.WriteLine($"Report file exists at: {TestReportGenerator.ReportPath}");

                    // Optionally open the report
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = TestReportGenerator.ReportPath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        TestContext.Progress.WriteLine($"Failed to open report: {ex.Message}");
                    }
                }
                else
                {
                    TestContext.Progress.WriteLine("Report file was not found!");
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error in TestSuiteTearDown: {ex.Message}");
                throw;
            }
        }

        protected string GetScreenshotPath()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var screenshotDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots");
            Directory.CreateDirectory(screenshotDir);
            return Path.Combine(screenshotDir, $"{testName}_{DateTime.Now:yyyyMMddHHmmss}.png");
        }

        protected void LogTestStep(string stepName, string details, Status status, bool takeScreenshot = true)
        {
            try
            {
                TestReportGenerator.AddTestStep(stepName, details, status);

                if (takeScreenshot)
                {
                    // Wait for animations to complete
                    Thread.Sleep(1000); // 1 second delay before screenshot

                    var screenshot = TakeScreenshot();
                    if (!string.IsNullOrEmpty(screenshot))
                    {
                        TestReportGenerator.AddScreenshot(screenshot, $"Screenshot - {stepName}");
                    }
                    else
                    {
                        TestContext.Progress.WriteLine($"Failed to capture screenshot for step: {stepName}");
                    }
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to log test step: {ex.Message}");
            }
        }

        protected void LogTestStep(string stepName, string details, Status status)
        {
            LogTestStep(stepName, details, status, true);
        }

        protected void LogTestStep(string stepName, Status status)
        {
            LogTestStep(stepName, string.Empty, status, true);
        }

        protected void LogInfo(string message, bool takeScreenshot = false)
        {
            LogTestStep("Info", message, Status.Info, takeScreenshot);
        }

        protected void LogPass(string message, bool takeScreenshot = true)
        {
            LogTestStep("Pass", message, Status.Pass, takeScreenshot);
        }

        protected void LogFail(string message, bool takeScreenshot = true)
        {
            LogTestStep("Fail", message, Status.Fail, takeScreenshot);
        }

        protected void LogWarning(string message, bool takeScreenshot = false)
        {
            LogTestStep("Warning", message, Status.Warning, takeScreenshot);
        }

        // Helper method for assertions with screenshots
        protected void AssertAndLog(Action assertion, string message, bool takeScreenshot = true)
        {
            try
            {
                assertion.Invoke();
                LogPass(message, takeScreenshot);
            }
            catch (Exception ex)
            {
                LogFail($"{message} - Failed: {ex.Message}", takeScreenshot);
                throw;
            }
        }
    }
}