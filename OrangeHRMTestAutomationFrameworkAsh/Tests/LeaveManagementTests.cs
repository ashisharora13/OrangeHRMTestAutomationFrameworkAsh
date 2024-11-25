using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base
{
    [TestFixture]
    public class LeaveManagementTests : BaseTest
    {
        private LoginPage _loginPage;
        private LeavePage _leavePage;

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Driver);
            _leavePage = _loginPage.Login(Config.AdminUsername, Config.AdminPassword)
                                 .NavigateToLeavePage();
        }

        [Test]
        public void Should_Apply_Leave_Successfully()
        {
            // Arrange
            var fromDate = DateTime.Now.AddDays(1);
            var toDate = fromDate.AddDays(5);

            // Act
            _leavePage.ApplyLeave("Vacation", fromDate, toDate);

            // Assert
            // Add appropriate assertions based on the application's behavior
        }
    }
}
