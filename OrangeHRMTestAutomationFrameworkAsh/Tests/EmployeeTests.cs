﻿using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.TestData;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base
{
    [TestFixture]
    public class EmployeeTests : BaseTest
    {
        private LoginPage _loginPage;
        private EmployeePage _employeePage;
        private EmployeeDataGenerator _dataGenerator;

        [SetUp]
        public void Setup()
        {
            _loginPage = new LoginPage(Driver);
            _dataGenerator = new EmployeeDataGenerator();
            _employeePage = _loginPage.Login(Config.AdminUsername, Config.AdminPassword)
                                    .NavigateToEmployeePage();
        }

        [Test]
        public void Should_Add_New_Employee_Successfully()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployee();

            // Act
            _employeePage.AddNewEmployee(employee.FirstName, employee.LastName, employee.EmployeeId);

            // Assert
            Assert.That(_employeePage.IsEmployeeCreated($"{employee.FirstName} {employee.LastName}"),
                Is.True, "Employee should be created successfully");
        }

        [Test]
        public void Should_Search_Employee_Successfully()
        {
            // Arrange
            var employee = _dataGenerator.GenerateEmployee();
            _employeePage.AddNewEmployee(employee.FirstName, employee.LastName, employee.EmployeeId);

            // Act
            _employeePage.SearchEmployee(employee.FirstName);

            // Assert
            Assert.That(_employeePage.IsEmployeeCreated($"{employee.FirstName} {employee.LastName}"),
                Is.True, "Employee should be found in search results");
        }
    }
}
