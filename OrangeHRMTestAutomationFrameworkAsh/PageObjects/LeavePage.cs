using OpenQA.Selenium;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;


namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class LeavePage : BasePage
    {
        private readonly By _applyButton = By.CssSelector("button.oxd-button--secondary");
        private readonly By _leaveTypeDropdown = By.CssSelector(".oxd-select-text");
        private readonly By _fromDate = By.CssSelector("input[placeholder='yyyy-mm-dd']");
        private readonly By _toDate = By.CssSelector("input[placeholder='yyyy-mm-dd']");
        private readonly By _submitButton = By.CssSelector("button[type='submit']");

        public LeavePage(IWebDriver driver) : base(driver) { }

        public void ApplyLeave(string leaveType, DateTime fromDate, DateTime toDate)
        {
            WaitAndClick(_applyButton);
            WaitAndClick(_leaveTypeDropdown);
            WaitAndClick(By.XPath($"//span[text()='{leaveType}']"));

            WaitAndSendKeys(_fromDate, fromDate.ToString("yyyy-MM-dd"));
            WaitAndSendKeys(_toDate, toDate.ToString("yyyy-MM-dd"));

            WaitAndClick(_submitButton);
        }
    }
}

