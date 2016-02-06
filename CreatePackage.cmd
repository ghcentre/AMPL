@echo off
setlocal enableextensions
if /%1/==// (
  echo Usage: %0 ProjectFolder
  exit 1
)

cd /d "%~1"
nuget pack "%1.csproj"
if errorlevel 1 (
  echo There was an error creating the package.
  exit 1
)

if exist *.nupkg move *.nupkg "C:\Content\Projects\GHCentre.com\LocalNugetFeed"
exit 0
