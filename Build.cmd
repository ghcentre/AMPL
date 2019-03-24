@echo off
setlocal enableextensions enabledelayedexpansion
call :Variables

if /%1/==// (call :Help & exit /b 1)
if /%1/==/-all/ (call :BuildAll & exit %ErrorLevel%)

if /%2/==// (
    call :Build %1
    exit /b %ErrorLevel%
) else (
    call :Build %2 %1
    exit /b %ErrorLevel%
)

:BuildAll
    rem
    rem Builds all projects.
    rem
    rem In:
    rem     (none)
    rem Out:
    rem     Errorlevel 0    -- success
    rem     Errorlevel 1    -- failure
    rem    
    setlocal
        call :ProjectList
        for %%i in (%Projects%) do (
            call :Build %%i
            if errorlevel 1 (
                echo [BuildAll] ERROR: Error building '%%i'.
                endlocal & exit /b 1
            )
        )
    endlocal & exit /b 0

:Build
    rem
    rem Builds single project.
    rem
    rem In:
    rem     %1 (required)   -- Project name without extension
    rem     %2 (optional)   -- One of:
    rem                             (empty) -- build, pack, push
    rem                             -d      -- build, pack, push this and dependent
    rem                             -b      -- build this only
    rem                             -p      -- pack, push this only
    rem Out:
    rem     Errorlevel 0    -- Build succeeded
    rem     Errorlevel 1    -- Build failed
    rem
    setlocal
        set Dependent=
        set Build=1
        set Pack=1
        set Project=%1.csproj
        set EL=0

        if /%2/==/-d/ (set Dependent=1)
        if /%2/==/-b/ (set Pack=)
        if /%2/==/-p/ (set Build=)

        echo [Build] Building '%1'
        echo [Build] Version: %Version%
        if /%Dependent%/==/1/ (echo [Build] Options: Dependent)
        if /%Build%/==/1/     (echo [Build] Options: Build)
        if /%Pack%/==/1/      (echo [Build] Options: Pack)

        if not exist %1\nul (
            echo [Build] ERROR: Project directory '%1' does not exist.
            set EL=1 & goto Build_End
        )
        if not exist %1\%Project% (
            echo [Build] ERROR: %1\%Project% does not exist.
            set EL=1 & goto Build_End
        )

        pushd %1

        call :GetProjectType %Project%

        if /%Build%/==/1/ (
            call :UpdatePackages %Project% %ProjectType%
            if errorlevel 1 (set EL=1 & goto Build_End)

            call :BuildProject %Project% %ProjectType%
            if errorlevel 1 (set EL=1 & goto Build_End)
        )

        if /%Pack%/==/1/ (
            call :PackProject %Project% %ProjectType%
            if errorlevel 1 (set EL=1 & goto Build_End)
        )

        popd

        if /%Dependent%/==/1/ (
            call :ProjectList %1
            if not /!Projects!/==// (
                echo [Build] Building dependent projects: !Projects!
                call :BuildDependentProjects !Projects!
                if errorlevel 1 (set EL=1 & goto Build_End)
            )
        )

    :Build_End
    endlocal & exit /b %EL%

:BuildDependentProjects
    rem
    rem Builds, packs and pushes dependent projects
    rem 
    rem In:
    rem     %* (required)   -- Dependent projects
    rem Out:
    rem     Errorlevel 0    -- Success
    rem     Errorlevel 1    -- At least one project failed to build, pack, or push
    setlocal
        set EL=0

        :BuildDependentProjects_Loop
        if /%1/==// (
            goto BuildDependentProjects_End
        )

        echo [BuildDependentProjects] Building dependent project: %1
        call :Build %1
        if errorlevel 1 (
            echo [BuildDependentProjects] ERROR: Error building dependent project: %1
            set EL=1
            goto BuildDependentProjects_End
        )

        shift
        goto BuildDependentProjects_Loop

    :BuildDependentProjects_End
    endlocal & exit /b %EL%

:PackProject
    rem
    rem Packs and pushes the project.
    rem 
    rem In:
    rem     %1 (required)   -- Project name with extension
    rem     %2 (required)   -- ProjectType, one of:
    rem                             core    -- .NET Standard or .NET Core
    rem                             net     -- .NET Framework
    rem Out:
    rem     Errorlevel 0    -- Success
    rem     Errorlevel 1    -- Failure
    rem
    setlocal
        for %%i in (*.nupkg) do del %%i

        echo [PackProject] Packing '%1'.
        nuget pack %1
        if errorlevel 1 (
            echo [PackProject] ERROR: Error packing '%1'.
            endlocal & exit /b 1
        )

        for %%i in (*.nupkg) do (
            echo [PackProject] Pushing '%%i'.
            nuget push %%i %Apikey% -Source %Source%
            if errorlevel 1 (
                echo [PackProject] ERROR: Error pushing '%%i'.
                endlocal & exit /b 1
            )
            del %%i
        )
    endlocal & exit /b 0

:BuildProject
    rem
    rem Builds the project.
    rem 
    rem In:
    rem     %1 (required)   -- Project name with extension
    rem     %2 (required)   -- ProjectType, one of:
    rem                             core    -- .NET Standard or .NET Core
    rem                             net     -- .NET Framework
    rem Out:
    rem     Errorlevel 0    -- Success
    rem     Errorlevel 1    -- Failure
    rem
    setlocal
        echo [BuildProject] Cleaning '%1'.
        msbuild /t:clean %MsbuildParams%
        if errorlevel 1 (
            echo [BuildProject] ERROR: Error cleaning '%1'.
            endlocal & exit /b 1
        )
        
        if /%2/==/net/ (
            echo [BuildProject] Building ^(net^) '%1'.
            msbuild /t:build /p:VersionAssembly=%Version% %MsbuildParams%
            if errorlevel 1 (
                echo [BuildProject] ERROR: Error building ^(net^) '%1'.
                endlocal & exit /b 1
            )
        )

        if /%2/==/core/ (
            echo [BuildProject] Building ^(core^) '%1'.
            msbuild /t:build /p:Version=%Version% /p:AssemblyVersion=%Version% /p:FileVersion=%Version% %MsbuildParams%
            if errorlevel 1 (
                echo [BuildProject] ERROR: Error building ^(core^) for '%1'.
                endlocal & exit /b 1
            )
        )
    endlocal & exit /b 0

:UpdatePackages
    rem
    rem Updates packages for the project.
    rem 
    rem In:
    rem     %1 (required)   -- Project name with extension
    rem     %2 (required)   -- ProjectType, one of:
    rem                             core    -- .NET Standard or .NET Core
    rem                             net     -- .NET Framework
    rem Out:
    rem     Errorlevel 0    -- Success
    rem     Errorlevel 1    -- Failure
    rem
    setlocal
        if /%2/==/net/ (
            echo [UpdatePackages] Updating packages ^(net^) for '%1'.
            nuget update %1
            if errorlevel 1 (
                echo [UpdatePackages] ERROR: Error updating packages ^(net^) for '%1'.
                endlocal & exit /b 1
            )
        )

        if /%2/==/core/ (
            echo [UpdatePackages] Updating packages ^(core^) for '%1'.
            for /f "usebackq tokens=1,2,3,4,5,* delims= " %%i in (`dotnet list package --outdated ^| find ">"`) do (
                dotnet add package %%j --version %%m
                if errorlevel 1 (
                    echo [UpdatePackages] ERROR: Error updating packages ^(core^) for '%1', package '%%j'.
                    endlocal & exit /b 1
                )
            )
        )
    endlocal & exit /b 0

:GetProjectType
    rem
    rem Determines project type.
    rem 
    rem In:
    rem     %1 (required)   -- Project name with extension
    rem Out:
    rem     ProjectType     -- One of:
    rem                             core    -- .NET Standard or .NET Core
    rem                             net     -- .NET Framework
    rem
    setlocal
    type %1|find "Project Sdk=""Microsoft.NET.Sdk""">nul
    if errorlevel 1 (
        endlocal & (set ProjectType=net) & exit /b 0
    )
    endlocal & (set ProjectType=core) & exit /b 0

:Variables
    rem
    rem Sets variables.
    rem

    path "%~dp0";C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin;%PATH%
    set Apikey=A2SvwIa3g0CpQR2GMcHv35h9nEv0RYbw
    set Source=https://nuget.ghcentre.com/api/v2/package

    set VersionFile=%~sdp0version.txt
    for /f "eol=# tokens=* delims=" %%i in (%VersionFile%) do set Version=%%i

    set MsbuildParamsFile=%~sdp0msbuildparams.txt
    set MsbuildParams=
    for /f "delims=" %%i in (%MsbuildParamsFile%) do set MsbuildParams=%MsbuildParams% %%i

    set ProjectListFile=%~sdp0projects.txt
    exit /b 0

:Help
    rem
    rem Prints help text.
    rem
    echo:
    echo AMPL Build script.
    echo:
    echo Copyright 2014-2019 George Harder's Centre (GHCentre) -- https://ghcentre.com
    echo:
    echo Usage:
    echo   Build -all            Builds, packs and pushes all packages
    echo   Build -d Package      Builds, packs and pushes single package and all its
    echo                         depencencies
    echo   Build Package         Builds, packs and pushes single package
    echo   Build -b Package      Builds single package
    echo   Build -p Package      Packs and pushes single package (no build)
    echo:
    echo Current version: %Version%
    echo Package List:
    call :ProjectList
    for %%i in (%Projects%) do echo     %%i
    exit /b 0

:ProjectList
    rem
    rem Returns list of dependent projects.
    rem
    rem In:
    rem     %1 (optional)   -- Project name to list dependencies of.
    rem                        Lists all projects if empty.
    rem Out:
    rem     Projects        -- List of projects
    rem
    setlocal
        set Proj=
        if /%1/==// (
            for /f "eol=# tokens=1,2,* delims= " %%i in (%ProjectListFile%) do set Proj=!Proj! %%i
        ) else (
            for /f "eol=# tokens=1,2,* delims= " %%i in (%ProjectListFile%) do (
                call :ContainsDependency %%i %1
                if errorlevel 1 (
                    set Proj=!Proj! %%i
                )
            )
        )
    endlocal & (set Projects=%Proj%) & (exit /b 0)

:ContainsDependency
    rem
    rem Determines whether Project1 depends on Project2
    rem
    rem In:
    rem     %1 (required)   -- Project1
    rem     %2 (required)   -- Project2
    rem Out:
    rem     Errorlevel 0    -- Project1 does NOT depend on Project2
    rem     Errorlevel 1    -- Project1 DOES depend on Project2
    setlocal
        call :DepsList %1
        for %%i in (%Dependencies%) do (
            if /%%i/==/%2/ (
                endlocal & exit /b 1
            )
        )
    endlocal & exit /b 0

:DepsList
    rem
    rem Returns list of dependencies for the project.
    rem
    rem In:
    rem     %1 (required)   -- Project name
    rem Out:
    rem     Dependencies    -- List of dependencies
    rem
    setlocal
        set Deps=
        for /f "eol=# tokens=1,2,* delims= " %%i in (%ProjectListFile%) do (
            if /%%i/==/%1/ (
                endlocal & (set Dependencies=%%k) & (exit /b 0)
            )
        )
    endlocal & (set Dependencies=) & (exit /b 0)

