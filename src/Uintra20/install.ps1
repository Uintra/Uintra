param($installPath, $toolsPath, $package, $project)

Write-Host "installPath:" "${installPath}"
Write-Host "toolsPath:" "${toolsPath}"
Write-Host " "

if ($project) {
	$dateTime = Get-Date -Format yyyyMMdd-HHmmss
	$projectPath = (Get-Item $project.Properties.Item("FullPath").Value).FullName
	$contentFolder = Join-Path $projectPath "Content"
	New-Item -ItemType Directory -Force -Path $umbracoFolder	
	$contentFolderSource = Join-Path $installPath "UintraFiles\Content"		
    Write-Host "copying files to $umbracoFolder ..."
    robocopy $contentFolderSource $contentFolder /is /it /e
    if (($lastexitcode -eq 1) -or ($lastexitcode -eq 3) -or ($lastexitcode -eq 5) -or ($lastexitcode -eq 7))
    {
        write-host "Copy succeeded!"
    }
    else
    {
        write-host "Copy failed with exit code:" $lastexitcode
    }
}
