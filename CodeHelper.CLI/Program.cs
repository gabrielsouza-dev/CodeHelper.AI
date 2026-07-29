using CodeHelper.Core;
using CodeHelper.Core.Models;
using JsonStreamingParser;

var apiKey = "sk-or...";
var apiUrl = "https://openrouter.ai/api/v1";

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
    .ConfigureClient(apiKey, apiUrl)
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