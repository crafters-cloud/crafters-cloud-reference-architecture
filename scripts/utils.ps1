<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
        [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0)
    {
        throw ("Exec: " + $errorMessage)
    }
}

function Get-FolderLevelsUp
{
    param (
        [string]$StartPath,
        [int]$LevelsUp
    )
    $TargetDir = $StartPath
    for ($i = 0; $i -lt $LevelsUp; $i++) {
        $TargetDir = Split-Path -Path $TargetDir -Parent
    }
    return $TargetDir
}

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