using System;

using Build.Common.Extensions;
using Build.Common.Services.Impl;

using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Represents the lifetime of the build process.
    /// </summary>
    public sealed class Lifetime : FrostingLifetime<Context>
    {
        /// <summary>
        /// Sets up the build context before the build process starts.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="setupContext"></param>
        public override void Setup(Context context, ISetupContext setupContext)
        {
            context.Information("Setting things up...");

            context.CleanDirectory(context.General.ArtifactsDir.AsDirectoryPath());

            // Clean the generated documentation directory if context.Doc is not null
            if (context.Doc != null)
            {
                context.CleanDirectory(context.Doc.GeneratedPath.AsDirectoryPath());
            }

            context.General.IsLocal = context.BuildSystem().IsLocalBuild;

            SetBranchInContext(context);

            context.Information(string.Join(Environment.NewLine, context.Environment.GetEnvironmentVariables()));
        }

        /// <summary>
        /// Tears down the build context after the build process finishes.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="info"></param>
        public override void Teardown(Context context, ITeardownContext info) =>
            context.Information("Tearing things down...");

        /// <summary>
        /// Sets the current branch in the build context.
        /// </summary>
        /// <param name="context"></param>
        private void SetBranchInContext(Context context)
        {
            string branchName = context.GitVersion(new GitVersionSettings { NoFetch = true }).BranchName.ToLower();
            context.General.CurrentBranchName = branchName;
            context.General.CurrentBranch = new BranchService().GetBranch(branchName);
        }
    }
}
