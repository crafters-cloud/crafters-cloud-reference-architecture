param (
    [string]$SolutionDir,
    [switch]$CleanOnly = $false
)

. ../../scripts/utils.ps1

function Exclude-Directories
{
    param (
        [string[]]$ExcludeDirs
    )
    process
    {
        $allowThrough = $true
        foreach ($directoryToExclude in $ExcludeDirs)
        {
            $directoryText = "*\" + $directoryToExclude
            $childText = "*\" + $directoryToExclude + "\*"
            if (($_.FullName -Like $directoryText -And $_.PsIsContainer) `
                -Or $_.FullName -Like $childText)
            {
                $allowThrough = $false
                break
            }
        }
        if ($allowThrough)
        {
            return $_
        }
    }
}


function Copy-FolderContents
{
    param (
        [string]$SourceDir,
        [string]$DestinationDir,
        [string[]]$ExcludeDirs,
        [switch]$CleanOnly = $false
    )
    
    # check if destination directory exists
    if (Test-Path $DestinationDir)
    {
        Write-Output "Cleaning ${DestinationDir}"
        Remove-Item -Path $DestinationDir -Recurse -Force
    }
    
    if ($CleanOnly -eq $true)
    {
        return
    }

    Write-Output "Copying from ${SourceDir} to ${DestinationDir}"

    # create the destination directory
    New-Item -ItemType Directory -Path $DestinationDir

    # Get all items in the source directory, excluding specified folders
    $items = Get-ChildItem -Path $SourceDir -Recurse | Exclude-Directories -ExcludeDirs $ExcludeDirs
    foreach ($item in $items) {
        $relativePath = $item.FullName.Substring($SourceDir.Length + 1)
        $destinationPath = Join-Path -Path $DestinationDir -ChildPath $relativePath

        if ($item.PSIsContainer) {
            # Create the directory if it doesn't exist
            if (-not (Test-Path $destinationPath)) {
                New-Item -ItemType Directory -Path $destinationPath
            }
        } else {
            # Copy the file
            Copy-Item -Path $item.FullName -Destination $destinationPath
        }
    }
}
# end utilities

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

