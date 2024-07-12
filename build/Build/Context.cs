using Cake.Core;
using System.Collections.Generic;

namespace Build
{
    /// <summary>
    /// Represents the context for the build process.
    /// </summary>
    public class Context : ContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="context"></param>
        public Context(ICakeContext context) : base(context)
        {
            General.Name = "Commons.Web";
        }

        /// <summary>
        /// Gets the solution-specific settings.
        /// </summary>
        public SolutionSpecifics SolutionSpecifics { get; } = new();

        /// <summary>
        /// Gets the documentation settings.
        /// </summary>
        public Doc Doc { get; } = new();

        /// <summary>
        /// Gets the documentation-only settings.
        /// </summary>
        public DocOnly DocOnly { get; } = null;

        /// <summary>
        /// Gets the test settings.
        /// </summary>
        public Tests Tests { get; } = new();
    }
    /// <summary>
    /// Represents the solution-specific settings.
    /// </summary>
    public class SolutionSpecifics
    {
        /// <summary>
        /// Gets the name of the solution.
        /// </summary>
        public string SolutionName { get; } = "Commons.Web.sln";

        /// <summary>
        /// Gets the projects to build within the solution.
        /// </summary>
        public List<ProjectToBuild> ProjectsToBuild { get; } = new List<ProjectToBuild>() {
            new ProjectToBuild("Release", @"src\Commons.Web.ExceptionHandling\Commons.Web.ExceptionHandling.csproj"),
            new ProjectToBuild("Release", @"src\Commons.Web.ModelBinding\Commons.Web.ModelBinding.csproj"),
            new ProjectToBuild("Release", @"src\Commons.Web.Security\Commons.Web.Security.csproj"),
        };
    }

    /// <summary>
    /// Represents a project to build within the solution.
    /// </summary>
    public class ProjectToBuild
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public ProjectToBuild(string buildConfig, string project, bool shouldBePublished)
        {
            BuildConfig = buildConfig;
            ProjectPath = project;
            ShouldBePublished = shouldBePublished;
        }

        /// <summary>
        /// Gets the build configuration for the project.
        /// </summary>
        public string BuildConfig { get; }

        /// <summary>
        /// Gets the path to the project file.
        /// </summary>
        public string ProjectPath { get; }

        /// <summary>
        /// Gets the flag indication whether the project should be published.
        /// </summary>
        public bool ShouldBePublished { get; }
    }

    /// <summary>
    /// Represents the test settings.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Gets the test projects.
        /// </summary>
        public Dictionary<string, string> TestProjects { get; } = new Dictionary<string, string>() {
            { "Commons.Web.ExceptionHandling.Tests", @"tests\Commons.Web.ExceptionHandling.Tests\Commons.Web.ExceptionHandling.Tests.csproj" },
            { "Commons.Web.ModelBinding.Tests", @"tests\Commons.Web.ModelBinding.Tests\Commons.Web.ModelBinding.Tests.csproj" },
            { "Commons.Web.Security.Tests", @"tests\Commons.Web.Security.Tests\Commons.Web.Security.Tests.csproj" }
        };
        public string BuildConfig { get; } = "Release";
    }

    /// <summary>
    /// Represents the documentation settings.
    /// </summary>
    public class Doc
    {
        /// <summary>
        /// Gets the path to the generated documentation.
        /// </summary>
        public string GeneratedPath { get; } = @"doc\Generated";

        /// <summary>
        /// Gets the path to the root documentation file.
        /// </summary>
        public string RootFilePath { get; } = @"doc\Demo.adoc";
    }

    /// <summary>
    /// Represents the documentation-only settings, when there is no project to build.
    /// </summary>
    public class DocOnly
    {
        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        public string AssemblyVersion { get; } = "1.0.0.0";
    }
}
