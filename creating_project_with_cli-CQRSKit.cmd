@REM mkdir <solution-name>
@REM cd <solution-name>

dotnet new sln --name Softoverse.CqrsKit

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
dotnet new classlib --name Softoverse.CqrsKit
dotnet new classlib --name Softoverse.CqrsKit.Abstraction
dotnet new classlib --name Softoverse.CqrsKit.Model
cd ..
dotnet sln add src/Softoverse.CqrsKit/Softoverse.CqrsKit.csproj
dotnet sln add src/Softoverse.CqrsKit.Abstraction/Softoverse.CqrsKit.Abstraction.csproj
dotnet sln add src/Softoverse.CqrsKit.Model/Softoverse.CqrsKit.Model.csproj

@REM tests projects
cd tests

mkdir console
cd console
dotnet new console --name Softoverse.CqrsKit.TestConsole
cd ..

mkdir web
cd web
dotnet new web --name Softoverse.CqrsKit.WebApi
cd ..

dotnet sln add tests/console/Softoverse.CqrsKit.TestConsole/Softoverse.CqrsKit.TestConsole.csproj
dotnet sln add tests/web/Softoverse.CqrsKit.WebApi/Softoverse.CqrsKit.WebApi.csproj

@REM Add Migration Command
@REM dotnet ef migrations add --project tests\web\Softoverse.CqrsKit.WebApi.DataAccess\Softoverse.CqrsKit.WebApi.DataAccess.csproj --startup-project tests\web\Softoverse.CqrsKit.WebApi\Softoverse.CqrsKit.WebApi.csproj --context Softoverse.CqrsKit.WebApi.DataAccess.ApplicationDbContext --configuration Debug Initial --output-dir Migrations

@REM Update Database Command
@REM dotnet ef database update --project tests\web\Softoverse.CqrsKit.WebApi.DataAccess\Softoverse.CqrsKit.WebApi.DataAccess.csproj --startup-project tests\web\Softoverse.CqrsKit.WebApi\Softoverse.CqrsKit.WebApi.csproj --context Softoverse.CqrsKit.WebApi.DataAccess.ApplicationDbContext --configuration Debug 20250121110353_Initial

