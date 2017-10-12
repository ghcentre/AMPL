@echo off
path C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin;%PATH%
set Apikey=A2SvwIa3g0CpQR2GMcHv35h9nEv0RYbw
set Source=http://nuget.ghcentre.com/api/v2/package

setlocal enableextensions
if /%1/==// (
  echo Usage: %0 ProjectFolder
  exit 1
)

cd /d "%~1"

msbuild
if errorlevel 1 (
  echo There was an error building the project '%1'.
  exit 1
)

if exist "%1.nuspec" (
  nuget pack "%1.csproj"
  if errorlevel 1 (
    echo There was an error creating the package.
    exit 1
  )
)


if exist *.nupkg (
  for %%i in (*.nupkg) do nuget push %%i %Apikey% -Source %Source% && del %%i
)

if exist bin\debug\*.nupkg (
  for %%i in (bin\debug\*.nupkg) do nuget push %%i %Apikey% -Source %Source% && del %%i
)
exit 0
