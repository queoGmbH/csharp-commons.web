using System;
using System.IO;

using Build.Common.Services;
using Build.Common.Services.Impl;

using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.Xml;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for generating the version of the artifact.
    /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: SolutionSpecifics or DocOnly.
    /// </summary>
    [TaskName("GenerateVersion")]
    public sealed class GenerateVersion : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to generate the version of the artifact.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            CheckRequirements(context);
            foreach (ProjectToBuild projectToBuild in context.SolutionSpecifics.ProjectsToBuild)
            {
                string assemblyVersion = GetAssemblyVersion(context, projectToBuild);

                context.Information($"Found {assemblyVersion} as assembly version.");

                SetArtifactVersion(context, assemblyVersion);
            }

        }

        /// <summary>
        /// Returns the assembly version.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAssemblyVersion(Context context, ProjectToBuild projectToBuild)
        {
            if (context.SolutionSpecifics != null)
            {
                // Get the version from the csproj file
                return context.XmlPeek(
                    Path.Combine(context.Environment.WorkingDirectory.FullPath, projectToBuild.ProjectPath),
                    "//VersionPrefix");
            }
            else
            {
                // Get the assembly version from the assembly version in the context file
                return context.DocOnly.AssemblyVersion;
            }
        }

        /// <summary>
        /// Sets the artifact version in the context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyVersion"></param>
        private void SetArtifactVersion(Context context, string assemblyVersion)
        {
            IArtifactVersionService versionService = new ArtifactVersionService(new BranchService());
            // Get the artifact version using the version service
            context.General.ArtifactVersion = versionService.GetArtifactVersion(
                context.General.IsLocal,
                context.General.CurrentBranch,
                context.General.CurrentBranchName,
                context.AzurePipelines()?.Environment.Build.Number,
                context.Information,
                assemblyVersion);

            // Log the artifact version
            context.Information($"Artifact Version: {context.General.ArtifactVersion}");
        }

        /// <summary>
        /// Checks the requirements for the task.
        /// </summary>
        /// <param name="context"></param>
        private void CheckRequirements(Context context)
        {
            // For the task to run, either ProjectSpecific or DocOnly in the context.s file must not be null.
            if (context.SolutionSpecifics == null && context.DocOnly == null)
            {
                throw new MissingFieldException("Either SolutionSpecifics or DocOnly in the Context.cs file must not be null.");
            }
        }
    }
}
