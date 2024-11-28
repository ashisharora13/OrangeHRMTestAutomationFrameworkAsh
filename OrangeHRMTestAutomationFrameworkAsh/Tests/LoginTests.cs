using NUnit.Framework;
using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.Helpers;
using AventStack.ExtentReports;
using static OrangeHRM.Automation.Framework.Helpers.TestReportGenerator;

namespace OrangeHRM.Automation.Framework.Tests
{
    [TestFixture]
    [Author("Ashish Arora")]
    public class LoginTests : TestBase
    {
        private new LoginPage _loginPage;
        private new DashboardPage? _dashboardPage;

        [SetUp]
        public new void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
        }

        [Test]
        [Category("Smoke")]
        [Category("Regression")]
        [Description("Verify user can successfully login with valid credentials")]
        [Author("Ashish Arora")]
        [Property("TestCaseId", "TC_LOGIN_001")]
        public void Should_Login_Successfully_With_Valid_Credentials()
        {
            try
            {
                // Arrange
                LogInfo("Setting up test data");
                //TestReportGenerator.AddTestStep("Arrange", "Setting up test data and preconditions", Status.Info);
                var username = Config.AdminUsername;
                var password = Config.AdminPassword;
                //TestReportGenerator.AddTestLog(LogStatus.Info, $"Using username: {username}");

                // Act
                //TestReportGenerator.AddTestStep("Act", "Performing login action", Status.Info);
                LogTestStep("Login", "Attempting to login with valid credentials", Status.Info);
                _dashboardPage = _loginPage.Login(username, password);

                // Assert
                AssertAndLog(() => Assert.That(_dashboardPage.IsUserLoggedIn(), Is.True), "User is logged in Successfully");
                //LogTestStep("Verify", "Checking if login was successful", Status.Info);
                //var welcomeMessage = _dashboardPage.GetWelcomeMessage();

                //Assert.Multiple(() =>
                //{
                //    Assert.That(welcomeMessage, Is.Not.Empty, "Welcome message should not be empty");
                //    Assert.That(_dashboardPage.IsUserLoggedIn(), Is.True, "User should be logged in");
                //});

                // Add final success screenshot
                //LogTestStep("Complete", "Login successful", Status.Pass);
            }

            catch (Exception ex)
            {
                //Take error screenshot
                LogFail($"Test failed: {ex.Message}");
                throw;
            }
        }

        [Test]
        [Category("Regression")]
        [Description("Verify system shows error message for invalid credentials")]
        [Author("YourName")]
        [Property("TestCaseId", "TC_LOGIN_002")]
        public void Should_Show_Error_Message_With_Invalid_Credentials()
        {
            try
            {
                // Arrange
                //TestReportGenerator.AddTestStep("Arrange", "Setting up test data", Status.Info);
                LogInfo("Setting up test data");
                const string invalidUsername = "invalid_user";
                const string invalidPassword = "invalid_pass";
                //TestReportGenerator.AddTestLog(LogStatus.Info, "Using invalid credentials for negative test");

                // Take screenshot before login
                //var beforeLoginShot = TakeScreenshot();
                //TestReportGenerator.AddScreenshot(beforeLoginShot, "Before Login");
                LogTestStep("Login", "Attempting to login with invalid credentials", Status.Info);

                // Act
                //TestReportGenerator.AddTestStep("Act", "Attempting login with invalid credentials", Status.Info);
                _loginPage.Login(invalidUsername, invalidPassword);

                // Add screenshot after invalid login attempt
                //var loginScreenshot = TakeScreenshot();
                //TestReportGenerator.AddScreenshot(loginScreenshot, "After Invalid Login Attempt");

                // Assert
                //TestReportGenerator.AddTestStep("Assert", "Verifying error message", Status.Info);
                var errorMessage = _loginPage.GetErrorMessage();
                AssertAndLog(
                    () => Assert.That(errorMessage, Contains.Substring("Invalid credentials")),
                        "Error message should indicate invalid credentials");

                //Assert.Multiple(() =>
                //{
                //    Assert.That(errorMessage, Is.Not.Empty, "Error message should not be empty");
                //    Assert.That(errorMessage, Contains.Substring("Invalid credentials"),
                //        "Error message should indicate invalid credentials");
                //    Assert.That(_loginPage.IsDisplayed(), Is.True,
                //        "Login page should still be displayed after failed login");
                //});

                //TestReportGenerator.AddTestStep("Verification", "Error message verified successfully", Status.Pass);
            }
            catch (Exception ex)
            {
                //TestReportGenerator.AddTestStep("Error", $"Test failed: {ex.Message}", Status.Fail);
                //var errorScreenshot = TakeScreenshot();
                //TestReportGenerator.AddScreenshot(errorScreenshot, "Error State");
                //Take error screenshot
                LogFail($"Test failed: {ex.Message}");
                throw;
            }
        }

        

        [TearDown]
        public new void TestCleanup()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                //var finalScreenshot = TakeScreenshot();
                //TestReportGenerator.AddScreenshot(finalScreenshot, "Final State on Failure");
                LogTestStep("Final State", "Final State on Failure", Status.Info, true);
            }

            // Logout if logged in
            if (_dashboardPage != null && _dashboardPage.IsUserLoggedIn())
            {
                //TestReportGenerator.AddTestLog(LogStatus.Info, "Performing cleanup - Logging out");
                LogInfo("Performing cleanup - Logging out");
                _dashboardPage.Logout();
                LogInfo("GetScreenshot after logout", true);
            }
        }
    }
}