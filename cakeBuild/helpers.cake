const string DefaultTargetKey = "Default";
const string ManualInstallationPackageTargetKey = "ManualInstallation";

const string uIntraProject = "uIntra";
const string CompentUintraProjectFileName = "Compent.uIntra";

public class BuildProject
{
    public FilePath File { get; set; }

    public DirectoryPath Directory { get; set; } 

    public DirectoryPath GitDirectory { get; set; }

    public DirectoryPath DeploymentDirectory { get; set; }

    public DirectoryPath DeploymentSourceDirectory { get; set; }

    public string NugetDirectoryPath { get; set; }

    public string AssemblyInfoPath { get; set; }
}

BuildProject GetBuildProject(string projectName, string configuration){
    var projectFile = GetFiles($"../**/{projectName}.csproj").SingleOrDefault();
    if(projectFile == null){
         throw new Exception("Could not find project file - " + projectName);
    }

    var directory = projectFile.GetDirectory();
    var deploymentDirectory = new DirectoryPath($"{directory}/bin/Deployment");

    return new BuildProject(){
        File = projectFile,
        Directory = directory,
        GitDirectory = GitFindRootFromPath(directory),
        NugetDirectoryPath = $"{directory}/bin/{configuration}",
        DeploymentDirectory = deploymentDirectory,
        DeploymentSourceDirectory = new DirectoryPath($"{deploymentDirectory}/_PublishedWebsites/{projectName}"),
        AssemblyInfoPath = $"{directory}/Properties/AssemblyInfo.cs"
    };
}

string GetProjectName(string target){
    switch (target)
    {
        case DefaultTargetKey:
            return uIntraProject;
        case ManualInstallationPackageTargetKey:
            return CompentUintraProjectFileName;
        default: 
            throw new Exception($"Can not find project for target: {target}");
    }
}

void DeleteDirectoryIfExists(string path, DeleteDirectorySettings settings = null){
    if(settings == null){
        settings  = new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        };
    }

    if(DirectoryExists(path)){
        DeleteDirectory(path, settings);
    }
}