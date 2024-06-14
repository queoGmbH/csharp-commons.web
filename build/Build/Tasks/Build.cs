using System.IO;

using Build.Common.Enums;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for building the main project.
    /// </summary>
    [TaskName("Build")]
    public sealed class Build : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to build the main project.
        /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: SolutionSpecifics.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            string versionSuffix = GetVersionSuffix(context);

            foreach (ProjectToBuild projectToBuild in context.SolutionSpecifics.ProjectsToBuild)
            {
                // Build project using the DotNetBuild tool.
                context.DotNetBuild(
                    Path.Combine(context.Environment.WorkingDirectory.FullPath, projectToBuild.ProjectPath),
                    new DotNetBuildSettings
                    {
                        VersionSuffix = versionSuffix,
                        Configuration = projectToBuild.BuildConfig
                    });
            }
        }

        /// <summary>
        /// Method that returns the version suffix to be used when building the project.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetVersionSuffix(Context context)
        {
            string versionSuffix = string.Empty;

            // Check if the build is being executed locally or not on the main branch.
            bool isLocalOrNotMainBranch = context.General.IsLocal || !(context.General.CurrentBranch == Branches.Main);

            // If the build is being executed locally or not on the main branch, set the version suffix to include the prerelease and build number.
            if (isLocalOrNotMainBranch)
            {
                versionSuffix = $"{context.General.ArtifactVersion.Prerelease}-{context.General.ArtifactVersion.Build}";
            }
            return versionSuffix;
        }
    }
}
