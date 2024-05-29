using System.IO;

using Build.Common.Extensions;

using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Tast for zipping the generated documentation artifacts.
    /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: Doc.
    /// </summary>
    [TaskName("ZipDocumentationArtifacts")]
    public class ZipDocumentationArtifacts : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to zip the generated documentation artifacts.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            // Generate the zip file name based on the project name and artifact version
            string zipFileName = $"{context.General.Name}_Doc-{context.General.ArtifactVersion}.zip";

            // Generate the full file path for the zip file
            string zipFilePath = $"{context.General.ArtifactsDir}{Path.DirectorySeparatorChar}{zipFileName}";

            // Log the zip file path
            context.Information($"Zipping to {zipFilePath}");

            // Zip the generated documentation artifacts
            context.Zip(context.Doc.GeneratedPath.AsDirectoryPath(), zipFilePath);

            // Add the zip file path to the list of artifacts to be uploaded to the pipeline
            context.General.ArtifactsToUploadToPipeline.Add(zipFilePath);

            // Following lines will only have an effect if the Task UploadArtifactsToQueoTransfer is used

            // Add the zip file path to the list of artifacts to be uploaded to transfer
            context.General.ArtifactsToUploadToTransfer.Add(zipFilePath);

            // Generate the file path for the latest version of the documentation artifact
            string docArtifactAsLatest = $@"{context.General.ArtifactsDir}{Path.DirectorySeparatorChar}{context.General.Name}_Doc_latest.zip";

            // Copy the zip file to the latest version file path
            context.CopyFile(zipFilePath.AsFilePath(), docArtifactAsLatest.AsFilePath());

            // Add the latest version of the documentation artifact to the list of artifacts to be uploaded to transfer
            context.General.ArtifactsToUploadToTransfer.Add(docArtifactAsLatest);
        }
    }
}
