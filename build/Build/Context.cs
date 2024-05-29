using Cake.Core;

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
        /// Gets the project-specific settings.
        /// </summary>
        public ProjectSpecifics ProjectSpecifics { get; } = new();

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
    /// Represents the project-specific settings.
    /// </summary>
    public class ProjectSpecifics
    {
        /// <summary>
        /// Gets the name of the solution.
        /// </summary>
        public string SolutionName { get; } = "Commons.Web.sln";

        /// <summary>
        /// Gets the build configuration.
        /// </summary>
        public string BuildConfig { get; } = "Release";

        /// <summary>
        /// Gets the path to the main project file.
        /// </summary>
        public string MainProject { get; } = @"src\Commons.Web\Commons.Web.csproj";
    }

    /// <summary>
    /// Represents the test settings.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Gets the name of the test project.
        /// </summary>
        public string TestProjectName { get; } = "Commons.Web.Tests";

        /// <summary>
        /// Gets the path to the test project file.
        /// </summary>
        public string TestProject { get; } = @"tests\Commons.Web.Tests\Commons.Web.Tests.csproj";
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
