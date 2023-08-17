using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
       .ConfigureFunctionsWorkerDefaults(builder => { }, option =>
       {
           option.EnableUserCodeException = true;
       })
        .ConfigureLogging(logging =>
        {
            logging.AddApplicationInsights();
        })
       .ConfigureAppConfiguration(config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables())
    .ConfigureServices(services =>
    {
        services.AddLogging(builder =>
        {
            builder.AddApplicationInsights();
        });
        var serviceProvider = services.BuildServiceProvider();

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddHttpClient();
    })
    .Build();



await host.RunAsync();

