using OpenQA.Selenium;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;


namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class DashboardPage : BasePage
    {
        private readonly By _welcomeMessage = By.CssSelector(".oxd-userdropdown-name");
        private readonly By _dashboardPageHeader = By.XPath("//h6[normalize-space()='Dashboard']");
        private readonly By _pimMenu = By.CssSelector("a[href*='viewPimModule']");
        private readonly By _leaveMenu = By.CssSelector("a[href*='viewLeaveModule']");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public string GetWelcomeMessage() => WaitAndGetText(_dashboardPageHeader);

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

