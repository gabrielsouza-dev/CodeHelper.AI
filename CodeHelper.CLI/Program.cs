using CodeHelper.Core;
using CodeHelper.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var basePath =
#if DEBUG
    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
#else
    AppContext.BaseDirectory;
#endif

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Configuration.Sources.Clear();

builder.Configuration
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

builder.Services.Configure<CodeHelperSettings>(
    builder.Configuration.GetSection("CodeHelperSettings"));

builder.Services.AddSingleton<ICodeHelper>((services) =>
{
    var settings = services.GetRequiredService<IOptions<CodeHelperSettings>>().Value;

    var codeHelper = CodeHelperFactory
        .ConfigureClient(settings.ApiKey, settings.ApiUrl)
        .SelectPrincipalModel(settings.AgentModel)
        .SelectRouterModel(settings.RouterModel)
        .WithLanguage(settings.ProgrammingLanguage)
        .Build();

    return codeHelper;
});

builder.Services.AddHostedService<ConsoleWorker>();

var app = builder.Build();
await app.RunAsync();