# import utils.ps1
. ./utils.ps1

$solutionDir = Get-FolderLevelsUp -StartPath $PSScriptRoot -LevelsUp 1
$artifacts = "${solutionDir}\artifacts"

if (Test-Path $artifacts)
{
    Remove-Item $artifacts -Force -Recurse
}

exec { & dotnet clean -c Release ..\CraftersCloud.ReferenceArchitecture.sln }

exec { & dotnet build -c Release ..\CraftersCloud.ReferenceArchitecture.sln }

#exec { & dotnet test -c Release ..\CraftersCloud.ReferenceArchitecture.sln --no-build -l trx --verbosity=normal }

$projects = Get-ChildItem -Path $solutionDir -Recurse -Filter *.csproj

foreach ($project in $projects)
{
    $content = Get-Content $project.FullName
    if ($content -match '<IsPackable>true</IsPackable>')
    {
        exec { & dotnet pack $project.FullName -c Release -o $artifacts --no-build }
    }
}



