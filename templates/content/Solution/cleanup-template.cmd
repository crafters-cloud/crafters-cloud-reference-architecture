@echo off

dotnet sln remove templates\ProjectTemplates.csproj
del templates\ProjectTemplates.csproj

dotnet sln remove templates\content\Feature\Feature.csproj
del templates\content\Feature\Feature.csproj

dotnet sln remove templates\content\Solution\Solution.csproj
del templates\content\Solution\Solution.csproj

dotnet sln remove workflows
dotnet sln remove templates

del icon-128x92.png
del update-template-content.ps1

rem Remove the cleanup script
(goto) 2>nul & del "%~f0"







