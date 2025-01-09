# import utils.ps1
. ./utils.ps1

$solutionDir = Get-FolderLevelsUp -StartPath $PSScriptRoot -LevelsUp 1
$artifacts = "${solutionDir}\artifacts"

try
{
    exec { & dotnet new uninstall CraftersCloud.ReferenceArchitecture.ProjectTemplates }
}
catch
{
    Write-Host "Templates not found"
}

exec { & dotnet new install "$artifacts\CraftersCloud.ReferenceArchitecture.ProjectTemplates.*.nupkg" }



