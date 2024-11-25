using OpenQA.Selenium;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;


namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class EmployeePage : BasePage
    {
        private readonly By _addButton = By.CssSelector("button.oxd-button--secondary");
        private readonly By _firstNameInput = By.Name("firstName");
        private readonly By _lastNameInput = By.Name("lastName");
        private readonly By _employeeId = By.CssSelector("input.oxd-input--active");
        private readonly By _saveButton = By.CssSelector("button[type='submit']");
        private readonly By _searchInput = By.CssSelector("input.oxd-input--active");
        private readonly By _searchButton = By.CssSelector("button[type='submit']");

        public EmployeePage(IWebDriver driver) : base(driver) { }

        public void AddNewEmployee(string firstName, string lastName, string employeeId = null)
        {
            WaitAndClick(_addButton);
            WaitAndSendKeys(_firstNameInput, firstName);
            WaitAndSendKeys(_lastNameInput, lastName);

            if (!string.IsNullOrEmpty(employeeId))
            {
                WaitAndSendKeys(_employeeId, employeeId);
            }

            WaitAndClick(_saveButton);
        }

        public void SearchEmployee(string searchTerm)
        {
            WaitAndSendKeys(_searchInput, searchTerm);
            WaitAndClick(_searchButton);
        }

        public bool IsEmployeeCreated(string employeeName)
        {
            return IsElementDisplayed(By.XPath($"//div[contains(text(),'{employeeName}')]"));
        }
    }
}

