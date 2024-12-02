using AventStack.ExtentReports;
using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.TestData;
using OrangeHRM.Automation.Framework.Reporting;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base
{
    [TestFixture]
    public class EmployeeTests : TestBase
    {
        private new LoginPage _loginPage;
        private EmployeePage _employeePage;
        private EmployeeDataGenerator _dataGenerator;

        [SetUp]
        public new void Setup()
        {
            _loginPage = new LoginPage(Driver);
            _dataGenerator = new EmployeeDataGenerator();
            _employeePage = _loginPage.Login(Config.AdminUsername, Config.AdminPassword)
                                    .NavigateToEmployeePage();
        }

        [Test]
        [Category("Regression")]
        [Property("TestCaseId", "12345")]  //Azure DevOps Test Case ID
        public void Should_Add_New_Employee_Successfully()
        {
            // Arrange
            LogInfo("Preparing the test data");
            var employee = _dataGenerator.GenerateEmployee();

            // Act
            LogTestStep("Add New Employee", "Adding a new employee to OrangeHRM", Status.Info);
            _employeePage.AddNewEmployee(employee.FirstName, employee.LastName, employee.EmployeeId);

            // Assert
            //Assert.That(_employeePage.IsEmployeeCreated($"{employee.FirstName} {employee.LastName}"),
            //  Is.True, "Employee should be created successfully");
            AssertAndLog(
            () => Assert.That(_employeePage.IsEmployeeCreated(employee.FirstName,employee.LastName), Is.True),
            "Employee should be created successfully");
        }

        [Test]
        [Category("Regression")]
        [Property("TestCaseId", "12346")]  //Azure DevOps Test Case ID
        public void Should_Search_Employee_Successfully()
        {
            // Arrange
            LogInfo("Preparing the test data");
            var employee = _dataGenerator.GenerateEmployee();
            LogTestStep("Add New Employee", "Adding a new employee to OrangeHRM", Status.Info);
            _employeePage.AddNewEmployee(employee.FirstName, employee.LastName, employee.EmployeeId);

            // Act
            LogTestStep("Search Employee", "Searching a new employee within OrangeHRM", Status.Info);
            _employeePage.SearchEmployee(employee.FirstName);
            _employeePage.ClickOnEmpRecordCreated(employee.FirstName);

            // Assert
            AssertAndLog(
            () => Assert.That(_employeePage.IsEmployeeCreated(employee.FirstName,employee.LastName),
                Is.True), "Employee should be found in search results");
        }
    }
}
