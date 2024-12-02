using OpenQA.Selenium;
using System.Text;

namespace OrangeHRM.Automation.Framework.Reporting
{
    public class TestReporter
    {
        private readonly List<TestStep> _steps;
        private readonly string _testName;
        private readonly string _testResultsPath;
        private readonly IWebDriver _driver;
        private int _stepCounter;

        public TestReporter(string testName, string testResultsPath, IWebDriver driver)
        {
            _steps = new List<TestStep>();
            _testName = testName;
            _testResultsPath = testResultsPath;
            _driver = driver;
            _stepCounter = 0;
        }

        public string CaptureStepScreen(string stepDescription)
        {
            _stepCounter++;
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var screenshotName = $"{_testName}_Step{_stepCounter}_{timestamp}.png";
            var screenshotPath = Path.Combine(_testResultsPath, screenshotName);

            try
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);

                _steps.Add(new TestStep
                {
                    StepNumber = _stepCounter,
                    Description = stepDescription,
                    ScreenshotPath = screenshotName,
                    Timestamp = DateTime.Now
                });

                return screenshotPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture step screenshot: {ex.Message}");
                return string.Empty;
            }
        }

        // Rest of the class implementation remains the same...
        public void GenerateHtmlReport()
        {
            var reportName = $"{_testName}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            var reportPath = Path.Combine(_testResultsPath, reportName);

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<!DOCTYPE html>");
            htmlBuilder.AppendLine("<html lang='en'>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("<meta charset='UTF-8'>");
            htmlBuilder.AppendLine("<title>Test Execution Report</title>");
            htmlBuilder.AppendLine("<style>");
            htmlBuilder.AppendLine(@"
                body { font-family: Arial, sans-serif; margin: 20px; }
                .header { background-color: #f4f4f4; padding: 20px; margin-bottom: 20px; }
                .step { margin-bottom: 30px; border: 1px solid #ddd; padding: 15px; }
                .step-header { background-color: #f8f8f8; padding: 10px; margin-bottom: 10px; }
                .screenshot { max-width: 800px; margin-top: 10px; border: 1px solid #ddd; }
                .timestamp { color: #666; font-size: 0.9em; }
            ");
            htmlBuilder.AppendLine("</style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");

            // Add test header
            htmlBuilder.AppendLine("<div class='header'>");
            htmlBuilder.AppendLine($"<h1>Test Report: {_testName}</h1>");
            htmlBuilder.AppendLine($"<p>Execution Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
            htmlBuilder.AppendLine("</div>");

            // Add steps
            foreach (var step in _steps)
            {
                htmlBuilder.AppendLine("<div class='step'>");
                htmlBuilder.AppendLine("<div class='step-header'>");
                htmlBuilder.AppendLine($"<h3>Step {step.StepNumber}: {step.Description}</h3>");
                htmlBuilder.AppendLine($"<p class='timestamp'>Timestamp: {step.Timestamp:yyyy-MM-dd HH:mm:ss}</p>");
                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine($"<img src='{step.ScreenshotPath}' alt='Step {step.StepNumber} Screenshot' class='screenshot'>");
                htmlBuilder.AppendLine("</div>");
            }

            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            File.WriteAllText(reportPath, htmlBuilder.ToString());
            Console.WriteLine($"HTML Report generated: {reportPath}");
        }
    }

    public class TestStep
    {
        public int StepNumber { get; set; }
        public string? Description { get; set; }
        public string? ScreenshotPath { get; set; }
        public DateTime Timestamp { get; set; }
    }
}