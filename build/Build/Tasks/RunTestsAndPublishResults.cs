using System.Collections.Generic;
using Build.Common.Builder;
using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Frosting;

using Path = System.IO.Path;

namespace Build
{
    /// <summary>
    /// Task for running tests and publishing the results.
    /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: SolutionSpecifics, Tests.
    /// </summary>
    [TaskName("RunTestsAndPublishResults")]
    public sealed class RunTestsAndPublishResults : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to run tests and publish the results.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Run(Context context)
        {
            IDictionary<string, string> testProjects = GetTestProjects(context);

            // Define the path to store the test artifacts
            string testArtifactsPath = Path.Combine(context.Environment.WorkingDirectory.FullPath,
                $"{context.General.ArtifactsDir}.tests");
            try
            {
                // Iterate over each test project
                foreach (KeyValuePair<string, string> nameAndPath in testProjects)
                {
                    string coverletArgs = GetCoverletArgs(testArtifactsPath, nameAndPath);
                    context.Information($"Coverlet args: {coverletArgs}");

                    RunTestsForTestProject(context, nameAndPath, coverletArgs);
                }
                if (context.Environment.Platform.IsWindows())
                {
                    // Generate the coverage report
                    GenerateCoverageReport(context, testArtifactsPath);
                }
            }
            finally
            {
                // Only Publish the test results if the build is not local
                if (!context.BuildSystem().IsLocalBuild)
                {
                    // Iterate over each test project
                    foreach (KeyValuePair<string, string> nameAndPath in testProjects)
                    {
                        PublishTestResults(context, nameAndPath);
                    }
                    if (context.Environment.Platform.IsWindows())
                    {
                        // Publish the code coverage results
                        PublishCodeCoverage(context, testArtifactsPath);
                    }
                }
            }
        }

        /// <summary>
        ///  Publish the code coverage results.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="testArtifactsPath"></param>
        private static void PublishCodeCoverage(Context context, string testArtifactsPath)
        {
            context.AzurePipelines().Commands.PublishCodeCoverage(new AzurePipelinesPublishCodeCoverageData
            {
                CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
                SummaryFileLocation = Path.Combine(testArtifactsPath, "coverage/Cobertura.xml"),
                ReportDirectory = Path.Combine(testArtifactsPath, "coverage")
            });
        }

        /// <summary>
        /// Generate the coverage report.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="testArtifactsPath"></param>
        private static void GenerateCoverageReport(Context context, string testArtifactsPath)
        {
            // In Multi-Target builds, the GlobPatern must be updated to include the target framework
            context.ReportGenerator(new GlobPattern($"{testArtifactsPath}/*.coverage.net8.0.xml"), Path.Combine(testArtifactsPath, "coverage"), new ReportGeneratorSettings()
            {
                ReportTypes = new List<ReportGeneratorReportType>()
                        {
                            ReportGeneratorReportType.Cobertura,
                            ReportGeneratorReportType.HtmlInline_AzurePipelines
                        }
            });
        }

        /// <summary>
        /// Get the coverlet arguments.
        /// </summary>
        /// <param name="testArtifactsPath"></param>
        /// <param name="nameAndPath"></param>
        /// <returns></returns>
        private static string GetCoverletArgs(string testArtifactsPath, KeyValuePair<string, string> nameAndPath)
        {
            string coverletArgs = new DotNetTestCoverletParameterBuilder()
            {
                CollectCoverage = true,
                CoverletOutputFormat = "opencover",
                CoverletOutput = $"{testArtifactsPath}/{nameAndPath.Key}.coverage.xml",
                Exclude = [
                            "[*.Tests?]*"
                        ],
                ExcludeByFile = []
            };
            return coverletArgs;
        }

        /// <summary>
        /// Get the test projects to run.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetTestProjects(Context context)
        {
            // Define a dictionary to store the test project names and their corresponding paths
            return context.Tests.TestProjects;
        }

        /// <summary>
        /// Run the tests for a single testProject
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nameAndPath"></param>
        private void RunTestsForTestProject(Context context, KeyValuePair<string, string> nameAndPath, string coverletArgs)
        {
            FilePath vsTestReportPath = Path.Combine(context.Environment.WorkingDirectory.FullPath,
                $"{context.General.ArtifactsDir}.tests", $"{nameAndPath.Key}.TestResult.xml");
            DotNetTestSettings dotNetTestSettings = new()
            {
                VSTestReportPath = vsTestReportPath,
                Configuration = context.Tests.BuildConfig,
                ArgumentCustomization = delegate (ProcessArgumentBuilder argument)
                {
                    argument.Append(new TextArgument(coverletArgs));
                    return argument;
                }
            };
            context.DotNetTest(nameAndPath.Value, dotNetTestSettings);
        }

        /// <summary>
        /// Publish test results for a single testProject
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nameAndPath"></param>
        private static void PublishTestResults(Context context, KeyValuePair<string, string> nameAndPath)
        {
            context.AzurePipelines().Commands.PublishTestResults(new AzurePipelinesPublishTestResultsData
            {
                TestResultsFiles = new List<FilePath>
                    {Path.Combine(context.Environment.WorkingDirectory.FullPath,
                        $"{context.General.ArtifactsDir}.tests",
                        $"{nameAndPath.Key}.TestResult.xml")
                    },
                TestRunner = AzurePipelinesTestRunnerType.VSTest
            });
        }
    }
}
