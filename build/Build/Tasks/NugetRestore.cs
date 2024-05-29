using Cake.Common.Tools.DotNet;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for restoring NuGet packages.
    /// </summary>
    [TaskName("NugetRestore")]
    public class NugetRestore : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to restore NuGet packages.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context) => context.DotNetRestore();
    }
}
