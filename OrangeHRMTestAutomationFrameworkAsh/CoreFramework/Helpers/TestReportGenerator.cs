using System;
using System.IO;
using NUnit.Framework;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace OrangeHRM.Automation.Framework.Helpers
{
    public class TestReportGenerator
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private static string _reportPath;
        private static bool _isRunningInAzureDevOps;
        public static string ReportPath => _reportPath;

        public static void InitializeReport()
        {
            try
            {
                Console.WriteLine("Starting report initialization...");
                TestContext.Progress.WriteLine("Starting report initialization...");

                _isRunningInAzureDevOps = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI"));

                SetupExtentReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InitializeReport: {ex.Message}");
                TestContext.Progress.WriteLine($"Error in InitializeReport: {ex.Message}");
                throw;
            }
        }

        private static void SetupExtentReport()
        {
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine($"Base Directory: {baseDirectory}");
                TestContext.Progress.WriteLine($"Base Directory: {baseDirectory}");

                var testResultsDir = Path.Combine(baseDirectory, "TestResults");
                Directory.CreateDirectory(testResultsDir);
                Console.WriteLine($"TestResults Directory Created: {testResultsDir}");
                TestContext.Progress.WriteLine($"TestResults Directory Created: {testResultsDir}");

                _reportPath = Path.Combine(testResultsDir, $"ExtentReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                Console.WriteLine($"Report Path Set: {_reportPath}");
                TestContext.Progress.WriteLine($"Report Path Set: {_reportPath}");

                var spark = new ExtentSparkReporter(_reportPath);
                Console.WriteLine("ExtentSparkReporter created");
                TestContext.Progress.WriteLine("ExtentSparkReporter created");

                // Configure report settings
                spark.Config.Theme = Theme.Dark;
                spark.Config.DocumentTitle = "OrangeHRM Test Execution Report";
                spark.Config.ReportName = "Automation Test Results";

                _extent = new ExtentReports();
                _extent.AttachReporter(spark);
                Console.WriteLine("Reporter attached to Extent Reports");
                TestContext.Progress.WriteLine("Reporter attached to Extent Reports");

                // Add system info
                _extent.AddSystemInfo("Framework Version", ".NET 8.0");
                _extent.AddSystemInfo("Environment", GetCurrentEnvironment());
                _extent.AddSystemInfo("Browser", "Chrome");
                _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
                _extent.AddSystemInfo("Machine", Environment.MachineName);

                Console.WriteLine("ExtentReport initialization completed");
                TestContext.Progress.WriteLine("ExtentReport initialization completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetupExtentReport: {ex.Message}\nStack Trace: {ex.StackTrace}");
                TestContext.Progress.WriteLine($"Error in SetupExtentReport: {ex.Message}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private static string GetCurrentEnvironment()
        {
            return Environment.GetEnvironmentVariable("ENVIRONMENT_NAME") ??
                   Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                   "Development";
        }

        public static void StartTest(string testName, string testCaseId = null, string author = null)
        {
            _test = _extent.CreateTest(testName);

            if (!string.IsNullOrEmpty(testCaseId))
            {
                _test.AssignCategory("TestCaseId: " + testCaseId);
            }

            if (!string.IsNullOrEmpty(author))
            {
                _test.AssignAuthor(author);
            }
        }

        public static void AddTestStep(string stepName, string details, Status status)
        {
            if (_test != null)
            {
                switch (status)
                {
                    case Status.Pass:
                        _test.Log(Status.Pass, $"Step: {stepName} - {details}");
                        break;
                    case Status.Fail:
                        _test.Log(Status.Fail, $"Step: {stepName} - {details}");
                        break;
                    case Status.Warning:
                        _test.Log(Status.Warning, $"Step: {stepName} - {details}");
                        break;
                    case Status.Info:
                        _test.Log(Status.Info, $"Step: {stepName} - {details}");
                        break;
                    default:
                        _test.Log(Status.Info, $"Step: {stepName} - {details}");
                        break;
                }
            }
        }

        public static void AddTestLog(LogStatus logStatus, string message)
        {
            if (_test != null)
            {
                switch (logStatus)
                {
                    case LogStatus.Info:
                        _test.Log(Status.Info, message);
                        break;
                    case LogStatus.Warning:
                        _test.Log(Status.Warning, message);
                        break;
                    case LogStatus.Error:
                        _test.Log(Status.Fail, message);  // Changed from Error to Fail
                        break;
                    case LogStatus.Pass:
                        _test.Log(Status.Pass, message);  // Added Pass case
                        break;
                }
            }
        }

        public static void EndTest(TestStatus status, string message = "")
        {
            if (_test != null)
            {
                switch (status)
                {
                    case TestStatus.Passed:
                        _test.Pass(message);
                        TestContext.Progress.WriteLine($"Test Passed: {message}");
                        break;
                    case TestStatus.Failed:
                        _test.Fail(message);
                        TestContext.Progress.WriteLine($"Test Failed: {message}");
                        break;
                    case TestStatus.Skipped:
                        // Only mark as skipped if explicitly skipped
                        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Skipped)
                        {
                            _test.Skip(message);
                            TestContext.Progress.WriteLine($"Test Skipped: {message}");
                        }
                        else
                        {
                            _test.Pass("Test completed with no explicit result");
                            TestContext.Progress.WriteLine("Test completed with no explicit result");
                        }
                        break;
                }
            }
        }

        public static void AddScreenshot(string screenshotPath, string title = "Screenshot")
        {
            if (_test != null && File.Exists(screenshotPath))
            {
                try
                {
                    _test.AddScreenCaptureFromPath(screenshotPath, title);
                }
                catch (Exception ex)
                {
                    _test.Log(Status.Warning, $"Failed to add screenshot: {ex.Message}");
                }
            }
        }

        public static void FinalizeReport()
        {
            try
            {
                Console.WriteLine("Starting report finalization...");
                TestContext.Progress.WriteLine("Starting report finalization...");

                if (_extent != null)
                {
                    _extent.Flush();
                    Console.WriteLine("Report flushed successfully");
                    TestContext.Progress.WriteLine("Report flushed successfully");

                    if (File.Exists(_reportPath))
                    {
                        Console.WriteLine($"Report file created successfully at: {_reportPath}");
                        TestContext.Progress.WriteLine($"Report file created successfully at: {_reportPath}");
                    }
                    else
                    {
                        Console.WriteLine("Report file was not created!");
                        TestContext.Progress.WriteLine("Report file was not created!");
                    }
                }
                else
                {
                    Console.WriteLine("_extent was null during finalization!");
                    TestContext.Progress.WriteLine("_extent was null during finalization!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FinalizeReport: {ex.Message}\nStack Trace: {ex.StackTrace}");
                TestContext.Progress.WriteLine($"Error in FinalizeReport: {ex.Message}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        //internal static void EndTest(NUnit.Framework.Interfaces.TestStatus status, string message)
        //{
        //    throw new NotImplementedException();
        //}

        public enum LogStatus
        {
            Info,
            Warning,
            Error,
            Pass
        }

        public enum TestStatus
        {
            Passed,
            Failed,
            Skipped
        }
    }
}