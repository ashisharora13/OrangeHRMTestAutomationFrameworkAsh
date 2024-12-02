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
using Microsoft.Azure.Pipelines.WebApi;
using Microsoft.TeamFoundation.Dashboards.WebApi;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using static OpenQA.Selenium.BiDi.Modules.Script.RemoteValue;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace OrangeHRMTestAutomationFrameworkAsh
{
    internal class Flowchart
    {
    //flowchart TB
    //Start([Framework Start]) --> CICD[CI / CD Pipeline Trigger]
    //CICD --> Init[Initialize Framework]

    //subgraph Core[Core Components]
    //    Init --> Base[Base Layer Setup]
    //    Base --> BaseTest[BaseTest Setup]
    //    Base --> TestBase[TestBase Setup]
    //    Base --> BasePage[BasePage Setup]

    //    BaseTest --> Browser[Browser Factory]
    //    Browser --> Driver[WebDriver Setup]
    //    Browser --> Config[Configuration Manager]

    //    Config --> Settings[Load AppSettings]
    //    Settings --> EnvConfig[Environment Config]
    //    Settings --> BrowserConfig[Browser Config]
    //    Settings --> AzureConfig[Azure DevOps Config]

    //    BaseTest --> Helpers[Initialize Helpers]
    //    Helpers --> Wait[Wait Helper]
    //    Helpers --> Element[Element Helper]
    //end

    //Core --> Framework[Framework Components]

    //subgraph Framework_Components[Framework Layer]
    //    Framework --> Pages[Initialize Page Objects]
    //    Pages --> Login[Login Page]
    //    Pages --> Dashboard[Dashboard Page]
    //    Pages --> Employee[Employee Page]

    //    Framework --> Data[Test Data Setup]
    //    Data --> Generator[Data Generators]
    //    Data --> Models[Data Models]

    //    Framework --> Report[Initialize Reporting]
    //    Report --> Extent[Extent Reports]
    //    Report --> Azure[Azure DevOps Reporter]
    //end

    //Framework_Components --> TestExec[Test Execution]

    //subgraph Test_Flow[Test Execution Flow]
    //    TestExec --> TestSetup[Test Setup]
    //    TestSetup --> PageInit[Initialize Page Objects]
    //    PageInit --> TestSteps[Execute Test Steps]

    //    TestSteps -->|Success| Pass[Test Passed]
    //    TestSteps -->|Failure| Fail[Test Failed]

    //    Pass --> Screenshot1[Capture Success Screenshot]
    //    Fail --> Screenshot2[Capture Failure Screenshot]

    //    Screenshot1 --> Report1[Update Reports]
    //    Screenshot2 --> Report2[Update Reports]

    //    Report1 --> Azure1[Update Azure DevOps]
    //    Report2 --> Azure2[Update Azure DevOps]
    //end

    //subgraph Report_System[Reporting System]
    //    Report1 --> ExtentReport1[Update Extent Report]
    //    Report2 --> ExtentReport2[Update Extent Report]
    //    Azure1 --> TestCase1[Update Test Case]
    //    Azure2 --> TestCase2[Update Test Case]
    //end

    //subgraph Pipeline[CI / CD Pipeline]
    //    Report_System --> Build[Build Solution]
    //    Build --> RunTest[Execute Tests]
    //    RunTest --> Publish[Publish Results]
    //    Publish --> Deploy[Deploy Reports]
    //end

    //Pipeline --> Cleanup[Resource Cleanup]
    //Cleanup --> End([Framework End])

    //style Core fill:#f9f,stroke:#333,stroke-width:2px
    //style Framework_Components fill:#bbf,stroke:#333,stroke-width:2px
    //style Test_Flow fill:#bfb,stroke:#333,stroke-width:2px
    //style Report_System fill:#fbb,stroke:#333,stroke-width:2px
    //style Pipeline fill:#ff9,stroke:#333,stroke-width:2px

    //classDef success fill:#0f0,stroke:#333,stroke-width:2px
    //classDef failure fill:#f00,stroke:#333,stroke-width:2px
    //classDef component fill:#ddd,stroke:#333,stroke-width:1px
    //class Pass success
    //class Fail failure
    //class Init,Framework,TestExec,Cleanup component
    }
}
