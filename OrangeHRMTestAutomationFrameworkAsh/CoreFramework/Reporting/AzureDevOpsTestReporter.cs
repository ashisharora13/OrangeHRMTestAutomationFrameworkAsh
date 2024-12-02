using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using NUnit.Framework;
using OrangeHRM.Automation.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;

namespace OrangeHRM.Automation.Framework.Reporting
{
    public class AzureDevOpsTestReporter
    {
        private readonly string _azureDevOpsUrl;
        private readonly string _personalAccessToken;
        private readonly string _project;
        private readonly TestManagementHttpClient _testClient;
        private readonly WorkItemTrackingHttpClient _workItemClient;
        private readonly VssConnection _connection;
        private string StartDate;

        public AzureDevOpsTestReporter(string Url, string PersonalAccessToken, string ProjectName)
        {
            try
            {
                if (string.IsNullOrEmpty(Url))
                    throw new ArgumentNullException(nameof(Url), "Azure DevOps URL cannot be null or empty");
                if (string.IsNullOrEmpty(PersonalAccessToken))
                    throw new ArgumentNullException(nameof(PersonalAccessToken), "Personal Access Token cannot be null or empty");
                if (string.IsNullOrEmpty(ProjectName))
                    throw new ArgumentNullException(nameof(ProjectName), "Project name cannot be null or empty");

                _azureDevOpsUrl = Url.TrimEnd('/');
                if (!_azureDevOpsUrl.StartsWith("https://dev.azure.com/"))
                {
                    throw new ArgumentException("Azure DevOps URL must be in format: https://dev.azure.com/organization");
                }
                _personalAccessToken = PersonalAccessToken;
                _project = ProjectName;

                TestContext.Progress.WriteLine($"Initializing Azure DevOps connection to: {_azureDevOpsUrl}");

                var credentials = new VssBasicCredential(string.Empty, _personalAccessToken);
                var settings = VssClientHttpRequestSettings.Default.Clone();
                settings.MaxRetryRequest = 3;

                _connection = new VssConnection(new Uri(_azureDevOpsUrl), credentials, settings);

                // Verify connection
                TestContext.Progress.WriteLine("Verifying Azure DevOps connection...");
                _connection.ConnectAsync().GetAwaiter().GetResult();
                TestContext.Progress.WriteLine("Connection verified successfully");

                //Initialise clients
                _testClient = _connection.GetClient<TestManagementHttpClient>();
                _workItemClient = _connection.GetClient<WorkItemTrackingHttpClient>();

                TestContext.Progress.WriteLine("Azure DevOps Test Reporter initialized successfully");
            }

            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to initialize Azure DevOps Test Reporter: {ex.Message}");
                TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }

        }

        public async Task<TestRun> CreateTestRun(string testSuiteName)
        {
            try
            {
                if (string.IsNullOrEmpty(testSuiteName))
                    throw new ArgumentNullException(nameof(testSuiteName), "Test suite name cannot be null or empty");

                var runName = $"Automated Test Run - {testSuiteName} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                TestContext.Progress.WriteLine($"Creating test run: {runName}");

                var testRun = new RunCreateModel(
                    name: runName,
                    isAutomated: true);

                var createdRun = await _testClient.CreateTestRunAsync(
                    project: _project,
                    testRun: testRun);

                if (createdRun != null)
                {
                    TestContext.Progress.WriteLine($"Test run created successfully. ID: {createdRun.Id}");
                    return createdRun;
                }

                throw new Exception("Test run creation returned null result");
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to create test run: {ex.Message}");
                TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }



        public async Task UpdateTestCase(string testCaseId, bool passed, string errorMessage = null)
        {
            try
            {
                if (string.IsNullOrEmpty(testCaseId))
                    throw new ArgumentNullException(nameof(testCaseId), "Test case ID cannot be null or empty");

                TestContext.Progress.WriteLine($"Updating test case {testCaseId}");

                // Create a patch document
                var patchDocument = new JsonPatchDocument();

                // Update the automation status
                patchDocument.Add(
                    new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = "/fields/Microsoft.VSTS.TCM.AutomationStatus",
                        Value = "Automated"
                    }
                );

                // Update the test result
                if (passed)
                {
                    patchDocument.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/Microsoft.VSTS.TCM.TestStatus",
                            Value = "Passed"
                        }
                    );
                }
                else
                {
                    patchDocument.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/Microsoft.VSTS.TCM.TestStatus",
                            Value = "Failed"
                        }
                    );

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        patchDocument.Add(
                            new JsonPatchOperation()
                            {
                                Operation = Operation.Add,
                                Path = "/fields/Microsoft.VSTS.TCM.ErrorMessage",
                                Value = errorMessage
                            }
                        );
                    }
                }

                // Update the test case
                var updatedWorkItem = await _workItemClient.UpdateWorkItemAsync(
                    patchDocument,
                    int.Parse(testCaseId));

                if (updatedWorkItem != null)
                {
                    TestContext.Progress.WriteLine($"Test case {testCaseId} updated successfully");
                }
                else
                {
                    throw new Exception($"Failed to update test case {testCaseId}");
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to update test case {testCaseId}: {ex.Message}");
                TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task AddTestResult(string testCaseId, TestResult result, string runId)
        {
            try
            {
                if (string.IsNullOrEmpty(testCaseId))
                    throw new ArgumentNullException(nameof(testCaseId), "Test case ID cannot be null or empty");

                if (result == null)
                    throw new ArgumentNullException(nameof(result), "Test result cannot be null");

                if (string.IsNullOrEmpty(runId))
                    throw new ArgumentNullException(nameof(runId), "Run ID cannot be null or empty");

                TestContext.Progress.WriteLine($"Adding test result for test case {testCaseId} to run {runId}");

                var testResults = new TestCaseResult[]
                {
            new TestCaseResult
            {
                TestCase = new ShallowReference
                {
                    Id = testCaseId
                },
                Outcome = result.Outcome,
                ErrorMessage = result.Message,
                State = "Completed",
                StartedDate = result.StartTime,
                CompletedDate = result.EndTime,
                DurationInMs = result.Duration,
                TestRun = new ShallowReference
                {
                    Id = runId
                }
            }
                };

                // Add test results to the test run
                var addedResults = await _testClient.AddTestResultsToTestRunAsync(
                    testResults,
                    _project,
                    int.Parse(runId));

                if (addedResults != null && addedResults.Count > 0)  // Changed from Length to Count
                {
                    TestContext.Progress.WriteLine($"Test result added successfully for test case {testCaseId}");
                }
                else
                {
                    throw new Exception($"Failed to add test result for test case {testCaseId}");
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to add test result for test case {testCaseId}: {ex.Message}");
                TestContext.Progress.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
