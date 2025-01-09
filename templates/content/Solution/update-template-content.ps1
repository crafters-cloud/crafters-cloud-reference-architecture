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

# remove \ from the end of the path
if ($SolutionDir.EndsWith("\"))
{
    $SolutionDir = $SolutionDir.Substring(0, $SolutionDir.Length - 1)
}

Write-Output "SolutionDir: ${SolutionDir}"

$ProjectDir = "${SolutionDir}\templates\content\Solution"

# Copy the template content
Copy-FolderContents -SourceDir "${SolutionDir}\scripts" -DestinationDir "${ProjectDir}\scripts" -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\src" -DestinationDir "${ProjectDir}\src" -ExcludeDirs @('bin', 'obj', 'TestResults') -CleanOnly:$CleanOnly
Copy-FolderContents -SourceDir "${SolutionDir}\tests" -DestinationDir "${ProjectDir}\tests" -Exclude @('bin', 'obj', 'TestResults') -CleanOnly:$CleanOnly

# Copy files from the $SolutionDir to the $ProjectDir
Get-ChildItem -Path $SolutionDir -File | ForEach-Object {
    $destinationPath = Join-Path -Path $ProjectDir -ChildPath $_.Name
    Copy-Item -Path $_.FullName -Destination $destinationPath -Force
}



