#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Npm"
#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Git"
#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Webpack"

// ARGUMENTS
var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");

// GLOBAL VARIABLES
var packagesLocation = "C:/inetpub/Nuget/Packages";
var uIntraProjectFileName = "uIntra.csproj";
var compentUintraProjectFileName = "Compent.uIntra.csproj";

var uIntraProject = GetFiles("../**/" + uIntraProjectFileName).SingleOrDefault();
if(uIntraProject == null){
     throw new Exception("Could not find project file - " + uIntraProjectFileName);
}
var uIntraProjectPath = uIntraProject.GetDirectory();

var compentUintraProject = GetFiles("../**/" + compentUintraProjectFileName).SingleOrDefault();
if(compentUintraProject == null){
     throw new Exception("Could not find project file - " + compentUintraProjectFileName);
}
var compentUintraProjectPath = compentUintraProject.GetDirectory();

var gitRootPath = GitFindRootFromPath(uIntraProjectPath);
var nugetPackageDir = uIntraProjectPath + "/bin/" + configuration;

// SETUP / TEARDOWN
Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

// TASK DEFINITIONS
Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
    Information("Cleaning {0}", uIntraProjectPath);

    CleanDirectories(uIntraProjectPath + "/**/bin/" + configuration);
    CleanDirectories(uIntraProjectPath + "/**/obj/" + configuration); 
});

Task("NuGet-Restore-Packages")
    .Description("Restores all the NuGet packages that are used by the specified project.")
    .Does(() =>
{
    Information("Restoring {0}...", uIntraProjectPath);

    var nugetConfig = GetFiles("../**/NuGet.Config").SingleOrDefault();
    if(nugetConfig == null){
        throw new Exception("Can't find nuget.config.");
    }

    var nuGetRestoreSettings = new NuGetRestoreSettings { ConfigFile  = nugetConfig };
    NuGetRestore(uIntraProject, nuGetRestoreSettings);
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .Does(() =>
{
     Information("Building {0}", uIntraProject);
        MSBuild(uIntraProject, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithTarget("Build")
                .SetConfiguration(configuration));
});

Task("Npm-Install")
    .Does(() =>
{
    Information("Installing NPM packages...");

    var installSettings = new NpmInstallSettings();
    installSettings.LogLevel = NpmLogLevel.Info;
    installSettings.WorkingDirectory = compentUintraProjectPath;

    NpmInstall(installSettings);
});

Task("Webpack")
    .Does(() =>
{
    Information("Running webpack...");
    Webpack
        .FromPath(compentUintraProjectPath)
        .Global(s => s.WithArguments("--optimize-minimize"));     
});

Task("NuGet-Pack")
    .Does(() =>
{
    Information("Packing nuget package ...");

    var nuGetPackSettings = new NuGetPackSettings 
    {
        OutputDirectory = nugetPackageDir,
        ArgumentCustomization = args => args.Append("-Prop Configuration=" + configuration)
    };

    NuGetPack(uIntraProjectPath + "/" + uIntraProjectFileName, nuGetPackSettings);
});

Task("Copy-Package-To-Packages-Location")
    .Does(() =>
{
    Information("Copying package to package location...");

    var nugetPackage = GetFiles(nugetPackageDir + "/*.nupkg").SingleOrDefault();
    if(nugetPackage == null)
    {
        throw new Exception("Could not find nupkg file.");
    }

    CopyFileToDirectory(nugetPackage, packagesLocation);
});

Task("Add-Git-Tag")
    .Does(() =>
{
    Information("Adding git tag...");

    var nugetPackage = GetFiles(nugetPackageDir + "/*.nupkg").SingleOrDefault();
    if(nugetPackage == null)
    {
         throw new Exception("Could not find nupkg file.");
    }

    var tag = nugetPackage.GetFilenameWithoutExtension().ToString();
    GitTag(gitRootPath, tag);
    StartProcess("git", "push origin " + tag);
});

// TARGETS
Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet-Restore-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Npm-Install")
    .IsDependentOn("Webpack")
    .IsDependentOn("NuGet-Pack")
    .IsDependentOn("Copy-Package-To-Packages-Location")
    .IsDependentOn("Add-Git-Tag");

// EXECUTION
RunTarget(target);