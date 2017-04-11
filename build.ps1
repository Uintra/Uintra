$projName = "uCommunity.Core";
$configuration = "Release";
$tagPrefix = "Core";


$nuget = ".\src\.nuget\nuget";
$projFile = Get-ChildItem -Recurse "$projName.csproj";


Invoke-Expression "$nuget pack -Build $projFile -Properties Configuration=$configuration";

$nugetFile = Get-ChildItem "$projName*.nupkg";

if (Test-Path $nugetFile) {
    $nugetsFolder = "C:\inetpub\Nuget\Packages";

    Invoke-Expression "$nuget push $nugetFile -ApiKey !QA2ws3ed -Source http://nuget.compent.dk/ ";

    Remove-Item $nugetFile;

    $assemblyPath = $projFile.DirectoryName + "\bin\$configuration\$projName.dll";
    $version = (Get-ChildItem $assemblyPath | Select-Object -ExpandProperty VersionInfo).FileVersion;

    $tagName = $tagPrefix + "_v." + $version;

    Invoke-Expression "git tag $tagName";
    Invoke-Expression "git push origin $tagName";

     Unblock-File $assemblyPath
     Unblock-File $projFile
}



