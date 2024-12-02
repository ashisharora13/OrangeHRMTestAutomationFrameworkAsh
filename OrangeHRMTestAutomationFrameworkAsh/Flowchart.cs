using AventStack.ExtentReports.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using OrangeHRM.Automation.Framework.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRMTestAutomationFrameworkAsh
{
    internal class Flowchart
    {
    //    flowchart TB
    //Start([Test Execution Start]) --> Config[Load Configuration]
    //Config --> Init[Initialize Framework]

    //subgraph Framework_Init[Framework Initialization]
    //    Init --> Driver[Create WebDriver]
    //    Init --> Reports[Initialize Reports]
    //    Init --> Azure[Setup Azure DevOps]
    //end


    //Framework_Init --> TestExec[Test Execution]

    //subgraph Test_Execution[Test Execution Flow]
    //    TestExec --> TestSetup[Test Setup]
    //    TestSetup --> PageInit[Initialize Page Objects]
    //    PageInit --> TestSteps[Execute Test Steps]

    //    TestSteps -->|Success| Pass[Test Passed]
    //    TestSteps --> |Failure| Fail[Test Failed]

    //    Pass --> Screenshot1[Capture Success Screenshot]
    //    Fail --> Screenshot2[Capture Failure Screenshot]

    //    Screenshot1 --> Report1[Update Reports]
    //    Screenshot2 --> Report2[Update Reports]

    //    Report1 --> Azure1[Update Azure DevOps]
    //    Report2 --> Azure2[Update Azure DevOps]
    //end


    //subgraph Reporting[Reporting System]
    //    Report1 --> Extent1[Extent Report]
    //    Report2 --> Extent2[Extent Report]
    //    Azure1 --> TestCase1[Update Test Case]
    //    Azure2 --> TestCase2[Update Test Case]
    //end


    //subgraph Cleanup[Test Cleanup]
    //    Extent1 --> Cleanup1[Cleanup Resources]
    //    Extent2 --> Cleanup1
    //    TestCase1 --> Cleanup1
    //    TestCase2 --> Cleanup1
    //    Cleanup1 --> Driver2[Close Driver]
    //end


    //Cleanup --> FinalReport[Generate Final Report]
    //FinalReport --> End([Test Execution End])


    //style Framework_Init fill:#f9f,stroke:#333,stroke-width:2px
    //style Test_Execution fill:#bbf,stroke:#333,stroke-width:2px
    //style Reporting fill:#bfb,stroke:#333,stroke-width:2px
    //style Cleanup fill:#fbb,stroke:#333,stroke-width:2px
    
    //classDef success fill:#0f0,stroke:#333,stroke-width:2px;
    //classDef failure fill:#f00,stroke:#333,stroke-width:2px;
    //class Pass success;
    //class Fail failure;
    }
}
