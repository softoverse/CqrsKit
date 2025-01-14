@REM mkdir <solution-name>
@REM cd <solution-name>

dotnet new sln --name CqrsKit

@REM dotnet new .gitignore
@REM dotnet new .editorconfig
@REM dotnet new tool-manifest
@REM dotnet new global.json
@REM dotnet new buildprops
@REM dotnet new buildtargets
@REM dotnet new packagesprops


@REM mkdir <directory_name>
@REM cd <directory_name>
@REM dotnet new classlib --name <ProjectName>
@REM cd ..
@REM dotnet sln add <directory_name>/<ProjectDirectory>/<ProjectName>.csproj

mkdir src
mkdir tests

@REM src projects
cd src
dotnet new classlib --name CqrsKit
dotnet new classlib --name CqrsKit.Abstraction
dotnet new classlib --name CqrsKit.Model
cd ..
dotnet sln add src/CqrsKit/CqrsKit.csproj
dotnet sln add src/CqrsKit.Abstraction/CqrsKit.Abstraction.csproj
dotnet sln add src/CqrsKit.Model/CqrsKit.Model.csproj

@REM tests projects
cd tests
dotnet new console --name CqrsKit.TestConsole
dotnet new web --name CqrsKit.WebApi
cd ..
dotnet sln add tests/CqrsKit.TestConsole/CqrsKit.TestConsole.csproj
dotnet sln add tests/CqrsKit.WebApi/CqrsKit.WebApi.csproj
