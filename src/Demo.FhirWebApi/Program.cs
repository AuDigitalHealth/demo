using Serilog;
using Demo.FhirWebApi;

// from pca
IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// from pca -Setup logging
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

try
{
    Log.Information("Starting web host");

    Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build().Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}