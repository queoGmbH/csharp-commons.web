using System.Collections.Generic;

using Build.Common.Enums;

using Cake.Core;
using Cake.Frosting;

using Semver;

namespace Build
{
    /// <summary>
    /// Base class for the build context.
    /// </summary>
    public abstract class ContextBase : FrostingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextBase"/> class.
        /// </summary>
        /// <param name="context"></param>
        public ContextBase(ICakeContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the general settings and properties.
        /// </summary>
        public General General { get; } = new();
    }

    /// <summary>
    /// Represents general settings and properties related to the build process.
    /// </summary>
    public class General
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets the directory where artifacts are stored.
        /// </summary>
        public string ArtifactsDir => ".artifacts";

        /// <summary>
        /// Gets the list of artifacts to upload to a pipeline.
        /// </summary>
        public IList<string> ArtifactsToUploadToPipeline { get; } = new List<string>();

        /// <summary>
        /// Gets the list of artifacts to upload to a transfer.
        /// </summary>
        public IList<string> ArtifactsToUploadToTransfer { get; } = new List<string>();

        /// <summary>
        /// Gets the list of NuGet packages.
        /// </summary>
        public IList<string> NuGetPackages { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the version of the artifact.
        /// </summary>
        public SemVersion ArtifactVersion { get; set; }

        /// <summary>
        /// Gets or sets the current branch.
        /// </summary>
        public Branches CurrentBranch { get; set; }

        /// <summary>
        /// Gets or sets the name of the current branch.
        /// </summary>
        public string CurrentBranchName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the build is running locally.
        /// </summary>
        public bool IsLocal { get; set; }
    }
}
