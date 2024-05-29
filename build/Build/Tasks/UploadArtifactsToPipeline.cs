using System.Linq;

using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for uploading artifacts and NuGet packages to the pipeline.
    /// </summary>
    [TaskName("UploadArtifactsToPipeline")]
    public sealed class UploadArtifactsToPipeline : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to upload artifacts and NuGet package to the pipeline.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Run(Context context)
        {
            context.Information("Remote Build - Uploading artifacts to the pipeline.");
            context.Information("Upload in progress...");

            foreach (string pipelineFile in context.General.ArtifactsToUploadToPipeline.Concat(context.General.NuGetPackages))
            {
                context.AzurePipelines().Commands.UploadArtifact(
                    "website",
                    pipelineFile,
                    "artifacts");
            }
        }

        /// <summary>
        /// Checks if the task should run. Returns true if the build is not local; otherwise, false.
        /// </summary>
        /// <param name="context">The context.</param>
        public override bool ShouldRun(Context context) => !context.General.IsLocal;
    }
}
