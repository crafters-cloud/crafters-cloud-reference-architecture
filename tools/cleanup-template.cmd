@echo off
dotnet sln remove Template.csproj
(goto) 2>nul & del "%~f0"
