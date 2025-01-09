@echo off
dotnet sln remove Feature.csproj
del Feature.csproj

dotnet sln remove Solution.csproj
del Solution.csproj

del icon-128x92.png

rem clean up the README.md
break>README.md

rem Remove the cleanup script
(goto) 2>nul & del "%~f0"







