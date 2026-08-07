using CodeHelper.CLI.Helpers;
using CodeHelper.Core.Tools;
using CodeHelper.Extensions;
using CodeHelper.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

ConsoleHelper.Initialize();

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

var codeHelperOptions = builder.Configuration.GetSection("CodeHelper").Get<CodeHelperOptions>()!;
await WriterHelper.PrintSettings(codeHelperOptions);

var webSearchMCPOptions = builder.Configuration.GetSection("WebSearchMCP").Get<WebSearchMCPOptions>()!;
var webSearchMCP = new WebSearchMCP(webSearchMCPOptions);
await webSearchMCP.StartWebSearchMCP();

builder.Services.AddSingleton(webSearchMCP);

builder.Services.AddCodeHelper();

builder.Services.AddHostedService<ConsoleWorker>();

ConsoleHelper.EnableQuickEdit();
var app = builder.Build();
await app.RunAsync();