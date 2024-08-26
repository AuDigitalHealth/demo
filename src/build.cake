#addin nuget:?package=Cake.CodeGen.NSwag&version=13.18.2&loaddependencies=true
#addin nuget:?package=Cake.FileHelpers&version=4.0.1

var target = Argument("target", "Test.Unit");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    DotNetCoreClean($"./demo.sln");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild("./demo.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("DemoFhirWebApi")
    .IsDependentOn("DemoFhirWebApi.Angular")
    .IsDependentOn("DemoFhirWebApi.CSharp");

Task("DemoFhirWebApi.Swagger")
    .IsDependentOn("Build")
    .Does(() => 
{ 
    //Generate swagger.json.
    using(var process = StartAndReturnProcess("c:/program files/dotnet/dotnet.exe", 
        new ProcessSettings{
            Arguments = "swagger tofile --serializeasv2 --output ./Demo.FhirWebApi/bin/Debug/net8.0/swagger.json ./Demo.FhirWebApi/bin/Debug/net8.0/Demo.FhirWebApi.dll v2" 
        }))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("DemoFhirWebApi.Angular")
    .IsDependentOn("DemoFhirWebApi.Swagger")
    .Does(() => 
{
    NSwag.FromJsonSpecification("Demo.FhirWebApi/bin/Debug/net8.0/swagger.json")
    .GenerateTypeScriptClient("./Demo.Portal/ClientApp/src/app/core/services/demo-client.service.ts", new TypeScriptClientGeneratorSettings() {
       BaseUrlTokenName = "Demo_FHIR_API_BASE_URL",
       ClassName = "{controller}Client",
       Template = TypeScriptTemplate.Angular,
       InjectionTokenType = InjectionTokenType.InjectionToken,
       RxJsVersion = 7.0M
    });
});

Task("DemoFhirWebApi.CSharp")
    .IsDependentOn("DemoFhirWebApi.Swagger")
    .Does(() =>
{
    //Clear out existing client to make sure there are no remains of an older version.
    CleanDirectory("./Demo.FhirWebApi.Client/Generated");
    
    //Run autoreset against the service.
    using(var process = StartAndReturnProcess("c:/program files/nodejs/npx.cmd", 
        new ProcessSettings{
            Arguments = "autorest --csharp --legacy --input-file=Demo.FhirWebApi/bin/Debug/net8.0/swagger.json --output-folder=./Demo.FhirWebApi.Client/Generated --license-header=MICROSOFT_MIT_NO_VERSION --namespace=Demo.FhirWebApi.Client --add-credentials" 
        }))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code: {0}", process.GetExitCode());
    }
    
    //Do a text replace on the client to replace relative path with full path.
    ReplaceRegexInFiles("Demo.FhirWebApi.Client/Generated/DemoFhirWebApi.cs", "\"/DemoFhirWebApi/v2\"", "\"https://localhost:44371/DemoFhirWebApi/v2\"");
});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);