// TODO 1: Include the necessary packages for dependency Injection
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shapes;


namespace DrawNotSoPerfect;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // TODO 2: Modify this code to wire up the Dependency Injection infrastructure.
        // TODO 3: Register LocalFileStorage class in the Dependency Injector
        //var factory = new DefaultServiceProviderFactory();
        //var services = new ServiceCollection();
        //var builder = factory.CreateBuilder(services);
        //builder.AddTransient<DrawMain>();
        //builder.AddSingleton<IStorage, LocalFileStorage>();
        //var prov = builder.BuildServiceProvider();

        //var builder2 = new ConfigurationBuilder();
        //builder2.SetBasePath(Environment.CurrentDirectory);
        //builder2.AddJsonFile("appsettings.json");
        //var config = builder2.Build();

        //var factory2 = LoggerFactory.Create(bld => {
        //    bld.AddConfiguration(config.GetSection("Logging"));

        //    bld.ClearProviders();
        //    bld.AddConsole();
        //});


        // TODO 5: Configure logging. Clear all log providers and add the Debug Provider
        // (writes output to "Output" window in Visual Studio).
        ApplicationConfiguration.Initialize();
        var host = CreateHostBuilder().Build();
        Application.Run(host.Services.GetRequiredService<DrawMain>());
    }
    
    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            //.ConfigureAppConfiguration((ctx, cf) => {
            //    cf.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json");
            //})
            .ConfigureLogging(bld =>
            {
                bld.ClearProviders();
                bld.AddDebug();
            })
            .ConfigureServices((ctx, services) => {
                services.AddTransient<DrawMain>();
                services.AddSingleton<IStorage, LocalFileStorage>();
            });
    }
}