using NUnit.Framework;
using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.TestData;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;

namespace OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage;

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Driver);
        }

        [Test]
        public void Should_Login_Successfully_With_Valid_Credentials()
        {
            // Each step will be captured with a screenshot
            CaptureStep("Navigating to login page");

                var loginPage = new LoginPage(Driver);
                loginPage.EnterUsername(Config.AdminUsername);
                CaptureStep("Entered username");

                loginPage.EnterPassword(Config.AdminPassword);
                CaptureStep("Entered password");

                loginPage.ClickLogin();
                CaptureStep("Clicked login button");

                // Assert dashboard is displayed
                Assert.AreEqual(new DashboardPage(Driver).GetWelcomeMessage(), "Dashboard");
                CaptureStep("Verified dashboard is displayed");
            }

            [Test]
        public void Should_Show_Error_Message_With_Invalid_Credentials()
        {
            // Arrange
            _loginPage.Login("invalid", "invalid");

            // Assert
            Assert.That(_loginPage.GetErrorMessage(), Contains.Substring("Invalid credentials"));
        }
    }
}
