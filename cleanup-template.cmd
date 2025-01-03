@echo off
dotnet sln remove Template.csproj
del Template.csproj

dotnet sln remove .template.config
del .template.config

dotnet sln remove .github
rd .github /s /q

del icon-128x92.png
del push.ps1
del reinstall-template.ps1

rem clean up the README.md
break>README.md

rem Remove the cleanup script
(goto) 2>nul & del "%~f0"







