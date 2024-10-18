@echo off
dotnet sln remove Template.csproj
del Template.csproj
(goto) 2>nul & del "%~f0"







