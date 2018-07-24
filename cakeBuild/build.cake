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
Setup((ctx) =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown((ctx) =>
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

    var solutionFile = GetFiles($"../**/*.sln").SingleOrDefault();
    if(solutionFile == null){
        throw new Exception("Could not find solution file.");
    }

    NuGetRestore(solutionFile);
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
    .Does(() =>
{
    var deploymentRelativePath = @"temp\uIntra\Packages";
    var packagesLocation = isLocalNugetBuild ? "C:/inetpub/Nuget/Packages" : $@"{hetznerWebIp}\{deploymentRelativePath}";
    Information("Copying package to package location...");

    var nugetPackage = GetFiles(project.NugetDirectoryPath + "/*.nupkg").SingleOrDefault();
    if(nugetPackage == null)
    {
        throw new Exception("Could not find nupkg file.");
    }

    CopyFileToDirectory(nugetPackage, packagesLocation);

    if(!isLocalNugetBuild)
    {
        Information($@"Download package by link: \\136.243.176.173\{deploymentRelativePath}\{nugetPackage.GetFilename()}");
    }
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
    var existedTags = GitTags(project.GitDirectory);

    if(existedTags.Exists(t => t.FriendlyName == tag)){
        Information($"Tag {tag} already exists. Deleting tag...");
        StartProcess("git", $"tag -d {tag}");
        StartProcess("git", $"push origin :refs/tags/{tag}");
    }
   
    GitTag(project.GitDirectory, tag);
    StartProcess("git", "push origin " + tag);
});

Task("BuildUmbracoPackage")
    .Description("Builds all the different parts of the project.")
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
                .WithProperty("DeployOnBuild", "true"));

    //CopyDirectory($"{project.Directory}/build", $"{project.DeploymentSourceDirectory}/build");

    var assemblyInfo = ParseAssemblyInfo(project.AssemblyInfoPath);
    var deploymentPackage = $"uIntraPackage.{assemblyInfo.AssemblyVersion}.zip";
    var deploymentPackageFile = new FilePath(deploymentPackage);

    Zip(project.DeploymentSourceDirectory, deploymentPackage);

     var deploymentRelativePath = @"D:\";
    // var deploymentDestination = $@"{hetznerWebIp}\{deploymentRelativePath}";

    CopyFileToDirectory(deploymentPackageFile, deploymentRelativePath);
    DeleteFile(deploymentPackageFile);

    Information($@"Download package by link: {deploymentRelativePath}");
});

Task("umbracoPackageInstaller")
	.Description("Create umbraco package installer")
	.Does(()=>
	{
		var compentUintraProjectName = "Compent.Uintra";
		var compentUintraProject = GetBuildProject(compentUintraProjectName, configuration);
		DeleteDirectoryIfExists(compentUintraProject.DeploymentDirectory.ToString());
		MSBuild(compentUintraProject.File, settings =>
            settings
                .SetPlatformTarget(PlatformTarget.MSIL)
				.UseToolVersion(MSBuildToolVersion.Default)
                .WithTarget("Build")
                .SetConfiguration(configuration)
                .WithProperty("OutDir", compentUintraProject.DeploymentDirectory.ToString())
                .WithProperty("DeployOnBuild", "true"));
		var compentUintraDll = new FilePath($"{compentUintraProject.DeploymentDirectory.FullPath}/{compentUintraProjectName}.dll");		

		var installerProjectName = "Uintra.Installer";
		var installerProject = GetBuildProject(installerProjectName, configuration);
		DeleteDirectoryIfExists(installerProject.DeploymentDirectory.ToString());
		MSBuild(installerProject.File, settings =>
            settings
                .SetPlatformTarget(PlatformTarget.MSIL)
				.UseToolVersion(MSBuildToolVersion.Default)
                .WithTarget("Build")
                .SetConfiguration(configuration)
                .WithProperty("OutDir", installerProject.DeploymentDirectory.ToString())
                .WithProperty("DeployOnBuild", "true"));
		var installerDll = new FilePath($"{installerProject.DeploymentDirectory.FullPath}/{installerProjectName}.dll");		

		var microsoftXdtDll = new FilePath($"{installerProject.DeploymentDirectory.FullPath}/Microsoft.Web.XmlTransform.dll");		
		var packageXmlTemplate = new FilePath($"{installerProject.Directory.FullPath}/package.xml");		
		var app_PluginsWebConfig = new FilePath($"{installerProject.Directory.FullPath}/config/App_Plugins/App_Plugins.Web.config");
		var viewsWebConfigXdt = new FilePath($"{installerProject.Directory.FullPath}/config/Views.Web.config.install.xdt");
		var webConfigXdt = new FilePath($"{installerProject.Directory.FullPath}/config/Web.config.install.xdt");

		var packageSourceDirectory = installerProject.DeploymentSourceDirectory;
		var tempDirectory = new DirectoryPath($"{packageSourceDirectory.FullPath}/{Guid.NewGuid().ToString()}");
		CreateDirectory(tempDirectory);
		CopyFileToDirectory(installerDll, tempDirectory);
		CopyFileToDirectory(microsoftXdtDll, tempDirectory);
		CopyFileToDirectory(packageXmlTemplate, tempDirectory);
		CopyFileToDirectory(compentUintraDll, tempDirectory);
		CopyFileToDirectory(app_PluginsWebConfig, tempDirectory);
		CopyFileToDirectory(viewsWebConfigXdt, tempDirectory);
		CopyFileToDirectory(webConfigXdt, tempDirectory);

		var packageXml = new FilePath($"{tempDirectory.FullPath}/package.xml");
		var assemblyInfo = ParseAssemblyInfo(compentUintraProject.AssemblyInfoPath); //project
		//XmlPoke(packageXml, "/umbPackage/info/package/name", assemblyInfo.Title);
		//XmlPoke(packageXml, "/umbPackage/info/package/version", assemblyInfo.AssemblyVersion);
		//XmlPoke(packageXml, "/umbPackage/info/author/name", assemblyInfo.Company);

		//var packageZipFileName = $"UintraPackageInstaller.{assemblyInfo.AssemblyVersion}.zip";
		var packageZipFileName = $"UintraPackageInstaller.{"0.3.2.0"}.zip";
		var packageZipFile = new FilePath($"{installerProject.DeploymentDirectory.FullPath}/{packageZipFileName}");

		Zip(packageSourceDirectory, packageZipFile);
	});

Task("UpdateManifest")
    .Description("Builds all the different parts of the project.")
    .Does(() =>
{
     Information("Updating manifest");
     
    var assemblyInfo = ParseAssemblyInfo(project.AssemblyInfoPath);

    var packageXml = File($"{project.Directory}/package.xml");

    XmlPoke(packageXml, "/umbPackage/info/package/name", assemblyInfo.Title);
    XmlPoke(packageXml, "/umbPackage/info/package/version", assemblyInfo.AssemblyVersion);
    XmlPoke(packageXml, "/umbPackage/info/author/name", assemblyInfo.Company);
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
    .IsDependentOn("Copy-Package-To-Gallery")
    .IsDependentOn("Add-Git-Tag");

Task(ManualInstallationPackageTargetKey)
    .Description("This is the ManualInstallationPackage task which creates uIntra for manual installation")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGet-Restore-Packages")
    .IsDependentOn("Npm-Install")
    .IsDependentOn("Webpack")
    .IsDependentOn("DeployOnBuild");

Task(UmbracoPackageTargetKey)
    .Description("This is the UmbracoPackage task which creates umbraco package")
    //.IsDependentOn("Clean")
    // .IsDependentOn("NuGet-Restore-Packages")
    // .IsDependentOn("Npm-Install")
    // .IsDependentOn("Webpack")
    //.IsDependentOn("BuildUmbracoPackage")
    //.IsDependentOn("UpdateManifest")
	.IsDependentOn("umbracoPackageInstaller");

// EXECUTION
RunTarget(target);