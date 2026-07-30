using CodeHelper.Core;
using CodeHelper.Core.Models;
using JsonStreamingParser;
using Microsoft.Extensions.Configuration;

var basePath =
#if DEBUG
    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
#else
    AppContext.BaseDirectory;
#endif

var configuration = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();

var settings = configuration
    .GetSection("CodeHelperSettings")
    .Get<CodeHelperSettings>();

if (settings is null)
    throw new ArgumentNullException(nameof(settings));

ConsoleWriter.Header();
Console.Write("Linguagem de programação: ");
var programmingLanguage = Console.ReadLine();
if (string.IsNullOrWhiteSpace(programmingLanguage))
{
    Console.WriteLine();
    Console.WriteLine("Linguagem de programação não informada!");
    Console.WriteLine("Pressione qualquer tecla para encerrar..");
    Console.ReadKey();
    return;
}

var codeHelper = CodeHelperFactory
    .ConfigureClient(settings.ApiKey, settings.ApiUrl)
    .SelectPrincipalModel(settings.AgentModel)
    .SelectRouterModel(settings.RouterModel)
    .WithLanguage(programmingLanguage)
    .Build();

do
{
    Console.Write("Input: ");
    var input = Console.ReadLine();
    Console.WriteLine();
    if (string.IsNullOrWhiteSpace(input))
        return;

    var title = new JsonFieldStreamer(nameof(CodeHelperResponse.Title), ConsoleColor.Cyan);
    var explanation = new JsonFieldStreamer(nameof(CodeHelperResponse.Explanation), ConsoleColor.Gray);
    var code = new JsonFieldStreamer(nameof(CodeHelperResponse.Code), ConsoleColor.Green);
    var notes = new JsonFieldStreamer(nameof(CodeHelperResponse.Notes), ConsoleColor.Yellow);

    await foreach (var chunk in codeHelper.RunAsync(input))
    {
        title.ProcessChunk(chunk);
        code.ProcessChunk(chunk);
        explanation.ProcessChunk(chunk);
        notes.ProcessChunk(chunk);
    }
    Console.WriteLine();
} while (true);