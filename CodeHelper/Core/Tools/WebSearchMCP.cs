using CodeHelper.Options;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;

namespace CodeHelper.Core.Tools;
public class WebSearchMCP : IAsyncDisposable
{
    private readonly WebSearchMCPOptions _mcpOptions;

    public WebSearchMCP(IOptions<WebSearchMCPOptions> mcpOptions)
    {
        _mcpOptions = mcpOptions.Value;
    }

    private McpClient? _client;
    public readonly List<AITool> Tools = new List<AITool>();

    public async Task StartWebSearchMCP()
    {
        Console.Write("Iniciando Web Search MCP... ");

        var args = new List<string> { "run", "-i", "--rm" };
        args.AddRange(["--name", _mcpOptions.Name]);
        args.AddRange(["-e", $"{_mcpOptions.EnvApiName}={_mcpOptions.ApiKey}"]);
        args.Add(_mcpOptions.Id);

        var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
        {
            Command = "docker",
            Arguments = [.. args],
            Name = _mcpOptions.Name
        });

        var mcpClient = await McpClient.CreateAsync(clientTransport);

        var toolList = await mcpClient.ListToolsAsync();

        _client = mcpClient;
        Tools.AddRange(toolList.Cast<AITool>().ToList());

        Console.WriteLine(" - OK");
        await Task.Delay(1000);
    }

    public async Task StopMcpsAsync()
    {
        if (_client is not null)
        {
            Console.Write($"Parando Web Search MCP...");
            await _client.DisposeAsync(); 
            Console.WriteLine(" - OK");
            await Task.Delay(1000);
        }

        _client = null;
        Tools.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        await StopMcpsAsync();
    }
}