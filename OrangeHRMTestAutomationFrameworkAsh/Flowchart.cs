using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Process.WebApi.Models;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.Users;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using OpenQA.Selenium.BiDi.Communication;
using OrangeHRM.Automation.Framework.Core.Base;
using OrangeHRM.Automation.Framework.Core.Browser;
using OrangeHRM.Automation.Framework.Core.Configuration;
using OrangeHRM.Automation.Framework.Core.Helpers;
using OrangeHRM.Automation.Framework.PageObjects;
using OrangeHRM.Automation.Framework.Reporting;
using OrangeHRMTestAutomationFrameworkAsh.CoreFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Azure.Pipelines.WebApi.PipelinesResources;

namespace OrangeHRMTestAutomationFrameworkAsh
{
    internal class Flowchart
    {
    //    flowchart TB
    //Start([Framework Start]) --> CICD{CI/CD Pipeline}

    //subgraph Core_Components[Core Components]
    //    direction TB
    //    subgraph Base[Base Classes]
    //        BaseTest[BaseTest.cs]
    //        TestBase[TestBase.cs]
    //        BasePage[BasePage.cs]
    //    end


    //    subgraph Browser[Browser Factory]
    //        BrowserFactory[BrowserFactory.cs]
    //        Driver[WebDriver Setup]
    //        BrowserConfig[Browser Configurations]
    //    end


    //    subgraph Config[Configuration]
    //        ConfigManager[ConfigurationManager.cs]
    //        AppSettings[AppSettings.json]
    //        EnvConfig[Environment Config]
    //    end


    //    subgraph Helpers[Helper Classes]
    //        WaitHelper[Wait Utilities]
    //        ElementHelper[Element Actions]
    //        CommonUtils[Common Utilities]
    //    end
    //end

    //subgraph Framework_Components[Framework Components]
    //    direction TB
    //    subgraph Pages[Page Objects]
    //        LoginPage[Login Page]
    //        DashboardPage[Dashboard Page]
    //        EmployeePage[Employee Page]
    //        LeavePage[Leave Page]
    //    end


    //    subgraph TestData[Test Data]
    //        DataGen[Data Generators]
    //        TestModels[Data Models]
    //        DataProvider[Data Providers]
    //    end


    //    subgraph Reports[Reporting System]
    //        ExtentReport[Extent Reports]
    //        AzureReport[Azure DevOps]
    //        Screenshots[Screenshot Manager]
    //end
    //end

    //subgraph Test_Execution[Test Execution]
    //    direction TB
    //    TestInit[Test Initialization]
    //    TestSteps[Test Steps]
    //    Assertions[Assertions & Logging]
    //    Results[Test Results]
    //end


    //subgraph Pipeline[CI / CD Pipeline]
    //    direction TB
    //    Build[Build Solution]
    //    TestRun[Run Tests]
    //    PublishResults[Publish Results]
    //    DeployReport[Deploy Reports]
    //end

    //%% Connections
    //CICD -->|Trigger| Core_Components
    //Core_Components -->|Initialize| Framework_Components
    //Framework_Components -->|Use| Test_Execution
    //Test_Execution -->|Generate| Reports
    //Test_Execution -->|Update| Pipeline
    
    //%% Base Connections
    //BaseTest -->|Extends| TestBase
    //BasePage -->|Used by| Pages
    
    //%% Browser Connections
    //BrowserFactory -->|Creates| Driver
    //Driver -->|Used by| BaseTest
    
    //%% Config Connections
    //ConfigManager -->|Reads| AppSettings
    //ConfigManager -->|Used by| BaseTest
    
    //%% Helper Connections
    //WaitHelper -->|Used by| BasePage
    //ElementHelper -->|Used by| BasePage
    
    //%% Page Connections
    //Pages -->|Used by| Test_Execution
    
    //%% Data Connections
    //TestData -->|Provides| Test_Execution
    
    //%% Report Connections
    //Reports -->|Updates| Pipeline
    
    //%% Styling
    //classDef coreStyle fill:#f9f,stroke:#333,stroke-width:2px;
    //classDef frameworkStyle fill:#bbf,stroke:#333,stroke-width:2px;
    //classDef execStyle fill:#bfb,stroke:#333,stroke-width:2px;
    //classDef pipelineStyle fill:#fbb,stroke:#333,stroke-width:2px;
    
    //class Core_Components coreStyle;
    //class Framework_Components frameworkStyle;
    //class Test_Execution execStyle;
    //class Pipeline pipelineStyle;
    
    //%% Subgraph Styling
    //style Base fill:#f9f9f9,stroke:#999,stroke-width:2px;
    //style Browser fill:#f9f9f9,stroke:#999,stroke-width:2px;
    //style Config fill:#f9f9f9,stroke:#999,stroke-width:2px;
    //style Helpers fill:#f9f9f9,stroke:#999,stroke-width:2px;
    //style Pages fill:#e6f3ff,stroke:#999,stroke-width:2px;
    //style TestData fill:#e6f3ff,stroke:#999,stroke-width:2px;
    //style Reports fill:#e6f3ff,stroke:#999,stroke-width:2px;
    }
}
