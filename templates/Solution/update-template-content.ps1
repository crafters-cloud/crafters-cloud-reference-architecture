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

# Copy the template content
Copy-FolderContents -SourceDir "${SolutionDir}\scripts" -DestinationDir "${SolutionDir}\templates\Solution\scripts" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\src" -DestinationDir "${SolutionDir}\templates\Solution\src" -ExcludeDirs @('bin', 'obj', 'TestResults') -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\tests" -DestinationDir "${SolutionDir}\templates\Solution\tests" -Exclude @('bin', 'obj', 'TestResults') -CleanOnly:$CleanOnly

