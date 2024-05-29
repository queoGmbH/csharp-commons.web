using Build.Common.Enums;
using Cake.Common.Tools.DotNet;
using Cake.Frosting;
using System.IO;

namespace Build.Tasks
{
    /// <summary>
    /// Task for building the NuGet package.
    /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: ProjectSpecifics.
    /// </summary>
    public sealed class BuildNuGetPackage : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to build the NuGet package.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            string versionSuffix = GetVersionSuffix(context);

            // Pack the main project using the DotNetPack tool.
            context.DotNetPack(Path.Combine(context.Environment.WorkingDirectory.FullPath, context.ProjectSpecifics.MainProject),
                new Cake.Common.Tools.DotNet.Pack.DotNetPackSettings()
                {
                    Configuration = context.ProjectSpecifics.BuildConfig,
                    OutputDirectory = context.General.ArtifactsDir,
                    VersionSuffix = versionSuffix,
                });
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
