using CodeHelper.Core.Interfaces;
using CodeHelper.Models;
using JsonStreamingParser;
using Microsoft.Extensions.Hosting;

public class ConsoleWorker : BackgroundService
{
    private readonly ICodeHelper _codeHelper;
    private readonly IHostApplicationLifetime _lifetime;

    public ConsoleWorker(ICodeHelper codeHelper, IHostApplicationLifetime lifetime)
    {
        _codeHelper = codeHelper;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        WriterHelper.Header();

        do
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Input: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                break;

            Console.WriteLine();
            try
            {
                var title = new JsonFieldStreamer(nameof(CodeHelperResponse.Title), ConsoleColor.Green);
                var explanation = new JsonFieldStreamer(nameof(CodeHelperResponse.Explanation), ConsoleColor.Cyan);
                var code = new JsonFieldStreamer(nameof(CodeHelperResponse.Code), ConsoleColor.Gray);
                var notes = new JsonFieldStreamer(nameof(CodeHelperResponse.Notes), ConsoleColor.Yellow);
                await foreach (var chunk in _codeHelper.RunAsync(input!, ct))
                {
                    title.ProcessChunk(chunk);
                    code.ProcessChunk(chunk);
                    explanation.ProcessChunk(chunk);
                    notes.ProcessChunk(chunk);
                }
            }
            catch (Exception ex)
            {
                if(ex is not OperationCanceledException)
                    Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
        } while (true);

        _lifetime.StopApplication();
    }
}