using Build.Tasks;
using Cake.Frosting;

namespace Build {

    /// <summary>
    /// This code defines various flow sequences for the build process of a .NET project. 
    /// Each flow is represented by the "Default" class, which is marked with various dependencies to other classes. 
    /// The flows vary depending on whether the project contains documentation, whether an AsciiDoc is used or whether a NuGet package is to be created. 
    /// Depending on specific requirements, the flows can be adapted and unnecessary lines can be deleted.
    /// </summary>

    //[IsDependentOn(typeof(FormatCheck))]
    //[IsDependentOn(typeof(NugetRestore))]
    //[IsDependentOn(typeof(GenerateVersion))]
    //[IsDependentOn(typeof(Build))]
    //[IsDependentOn(typeof(RunTestsAndPublishResults))]
    //[IsDependentOn(typeof(BuildNuGetPackage))]
    //[IsDependentOn(typeof(GetNuGetPackagesFromArtifacts))]
    //[IsDependentOn(typeof(UploadArtifactsToPipeline))]
    //public partial class Default { }

    [IsDependentOn(typeof(FormatCheck))]
    [IsDependentOn(typeof(NugetRestore))]
    [IsDependentOn(typeof(GenerateVersion))]
    [IsDependentOn(typeof(Build))]
    [IsDependentOn(typeof(RunTestsAndPublishResults))]
    [IsDependentOn(typeof(BuildHtml))]
    [IsDependentOn(typeof(BuildPdf))]
    [IsDependentOn(typeof(PublishApp))]
    [IsDependentOn(typeof(BuildNuGetPackage))]
    [IsDependentOn(typeof(GetNuGetPackagesFromArtifacts))]
    [IsDependentOn(typeof(ZipArtifacts))]
    [IsDependentOn(typeof(ZipDocumentationArtifacts))]
    [IsDependentOn(typeof(PushNuGetPackagesToQueo))]
    [IsDependentOn(typeof(UploadArtifactsToPipeline))]
    [IsDependentOn(typeof(UploadArtifactsToQueoTransfer))]
    public partial class Default { }

    [IsDependentOn(typeof(FormatCheck))]
    [IsDependentOn(typeof(NugetRestore))]
    [IsDependentOn(typeof(GenerateVersion))]
    [IsDependentOn(typeof(Build))]
    [IsDependentOn(typeof(RunTestsAndPublishResults))]
    public partial class BuildAndTest { }

    [IsDependentOn(typeof(NugetRestore))]
    [IsDependentOn(typeof(GenerateVersion))]
    [IsDependentOn(typeof(BuildNuGetPackage))]
    [IsDependentOn(typeof(GetNuGetPackagesFromArtifacts))]
    [IsDependentOn(typeof(UploadArtifactsToPipeline))]
    public partial class BuildPackage { }

    // Flow for the build process of a .NET project with AsciiDoc.
    //[IsDependentOn(typeof(FormatCheck))]
    //[IsDependentOn(typeof(NugetRestore))]
    //[IsDependentOn(typeof(GenerateVersion))]
    //[IsDependentOn(typeof(Build))]
    //[IsDependentOn(typeof(RunTestsAndPublishResults))]
    //[IsDependentOn(typeof(BuildHtml))]
    //[IsDependentOn(typeof(BuildPdf))]
    //[IsDependentOn(typeof(PublishApp))]
    //[IsDependentOn(typeof(ZipArtifacts))]
    //[IsDependentOn(typeof(ZipDocumentationArtifacts))]
    //[IsDependentOn(typeof(UploadArtifactsToPipeline))]
    //public partial class Default { }

    // Flow for Creating a PDF and HTML File from an Asciidoc file without any project.
    //[IsDependentOn(typeof(GenerateVersion))]
    //[IsDependentOn(typeof(BuildHtml))]
    //[IsDependentOn(typeof(BuildPdf))]
    //[IsDependentOn(typeof(ZipDocumentationArtifacts))]
    //[IsDependentOn(typeof(UploadArtifactsToPipeline))]
    //[IsDependentOn(typeof(UploadArtifactsToQueoTransfer))]
    //public partial class Default { }
}
