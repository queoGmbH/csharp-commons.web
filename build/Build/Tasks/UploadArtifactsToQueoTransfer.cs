using System;
using System.IO;

using Build.Common.Enums;
using Build.Common.Extensions;

using Cake.Core;
using Cake.Frosting;

using WinSCP;

namespace Build
{
    /// <summary>
    /// Task for uploading artifacts to Queo Transfer FTP.
    /// </summary>
    [TaskName("UploadArtifactsToQueoTransfer")]
    public class UploadArtifactsToQueoTransfer : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            CheckRequirements(context);

            SessionOptions sessionOption = SetSessionOptions(context);

            using Session session = new();
            session.Open(sessionOption);
            foreach (string transferFile in context.General.ArtifactsToUploadToTransfer)
            {
                PutFileToDirectory(context, session, transferFile);
            }
            session.Close();
        }

        private static void PutFileToDirectory(Context context, Session session, string transferFile)
        {
            session.PutFileToDirectory(
                                Path.Combine(context.Environment.WorkingDirectory.FullPath, transferFile),
                                "/data/",
                                false,
                                new TransferOptions { OverwriteMode = OverwriteMode.Overwrite });
        }

        private SessionOptions SetSessionOptions(Context context)
        {
            return new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = context.Arguments.GetArgument("QueoTransferUrl"),
                PortNumber = context.Arguments.GetArgument("QueoTransferPort").AsInt(),
                UserName = context.Arguments.GetArgument("QueoTransferUser"),
                Password = context.Arguments.GetArgument("QueoTransferPwd"),
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
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
        /// Checks if the required arguments are set.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="MissingFieldException"></exception>
        private void CheckRequirements(Context context)
        {
            // This task requires the following arguments to be set:
            // - QueoTransferUrl
            // - QueoTransferPort
            // - QueoTransferUser
            // - QueoTransferPwd

            if (!context.Arguments.HasArgument("QueoTransferUrl"))
            {
                throw new MissingFieldException("QueoTransferUrl is missing");
            }

            if (!context.Arguments.HasArgument("QueoTransferPort"))
            {
                throw new MissingFieldException("QueoTransferPort is missing");
            }

            if (!context.Arguments.HasArgument("QueoTransferUser"))
            {
                throw new MissingFieldException("QueoTransferUser is missing");
            }

            if (!context.Arguments.HasArgument("QueoTransferPwd"))
            {
                throw new MissingFieldException("QueoTransferPwd is missing");
            }
        }
    }
}
