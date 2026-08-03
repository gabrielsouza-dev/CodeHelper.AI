using CodeHelper.Core;
using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Tools;
using CodeHelper.Options;
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
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args, new Dictionary<string, string>
    {
        ["--api-key"] = "CodeHelper:ApiKey",
        ["--api-url"] = "CodeHelper:ApiUrl",
        ["--agent-model"] = "CodeHelper:AgentModel",
        ["--router-model"] = "CodeHelper:RouterModel",
        ["--websearch-model"] = "CodeHelper:WebSearchModel",
        ["--language"] = "CodeHelper:ProgrammingLanguage"
    });

builder.Services.Configure<CodeHelperOptions>(
    builder.Configuration.GetSection("CodeHelper"));

builder.Services.Configure<WebSearchMCPOptions>(
    builder.Configuration.GetSection("WebSearchMCP"));

builder.Services.AddSingleton<WebSearchMCP>();

//builder.Services.AddHostedService<WebSearchMCPStartupService>();
var webSearchMCP = builder.Services.BuildServiceProvider().GetRequiredService<WebSearchMCP>();
await webSearchMCP.StartWebSearchMCP();

builder.Services.AddSingleton<ICodeHelper>((services) =>
{
    var settings = services.GetRequiredService<IOptions<CodeHelperOptions>>().Value;

    var codeHelper = CodeHelperFactory
        .ConfigureClient(settings.ApiKey, settings.ApiUrl)
        .SelectPrincipalModel(settings.AgentModel)
        .SelectRouterModel(settings.RouterModel)
        .SelectWebSearchModel(settings.WebSearchModel)
        .WithTools(webSearchMCP.Tools)
        .WithLanguage(settings.ProgrammingLanguage)
        .Build();

    return codeHelper;
});

builder.Services.AddHostedService<ConsoleWorker>();

var app = builder.Build();
await app.RunAsync();
