@echo off

del delete Feature.csproj

del icon-128x92.png
del update-template-content.ps1

rem clean up the README.md
break>README.md

rem Remove the cleanup script
(goto) 2>nul & del "%~f0"






