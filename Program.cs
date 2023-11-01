using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

var host = new HostBuilder()
       .ConfigureFunctionsWorkerDefaults(builder => { }, option =>
       {
           option.EnableUserCodeException = true;
       })
       .ConfigureAppConfiguration(config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables())
    .ConfigureServices((hostContext,services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.Configure<LoggerFilterOptions>(options =>
        {
            // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
            // Log levels can also be configured using appsettings.json. For more information, see https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service#ilogger-logs
            LoggerFilterRule toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (toRemove is not null)
            {
                options.Rules.Remove(toRemove);
            }
        });
      
        var configuration = hostContext.Configuration;
      services.AddSingleton<ITelemetryInitializer, LogTelemetryInitializer>();
        services.AddHttpClient();
    })
    .Build();



await host.RunAsync();

