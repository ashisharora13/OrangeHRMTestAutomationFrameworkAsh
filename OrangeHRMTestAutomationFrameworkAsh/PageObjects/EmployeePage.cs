using OpenQA.Selenium;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;
using System.Drawing.Text;
using System.Security.Cryptography.X509Certificates;


namespace OrangeHRM.Automation.Framework.PageObjects
{
    public class EmployeePage : BasePage
    {
        private readonly By _addButton = By.CssSelector("button[class='oxd-button oxd-button--medium oxd-button--secondary']");
        private readonly By _firstNameInput = By.Name("firstName");
        private readonly By _lastNameInput = By.Name("lastName");
        private readonly By _employeeId = By.CssSelector("input.oxd-input--active");
        private readonly By _saveButton = By.CssSelector("button[type='submit']");
        private readonly By _searchInput = By.XPath("//div[@class='oxd-grid-4 orangehrm-full-width-grid']//div[1]//div[1]//div[2]//div[1]//div[1]//input[1]");
        private readonly By _searchButton = By.CssSelector("button[type='submit']");
        private readonly By _empRecord = By.XPath("//div[contains(text(),'{employeeName}')]");

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
            var dashboard = new DashboardPage(Driver);
            dashboard.NavigateToEmployeePage();
            WaitAndSendKeys(_searchInput, searchTerm);
            WaitAndClick(_searchButton);
        }

        public bool IsEmployeeCreated(string employeeFirstName, string employeeLastName)
        {
            var employeeName = string.Concat(employeeFirstName," ", employeeLastName);
            return IsElementDisplayed(By.XPath($"//h6[normalize-space()='{employeeName}']"));
        }

        public void ClickOnEmpRecordCreated(string employeeName)
        {
            By _empRecord = By.XPath($"//div[contains(text(),'{employeeName}')]");
            WaitAndClick(_empRecord);
        }
    }
}

