param (
    [string]$SolutionDir,
    [switch]$CleanOnly = $false
)

. ../../../tools/utils.ps1

# get the solution directory
if (-not $SolutionDir)
{
    $SolutionDir = Get-FolderLevelsUp -StartPath $PSScriptRoot -LevelsUp 3
}

# remove trailing directory separator from the end of the path
$SolutionDir = $SolutionDir.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)

Write-Output "SolutionDir: ${SolutionDir}"

$ProjectDir = "${SolutionDir}/templates/content/Feature"

# Copy the template content
Copy-FolderContents -SourceDir "${SolutionDir}/src/Api/Endpoints/Products" -DestinationDir "${ProjectDir}/src/Api/Endpoints/Products" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}/src/Domain/Products" -DestinationDir "${ProjectDir}/src/Domain/Products" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}/src/Infrastructure/Data/Configurations" -DestinationDir "${ProjectDir}/src/Infrastructure/Data/Configurations" -Filter "Product*" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}/src/Migrations/Seeding" -DestinationDir "${ProjectDir}/src/Migrations/Seeding" -Filter "Product*" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}/tests/Api.Tests/Endpoints" -DestinationDir "${ProjectDir}/tests/Api.Tests/Endpoints" -Filter "Product*.cs" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}/tests/Domain.Tests/Products" -DestinationDir "${ProjectDir}/tests/Domain.Tests/Products" -Filter "Product*" -CleanOnly:$CleanOnly

