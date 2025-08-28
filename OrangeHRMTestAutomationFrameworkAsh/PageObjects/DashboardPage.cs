using OpenQA.Selenium;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;


namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class DashboardPage : BasePage
    {
        private readonly By _pimMenu = By.CssSelector("a[href*='viewPimModule']");
        private readonly By _leaveMenu = By.CssSelector("a[href*='viewLeaveModule']");
        private readonly By _userDropdown = By.ClassName("oxd-userdropdown-tab");
        private readonly By _dropdownMenu = By.ClassName("oxd-dropdown-menu");
        private readonly By _logoutLink = By.LinkText("Logout");
        private readonly By _welcomeMessage = By.ClassName("oxd-userdropdown-name");
        private readonly By _dashboardHeader = By.XPath("//h6[text()='Dashboard']");
        private readonly By _mainMenu = By.ClassName("oxd-main-menu");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public string GetWelcomeMessage() => WaitAndGetText(_welcomeMessage);

        public bool IsUserLoggedIn()
        {
            try
            {
                return IsElementDisplayed(_userDropdown) &&
                       IsElementDisplayed(_dashboardHeader) &&
                       IsElementDisplayed(_mainMenu);
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error checking login status: {ex.Message}");
                return false;
            }
        }

        public void Logout()
        {
            try
            {
                // Click on the user dropdown
                WaitAndClick(_userDropdown);

                // Wait for dropdown menu to appear and click logout
                WaitHelper.WaitForVisible(_dropdownMenu);
                WaitAndClick(_logoutLink);

                // Wait for logout to complete (can add additional verification if needed)
                WaitHelper.WaitForVisible(_mainMenu);
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error during logout: {ex.Message}");
                throw new Exception("Failed to logout", ex);
            }
        }


        public EmployeePage NavigateToEmployeePage()
        {
            WaitAndClick(_pimMenu);
            return new EmployeePage(Driver);
        }

        public LeavePage NavigateToLeavePage()
        {
            WaitAndClick(_leaveMenu);
            return new LeavePage(Driver);
        }
    }
}

