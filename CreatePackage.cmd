@echo off
set Apikey=A2SvwIa3g0CpQR2GMcHv35h9nEv0RYbw
set Source=http://nuget.ghcentre.com/api/v2/package

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

REM if exist *.nupkg move *.nupkg "C:\Content\Projects\GHCentre.com\LocalNugetFeed"
if exist *.nupkg (
  for %%i in (*.nupkg) do nuget push %%i %Apikey% -Source %Source% && del %%i
)
exit 0
