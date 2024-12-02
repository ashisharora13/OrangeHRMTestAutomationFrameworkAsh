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

namespace OrangeHRMTestAutomationFrameworkAsh
{
    internal class Flowchart
    {
    //flowchart TB
    //Start([Framework Start]) --> Core[Core Components]

    //Core --> Base[Base Classes]
    //Core --> Browser[Browser Factory]
    //Core --> Config[Configuration]
    //Core --> Helpers[Helper Classes]

    //Base --> BasePage[BasePage]
    //Base --> BaseTest[BaseTest]
    //    Base --> TestBase[TestBase]

    //Browser --> BFactory[BrowserFactory]
    //Browser --> WDriver[WebDriver Setup]

    //Config --> CManager[ConfigurationManager]
    //Config --> Settings[AppSettings]

    //Helpers --> Wait[Wait Utilities]
    //Helpers --> Element[Element Actions]

    //Core --> Framework[Framework Components]

    //Framework --> Pages[Page Objects]
    //Framework --> Data[Test Data]
    //Framework --> Report[Reporting]

    //Pages --> Login[LoginPage]
    //Pages --> Dashboard[DashboardPage]
    //Pages --> Employee[EmployeePage]

    //Data --> Generator[Data Generators]
    //Data --> Models[Data Models]

    //Report --> Extent[Extent Reports]
    //Report --> Azure[Azure DevOps]

    //Framework --> Tests[Test Execution]

    //Tests --> Setup[Test Setup]
    //Tests --> Execute[Test Steps]
    //Tests --> Assert[Assertions]
    //Tests --> Results[Test Results]

    //Tests --> Pipeline[CI / CD Pipeline]

    //Pipeline --> Build[Build]
    //Pipeline --> Run[Test Run]
    //Pipeline --> Publish[Publish Results]

    //classDef default fill:#f9f,stroke:#333,stroke-width:1px
    //classDef framework fill:#bbf,stroke:#333,stroke-width:1px
    //classDef execution fill:#bfb,stroke:#333,stroke-width:1px
    //classDef pipeline fill:#fbb,stroke:#333,stroke-width:1px
    
    //class Core,Base,Browser,Config,Helpers default
    //class Framework,Pages,Data,Report framework
    //class Tests,Setup,Execute,Assert,Results execution
    //class Pipeline,Build,Run,Publish pipeline
    }
}
