using System.Diagnostics;
using System.IO;

using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace Build
{
    /// <summary>
    /// Task for generating HTML documentation from AsciiDoc files.
    /// If you want to include this task in your build flow you need to set following fields beside the General field in your context: Doc.
    /// </summary>
    [TaskName("BuildHtml")]
    public class BuildHtml : FrostingTask<Context>
    {
        /// <summary>
        /// Runs the task to generate HTML documentation from AsciiDoc files.
        /// </summary>
        /// <param name="context"></param>
        public override void Run(Context context)
        {
            // AsciiDoctor is a Ruby gem that converts AsciiDoc files to HTML.
            // It needs to be installed on the machine to generate HTML documentation.
            if (!AsciiDoctorIsInstalled())
            {
                context.Information("AsciiDoctor is not installed.");
            }
            else
            {
                // Get the root file path, generated path, and name from the context.
                string rootFilePath = context.Doc.RootFilePath;
                string generatedPath = context.Doc.GeneratedPath;
                string name = context.General.Name;

                Process process = Process.Start(
                @"powershell",
                $@"asciidoctor -r asciidoctor-diagram -b html5 {rootFilePath} -o {generatedPath}{Path.DirectorySeparatorChar}{name}.html");
                process?.WaitForExit();
            }
        }

        /// <summary>
        /// Returns true if AsciiDoctor is installed on the machine; otherwise, false.
        /// </summary>
        private bool AsciiDoctorIsInstalled()
        {
            Process process = Process.Start(
                @"powershell",
                "asciidoctor --version");
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}
