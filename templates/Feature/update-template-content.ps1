param (
    [string]$SolutionDir,
    [switch]$CleanOnly = $false
)

. ../../tools/utils.ps1

# get the solution directory
if (-not $SolutionDir)
{
    $SolutionDir = Get-FolderLevelsUp -StartPath $PSScriptRoot -LevelsUp 2
}

# remove \ from the end of the path
if ($SolutionDir.EndsWith("\"))
{
    $SolutionDir = $SolutionDir.Substring(0, $SolutionDir.Length - 1)
}

Write-Output "SolutionDir: ${SolutionDir}"

$ProjectDir = "${SolutionDir}\templates\Feature"

# Copy the template content
Copy-FolderContents -SourceDir "${SolutionDir}\src\Api\Endpoints\Users" -DestinationDir "${ProjectDir}\src\Api\Endpoints\Users" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\src\Domain\Users" -DestinationDir "${ProjectDir}\src\Domain\Users" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\src\Infrastructure\Data\Configurations" -DestinationDir "${ProjectDir}\src\Infrastructure\Data\Configurations" -Filter "User*" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\src\Migrations\Seeding" -DestinationDir "${ProjectDir}\src\Migrations\Seeding" -Filter "User*" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\tests\Api.Tests\Endpoints" -DestinationDir "${ProjectDir}\tests\Api.Tests\Endpoints" -Filter "User*.cs" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\tests\Domain.Tests\Users" -DestinationDir "${ProjectDir}\tests\Domain.Tests\Users" -Filter "User*" -CleanOnly:$CleanOnly

