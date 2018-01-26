#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Npm"
#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Git"
#addin "nuget:https://www.nuget.org/api/v2/?package=Cake.Webpack"
#load helpers.cake

// ARGUMENTS
var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");

// GLOBAL VARIABLES
var isLocalNugetBuild = target == DefaultTargetKey && configuration == "Release";
var hetznerWebIp = @"\\192.168.200.2";

string projectName = GetProjectName(target);

Information($"Project name is {projectName}");
Information($"Configuration is {configuration}");

var project = GetBuildProject(projectName, configuration);
var compentUIntraProject = GetBuildProject(CompentUintraProjectFileName, configuration);

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
    Information("Cleaning {0}", project.Directory);

    CleanDirectories($"{project.Directory}/**/bin/{configuration}");
    CleanDirectories($"{project.Directory}/**/obj/{configuration}");

    DeleteDirectoryIfExists($"{project.Directory}/umbraco");
    DeleteDirectoryIfExists($"{project.Directory}/umbraco_client");
});

Task("NuGet-Restore-Packages")
    .Description("Restores all the NuGet packages that are used by the specified project.")
    .Does(() =>
{
    Information("Restoring {0}...", project.Directory);

    var nugetConfig = GetFiles("../**/NuGet.Config").SingleOrDefault();
    if(nugetConfig == null){
        throw new Exception("Can't find nuget.config.");
    }

    var nuGetRestoreSettings = new NuGetRestoreSettings { ConfigFile  = nugetConfig };
    NuGetRestore(project.File, nuGetRestoreSettings);
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .Does(() =>
{
    Information("Building {0}", project.File);
        MSBuild(project.File, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
					.UseToolVersion(MSBuildToolVersion.Default)
                .WithTarget("Build")
                .SetConfiguration(configuration));
});

Task("DeployOnBuild")
    .Description("Build and deploy")
    .Does(() => 
{
    Information("Build and deploy {0}", project.File);

    DeleteDirectoryIfExists(project.DeploymentDirectory.ToString());

    MSBuild(project.File, settings =>
            settings
                .SetPlatformTarget(PlatformTarget.MSIL)
				.UseToolVersion(MSBuildToolVersion.Default)
                .WithTarget("Build")
                .SetConfiguration(configuration)
                .WithProperty("OutDir", project.DeploymentDirectory.ToString())
                .WithProperty("DeployOnBuild", "true")
                );

    CopyDirectory($"{project.Directory}/build", $"{project.DeploymentSourceDirectory}/build");
    CopyDirectory($"{project.Directory}/umbraco", $"{project.DeploymentSourceDirectory}/umbraco");
    CopyDirectory($"{project.Directory}/umbraco_client", $"{project.DeploymentSourceDirectory}/umbraco_client");

    var assemblyInfo = ParseAssemblyInfo(project.AssemblyInfoPath);
    var deploymentPackage = $"uIntra.{assemblyInfo.AssemblyVersion}.zip";
    var deploymentPackageFile = new FilePath(deploymentPackage);

    Zip(project.DeploymentSourceDirectory, deploymentPackage);

    var deploymentRelativePath = @"temp\uIntra\ManualInstallation";
    var deploymentDestination = $@"{hetznerWebIp}\{deploymentRelativePath}";

    CopyFileToDirectory(deploymentPackageFile, deploymentDestination);
    DeleteFile(deploymentPackageFile);

    Information($@"Download package by link: \\136.243.176.173\{deploymentRelativePath}\{deploymentPackage}");
});

Task("Npm-Install")
    .Does(() =>
{
    Information("Installing NPM packages...");

    var installSettings = new NpmInstallSettings();
    installSettings.LogLevel = NpmLogLevel.Info;
    installSettings.WorkingDirectory = compentUIntraProject.Directory;

    NpmInstall(installSettings);
});

Task("Webpack")
    .Does(() =>
{
    Information("Running webpack...");
    DeleteDirectoryIfExists($"{project.Directory}/build");
    Webpack
        .FromPath(compentUIntraProject.Directory)
        .Global(s => s.WithArguments("--optimize-minimize"));     
});

Task("NuGet-Pack")
    .Does(() =>
{
    Information("Packing nuget package ...");

    var nuGetPackSettings = new NuGetPackSettings 
    {
        OutputDirectory = project.NugetDirectoryPath,
        ArgumentCustomization = args => args.Append("-Prop Configuration=" + configuration)
    };
    
    NuGetPack(project.File, nuGetPackSettings);
});

Task("Copy-Package-To-Gallery")
    .WithCriteria(isLocalNugetBuild)
    .Does(() =>
{
    var packagesLocation = "C:/inetpub/Nuget/Packages";
    Information("Copying package to package location...");

    var nugetPackage = GetFiles(project.NugetDirectoryPath + "/*.nupkg").SingleOrDefault();
    if(nugetPackage == null)
    {
        throw new Exception("Could not find nupkg file.");
    }

    CopyFileToDirectory(nugetPackage, packagesLocation);
});

Task("Copy-Package-To-Hetzner")
    .WithCriteria(!isLocalNugetBuild)
    .Does(() =>
{
    Information("Zipping package to package location...");
    var deploymentRelativePath = @"temp\uIntra\Packages";
    var packagesLocation = $@"{hetznerWebIp}\{deploymentRelativePath}";
    var assemblyInfo = ParseAssemblyInfo(project.AssemblyInfoPath);
    var deploymentPackage = $"uIntraPackages.{assemblyInfo.AssemblyVersion}.zip";
    var deploymentPackageFile = new FilePath(deploymentPackage);

    Zip(project.NugetDirectoryPath, deploymentPackage, "./*.nupkg");

    Information("Copying package to package location...");
    CopyFileToDirectory(deploymentPackageFile, packagesLocation);
    DeleteFile(deploymentPackageFile);

     Information($@"Download package by link: \\136.243.176.173\{deploymentRelativePath}\{deploymentPackage}");
});

Task("Add-Git-Tag")
    .WithCriteria(isLocalNugetBuild)
    .Does(() =>
{
    Information("Adding git tag...");

    var nugetPackage = GetFiles(project.NugetDirectoryPath + "/*.nupkg").SingleOrDefault();
    if(nugetPackage == null)
    {
         throw new Exception("Could not find nupkg file.");
    }

    var tag = nugetPackage.GetFilenameWithoutExtension().ToString();
    GitTag(project.GitDirectory, tag);
    StartProcess("git", "push origin " + tag);
});

// TARGETS
Task(DefaultTargetKey)
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet-Restore-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Npm-Install")
    .IsDependentOn("Webpack")
    .IsDependentOn("NuGet-Pack")
    .IsDependentOn("Copy-Package-To-Hetzner")
    .IsDependentOn("Copy-Package-To-Gallery")
    .IsDependentOn("Add-Git-Tag");

Task(ManualInstallationPackageTargetKey)
    .Description("This is the ManualInstallationPackage task which creates uIntra for manual installation")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet-Restore-Packages")
    .IsDependentOn("Npm-Install")
    .IsDependentOn("Webpack")
    .IsDependentOn("DeployOnBuild");

// EXECUTION
RunTarget(target);