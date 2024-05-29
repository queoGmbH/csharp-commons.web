using System.IO;

using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for zipping the artifacts.
    /// </summary>
    [TaskName("ZipArtifacts")]
    public sealed class ZipArtifacts : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to zip the artifacts.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            // Generate the name of the zip file
            string appZip = $"{context.General.Name}_App-{context.General.ArtifactVersion}.zip";

            // Generate the destination path for the zip file
            string destination = Path.Combine(context.Environment.WorkingDirectory.FullPath,
                context.General.ArtifactsDir, appZip);

            // Log the information about the zip file being created
            context.Information($"Zipping as {appZip}");

            // Zip the artifacts directory to the destination path
            context.Zip(
                Path.Combine(context.Environment.WorkingDirectory.FullPath, context.General.ArtifactsDir),
                destination);

            // Add the destination path to the list of artifacts to upload to the pipeline
            context.General.ArtifactsToUploadToPipeline.Add(destination);
        }
    }
}
