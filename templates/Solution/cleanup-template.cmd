@echo off
dotnet sln remove templates\Feature\Feature.csproj
del templates\Feature\Feature.csproj

dotnet sln remove templates\Solution\Solution.csproj
del templates\Solution\Solution.csproj

dotnet sln remove templates

del icon-128x92.png
del update-template-content.ps1

rem clean up the README.md
break>README.md

rem Remove the cleanup script
(goto) 2>nul & del "%~f0"







