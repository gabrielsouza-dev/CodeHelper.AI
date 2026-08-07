using CodeHelper.Options;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using System.Diagnostics;

namespace CodeHelper.Core.Tools;
public class WebSearchMCP : IAsyncDisposable
{
    private readonly WebSearchMCPOptions _mcpOptions;

    private McpClient? _client;
    private string? _containerName;

    public readonly List<AITool> Tools = [];

    public WebSearchMCP(WebSearchMCPOptions mcpOptions)
    {
        _mcpOptions = mcpOptions;
    }

    public async Task StartWebSearchMCP()
    {
        Console.Write("Iniciando Web Search MCP... ");

        _containerName = _mcpOptions.Name;

        var args = new List<string>
        {
            "run",
            "-i",
            "--rm",
            "--name",
            _containerName
        };

        args.AddRange(_mcpOptions.Args.Split(" "));
        args.Add(_mcpOptions.Id);

        var transport = new StdioClientTransport(new StdioClientTransportOptions
        {
            Command = "docker",
            Arguments = [.. args],
            Name = _containerName
        });

        _client = await McpClient.CreateAsync(transport);

        var tools = await _client.ListToolsAsync();

        Tools.AddRange(tools.Cast<AITool>());

        Console.WriteLine(" - OK");
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("Encerrando Web Search MCP...");

        if (_client != null)
        {
            await _client.DisposeAsync();
        }

        if (!string.IsNullOrEmpty(_containerName))
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"rm -f {_containerName}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                });

                if (process != null)
                    await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Falha removendo container: {ex.Message}");
            }
        }

        Console.WriteLine("Web Search MCP finalizado.");
    }
}
