using Build.Common.Enums;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for publishing the application.
    /// </summary>
    [TaskName("PublishApp")]
    public class PublishApp : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to publish the application.
        /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: ProjectSpecifics.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            string versionSuffix = GetVersionSuffix(context);

            DotNetPublishSettings dotNetCorePublishSettings = new()
            {
                Configuration = "release",
                PublishSingleFile = true,
                SelfContained = true,
                PublishReadyToRun = true,
                Runtime = "win-x64",
                OutputDirectory = context.General.ArtifactsDir,
                VersionSuffix = versionSuffix
            };

            // Publish the main project using the DotNetPublish tool.
            context.DotNetPublish(context.ProjectSpecifics.MainProject, dotNetCorePublishSettings);
        }

        /// <summary>
        /// Method that returns the version suffix to be used when publishing the app.
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
