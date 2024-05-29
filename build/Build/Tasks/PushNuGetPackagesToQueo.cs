using System;
using System.Linq;

using Build.Common.Enums;

using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for pushing NuGet packages to queo.
    /// </summary>
    [TaskName("PushNuGetPackagesToQueo")]
    public class PushNuGetPackagesToQueo : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to push NuGet packages to queo.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            CheckRequirements(context);

            foreach (string contextPackageFile in context.General.NuGetPackages)
            {
                context.NuGetPush(contextPackageFile, new NuGetPushSettings()
                {
                    Source = context.Arguments.GetArguments("NuGetSource").First(),
                    ApiKey = context.Arguments.GetArguments("NuGetKey").First()
                });
            }
        }

        /// <summary>
        /// Determines whether the task should be run.
        /// Only run the task if the build is not local and the current branch is the main branch.
        /// </summary>
        /// <param name="context"></param>
        public override bool ShouldRun(Context context)
        {
            return !context.General.IsLocal && context.General.CurrentBranch == Branches.Main;
        }

        /// <summary>
        /// Checks the requirements for the task.
        /// </summary>
        /// <param name="context"></param>
        private void CheckRequirements(Context context)
        {
            // For the task to run, the NuGetSource and NuGetKey arguments must be present.
            if (!context.Arguments.HasArgument("NuGetSource"))
            {
                throw new MissingFieldException("NuGetSource not present");
            }

            if (!context.Arguments.HasArgument("NuGetKey"))
            {
                throw new MissingFieldException("NuGetKey not present");
            }
        }
    }
}
