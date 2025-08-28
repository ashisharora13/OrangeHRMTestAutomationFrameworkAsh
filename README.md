# OrangeHRM Automation Framework Documentation

## Table of Contents
1. [Introduction](#introduction)
2. [Framework Architecture](#framework-architecture)
3. [Best Practices](#best-practices)
4. [Writing Tests](#writing-tests)
5. [Do's and Don'ts](#dos-and-donts)
6. [CI/CD Pipeline](#cicd-pipeline)

## Introduction

The OrangeHRM Automation Framework is a robust, scalable test automation solution built with C#, Selenium WebDriver, and NUnit. This framework is designed to support automated testing of the OrangeHRM application across multiple environments and browsers.

### Key Features and Capabilities

- **Cross-Browser Testing**: Support for Chrome, Edge, and Firefox browsers
- **Environment Flexibility**: Run tests locally or in cloud/pipeline environments
- **Headless Mode**: Support for both headless and headed test execution
- **Embedded Drivers**: WebDriver executables embedded as resources for offline execution
- **Page Object Model**: Structured approach to element management and test design
- **Robust Reporting**: ExtentReports integration with detailed test steps and screenshots
- **Azure DevOps Integration**: Full integration with Azure Test Plans and pipelines
- **Parallel Execution**: Support for concurrent test execution
- **Configurable Settings**: Environment-specific settings via configuration files
- **Screenshot Capture**: Automatic screenshot capture for failures and test steps
- **Wait Strategies**: Intelligent element interaction with explicit waits
- **Retry Mechanisms**: Handling of intermittent issues with retry logic

## Framework Architecture

The framework follows a modular, layered architecture that promotes code reuse, maintainability, and separation of concerns.

### Core Components

```
OrangeHRM.Automation.Framework/
├── Core/
│   ├── Base/
│   │   ├── BaseTest.cs        # Base test infrastructure
│   │   ├── BasePage.cs        # Base page object functionality
│   │   └── TestBase.cs        # Test execution and reporting
│   ├── Browser/
│   │   ├── BrowserFactory.cs  # Browser instantiation
│   │   ├── IBrowserFactory.cs # Browser factory interface
│   │   ├── BrowserType.cs     # Supported browsers enum
│   │   └── DriverManager.cs   # Driver resource management
│   ├── Configuration/
│   │   ├── ConfigurationManager.cs  # Settings management
│   │   └── AppSettings.cs     # Configuration models
│   ├── Helpers/
│   │   ├── WaitHelper.cs      # Wait utilities
│   │   └── ElementHelper.cs   # Element interaction helpers
│   └── Models/
│       └── TestResult.cs      # Test result data model
├── PageObjects/               # Application page models
├── Reporting/
│   ├── TestReportGenerator.cs # ExtentReports integration
│   └── AzureDevOpsReporter.cs # Azure Test Plans integration
├── Resources/                 # Embedded resources
└── Tests/                     # Test implementations
```

### Flow Diagram

```
           ┌───────────────┐
           │ Configuration │
           └───────┬───────┘
                   │
           ┌───────▼───────┐
           │  BaseTest     │
           └───────┬───────┘
                   │
           ┌───────▼───────┐         ┌───────────────┐
           │   TestBase    │─────────► TestReporting │
           └───────┬───────┘         └───────────────┘
                   │
          ┌────────▼────────┐
          │    Test Class   │
          └────────┬────────┘
                   │
        ┌──────────▼──────────┐
        │    Page Objects     │
        └──────────┬──────────┘
                   │
        ┌──────────▼──────────┐
        │ Browser Interactions │
        └─────────────────────┘
```

## Best Practices

### Test Design

1. **Independent Tests**: Each test should be able to run independently without relying on other tests.

2. **Clear Purpose**: Each test should validate a single feature or functionality.

3. **Descriptive Names**: Test names should clearly indicate what they're testing.
   ```csharp
   [Test]
   public void Should_Login_Successfully_With_Valid_Credentials()
   ```

4. **Appropriate Categories**: Use test categories to organize tests.
   ```csharp
   [Category("Smoke")]
   [Category("Regression")]
   ```

5. **Set Up and Tear Down**: Use appropriate NUnit attributes for setup and cleanup.
   ```csharp
   [SetUp]
   public void TestSetup() { /* ... */ }
   
   [TearDown]
   public void TestCleanup() { /* ... */ }
   ```

### Page Objects

1. **Single Responsibility**: Each page object should represent a single page or component.

2. **Encapsulation**: Keep locators private and expose methods for actions.
   ```csharp
   private readonly By _usernameInput = By.Name("username");
   
   public void EnterUsername(string username) {
       WaitAndSendKeys(_usernameInput, username);
   }
   ```

3. **Method Naming**: Methods should represent user actions or state verification.
   ```csharp
   public void ClickSaveButton() { /* ... */ }
   public bool IsErrorMessageDisplayed() { /* ... */ }
   ```

4. **Return Types**: Page navigation methods should return the resulting page.
   ```csharp
   public DashboardPage ClickLogin() {
       WaitAndClick(_loginButton);
       return new DashboardPage(Driver);
   }
   ```

### Element Interaction

1. **Always Use Waits**: Never interact with elements without proper waiting.

2. **Handle Dynamic Elements**: Use robust locators for elements that change.
   ```csharp
   private readonly By _dynamicButton = By.XPath("//button[@id='saveBtn'] | //input[@value='Save']");
   ```

3. **Scroll Into View**: Ensure elements are visible before interaction.
   ```csharp
   protected void ScrollIntoView(IWebElement element) {
       ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
   }
   ```

### Error Handling

1. **Meaningful Exceptions**: Provide context in exception messages.

2. **Screenshot on Failure**: Always capture visual evidence of failures.

3. **Retry Flaky Operations**: Use retry logic for operations prone to intermittent failures.
   ```csharp
   protected void RetryAction(Action action, int maxAttempts = 3) {
       Exception lastException = null;
       for (int attempt = 1; attempt <= maxAttempts; attempt++) {
           try {
               action();
               return;
           } catch (Exception ex) {
               lastException = ex;
               if (attempt < maxAttempts) Thread.Sleep(1000);
           }
       }
       throw lastException;
   }
   ```

## Writing Tests

### Sample Test Case

Here's a complete example of a test that logs in and verifies dashboard access:

```csharp
using NUnit.Framework;
using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.Reporting;

namespace OrangeHRM.Automation.Framework.Tests
{
    [TestFixture]
    [Author("Your Name")]
    public class LoginTests : TestBase
    {
        [Test]
        [Category("Smoke")]
        [Property("TestCaseId", "TC_LOGIN_001")]
        [Description("Verify user can login with valid credentials")]
        public void Should_Login_Successfully_With_Valid_Credentials()
        {
            try
            {
                // Arrange - Prepare test data
                TestReportGenerator.AddTestStep("Arrange", "Setting up test data", Status.Info);
                var username = Config.AdminUsername;
                var password = Config.AdminPassword;

                // Act - Perform login
                TestReportGenerator.AddTestStep("Act", "Performing login action", Status.Info);
                var dashboardPage = _loginPage.Login(username, password);
                
                // Take screenshot after login
                var screenshot = TakeScreenshot();
                TestReportGenerator.AddScreenshot(screenshot, "After Login");

                // Assert - Verify successful login
                TestReportGenerator.AddTestStep("Assert", "Verifying login success", Status.Info);
                
                Assert.Multiple(() =>
                {
                    Assert.That(dashboardPage.IsUserLoggedIn(), Is.True, "User should be logged in");
                    Assert.That(dashboardPage.GetWelcomeMessage(), Contains.Substring("Welcome"), 
                        "Welcome message should be displayed");
                });

                TestReportGenerator.AddTestStep("Verification", "Login successful", Status.Pass);
            }
            catch (Exception ex)
            {
                TestReportGenerator.AddTestStep("Error", $"Test failed: {ex.Message}", Status.Fail);
                var errorScreenshot = TakeScreenshot();
                TestReportGenerator.AddScreenshot(errorScreenshot, "Error State");
                throw;
            }
        }
    }
}
```

### Page Object Implementation

Here's how the corresponding page objects might be implemented:

```csharp
public class LoginPage : BasePage
{
    // Locators
    private readonly By _usernameInput = By.Name("username");
    private readonly By _passwordInput = By.Name("password");
    private readonly By _loginButton = By.CssSelector("button[type='submit']");
    private readonly By _errorMessage = By.CssSelector(".alert-content-text");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void EnterUsername(string username)
    {
        WaitAndSendKeys(_usernameInput, username);
    }

    public void EnterPassword(string password)
    {
        WaitAndSendKeys(_passwordInput, password);
    }

    public DashboardPage ClickLoginButton()
    {
        WaitAndClick(_loginButton);
        return new DashboardPage(Driver);
    }

    public DashboardPage Login(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        return ClickLoginButton();
    }

    public string GetErrorMessage()
    {
        return WaitAndGetText(_errorMessage);
    }

    public bool IsErrorMessageDisplayed()
    {
        return IsElementDisplayed(_errorMessage);
    }
}

public class DashboardPage : BasePage
{
    // Locators
    private readonly By _welcomeMessage = By.CssSelector(".oxd-userdropdown-name");
    private readonly By _userMenu = By.CssSelector(".oxd-userdropdown-tab");
    private readonly By _logoutOption = By.XPath("//a[text()='Logout']");

    public DashboardPage(IWebDriver driver) : base(driver) { }

    public string GetWelcomeMessage()
    {
        return WaitAndGetText(_welcomeMessage);
    }

    public bool IsUserLoggedIn()
    {
        return IsElementDisplayed(_welcomeMessage);
    }

    public void ClickUserMenu()
    {
        WaitAndClick(_userMenu);
    }

    public LoginPage Logout()
    {
        ClickUserMenu();
        WaitAndClick(_logoutOption);
        return new LoginPage(Driver);
    }
}
```

## Do's and Don'ts

### Do's

1. ✅ **DO** use descriptive test and method names that explain what's being tested
2. ✅ **DO** follow the Page Object Model pattern consistently
3. ✅ **DO** add appropriate waits before interacting with elements
4. ✅ **DO** use explicit assertions with meaningful messages
5. ✅ **DO** capture screenshots for failures and important steps
6. ✅ **DO** categorize tests appropriately
7. ✅ **DO** handle dynamic elements with robust locators
8. ✅ **DO** clean up resources in TearDown methods
9. ✅ **DO** log details of each test step for traceability
10. ✅ **DO** parameterize tests for different environments

### Don'ts

1. ❌ **DON'T** create dependencies between tests
2. ❌ **DON'T** use Thread.Sleep() instead of explicit waits
3. ❌ **DON'T** hardcode test data or credentials
4. ❌ **DON'T** use absolute XPaths when possible
5. ❌ **DON'T** put assertions in page objects
6. ❌ **DON'T** create tests that are too broad in scope
7. ❌ **DON'T** access page elements directly from test classes
8. ❌ **DON'T** leave failed tests without investigation
9. ❌ **DON'T** use locators that are likely to change frequently
10. ❌ **DON'T** ignore flaky tests - address the root cause

## CI/CD Pipeline

The framework integrates with Azure DevOps CI/CD pipelines for automated test execution across environments.

### CI Pipeline

```yaml
trigger:
  branches:
    include:
    - main
    - develop

pool:
  vmImage: 'windows-latest'

variables:
  solution: '**/*.sln'
  buildPlatform: 'Any CPU'
  buildConfiguration: 'Release'

stages:
- stage: Build
  displayName: 'Build Solution'
  jobs:
  - job: Build
    steps:
    - task: UseDotNet@2
      inputs:
        version: '8.0.x'
        
    - task: NuGetCommand@2
      inputs:
        restoreSolution: '$(solution)'
        
    - task: VSBuild@1
      inputs:
        solution: '$(solution)'
        configuration: '$(buildConfiguration)'
        
    - task: PublishBuildArtifacts@1
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'drop'
```

### CD Pipeline

```yaml
resources:
  pipelines:
    - pipeline: CI-Pipeline
      source: YourCIPipelineName
      trigger: 
        branches:
          include:
            - main
            - develop

variables:
  - name: dotnetVersion
    value: '8.0.x'

stages:
- stage: TestDev
  displayName: 'Run Tests in Dev'
  variables:
    - group: Dev-Environment
  jobs:
  - deployment: RunDevTests
    environment: 'Dev'
    strategy:
      runOnce:
        deploy:
          steps:
          - download: CI-Pipeline
            artifact: 'drop'
            
          - task: UseDotNet@2
            inputs:
              version: $(dotnetVersion)
              
          - task: VSTest@2
            inputs:
              testSelector: 'testAssemblies'
              testAssemblyVer2: '**\*Tests.dll'
              searchFolder: '$(Pipeline.Workspace)/CI-Pipeline/drop'
              testFiltercriteria: 'TestCategory=Smoke'
              overrideTestrunParameters: |
                -BaseUrl "$(BaseUrl)"
                -IsHeadless "$(IsHeadless)"
                -Environment "Dev"

- stage: PublishResults
  displayName: 'Publish Test Results'
  condition: always()
  jobs:
  - job: PublishResults
    steps:
    - task: PublishTestResults@2
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '**/*.trx'
        
    - task: PublishHtmlReport@1
      inputs:
        reportDir: '$(Pipeline.Workspace)/*/drop/**/TestResults'
        tabName: 'Extent Test Report'
```

### Pipeline Features

1. **Environment-Specific Variables**: Using variable groups for different environments
2. **Conditional Testing**: Running smoke tests before regression tests
3. **Artifacts**: Publishing test results and reports as artifacts
4. **Test Parameters**: Overriding test parameters for different environments
5. **HTML Reports**: Publishing ExtentReports as viewable HTML in Azure DevOps

This pipeline setup enables:
- Continuous integration on code changes
- Automated test execution across environments
- Visibility into test results and failures
- Historical test data for trend analysis
- Environment-specific test configurations
