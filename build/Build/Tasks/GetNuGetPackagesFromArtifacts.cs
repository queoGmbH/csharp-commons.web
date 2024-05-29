using System.IO;

using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for getting the NuGet packages from the artifacts directory.
    /// </summary>
    [TaskName("GetNuGetPackagesFromArtifacts")]
    public class GetNuGetPackagesFromArtifacts : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to get the NuGet packages from the artifacts directory.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            // Get all .nupkg files from the ".artifacts" directory
            foreach (string file in Directory.GetFiles(".artifacts", "*.nupkg"))
            {
                context.General.NuGetPackages.Add(file);
            }

            // Display the list of package files found
            context.Information("Found the following package files:");

            // Add each package file to the list of NuGet packages stored in the context
            foreach (string contextPackageFile in context.General.NuGetPackages)
            {
                context.Information(contextPackageFile);
            }
        }
    }
}
