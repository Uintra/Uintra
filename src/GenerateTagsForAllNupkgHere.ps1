Get-ChildItem *.nupkg | ForEach-Object { 
    $groups = [regex]::Match($_.Name, "uCommunity\.([A-Za-z]*)\.(.*)\.nupkg").captures.groups;
    $name = $groups[1].value;
    $version = $groups[2].value;
    git tag ($name + "_v" + $version);
}
