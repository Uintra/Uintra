# $projName = "Uintra.Core";
# $configuration = "Release";
# $tagPrefix = "Core";

param 
( 
    [string]$projName = "",
    [string]$configuration = "",
    [string]$tagPrefix = "",
    [string]$packagesLocation = "C:\inetpub\Nuget\Packages"
)

if([string]::IsNullOrEmpty($projName)){
    throw [System.ArgumentException] "Invalid project name!"
}

if([string]::IsNullOrEmpty($configuration)){
    throw [System.ArgumentException] "Invalid configuration name!"
}

if([string]::IsNullOrEmpty($tagPrefix)){
    throw [System.ArgumentException] "Invalid tag prefix!"
}

$nuget = ".\src\.nuget\NuGet.exe";

if(-not (Test-Path $nuget)){
    throw [System.IO.FileNotFoundException] "Can't find nuget.exe. Should be in $nuget";
}

$nugetConfig = ".\src\.nuget\NuGet.Config";

if(-not (Test-Path $nugetConfig)){
    throw [System.IO.FileNotFoundException] "Can't find nuget.config. Should be in $nugetConfig";
}

$projFile = Get-ChildItem -Recurse "$projName.csproj";

if(-not (Test-Path $projFile)){
     throw [System.IO.FileNotFoundException] "Could not find project file - '$projName'";
}

Invoke-Expression "$nuget restore '$projFile' -ConfigFile '$nugetConfig'";

Invoke-Expression "$nuget pack -Build '$projFile' -Properties Configuration=$configuration";

$nugetFile = Get-ChildItem "$projName*.nupkg";

if (Test-Path $nugetFile) {
    #Using copy instead of nuget push because can't authorize even with api key
    Copy-Item $nugetFile $packagesLocation -Force

    Remove-Item $nugetFile;

    $assemblyPath = $projFile.DirectoryName + "\bin\$configuration\$projName.dll";
    $version = (Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo).FileVersion;

    $tagName = $tagPrefix + "_v." + $version;

    Invoke-Expression "git tag $tagName";
    Invoke-Expression "git push origin $tagName";

     Unblock-File $assemblyPath
     Unblock-File $projFile
}