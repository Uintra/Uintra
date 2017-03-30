Get-ChildItem *.nuspec -Recurse | Where-Object {$_.Name -match "uCommunity\.([A-Za-z]*)\.nuspec"} | ForEach-Object { 
    # $assemblyInfo = (Get-Content ($_.Directory.FullName + "\Properties\AssemblyInfo.cs"));
    # $version = (($assemblyInfo -match 'AssemblyVersion\(".*"\)') -split ('"'))[1];
    # $title = (($assemblyInfo -match 'AssemblyTitle\(".*"\)') -split ('"'))[1];
    $pathToProjFile = $_.Directory.FullName + "\" + $_.Name;
    Invoke-Expression (".nuget\nuget pack " + $pathToProjFile)
}

