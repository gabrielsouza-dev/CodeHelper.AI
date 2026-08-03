using CodeHelper.Core.Options;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

public class WebSearchMCP : IAsyncDisposable
{
    private McpClient? _client;
    public readonly List<AITool> Tools = new List<AITool>();

    public async Task StartWebSearchMCP(WebSearchMCPOptions mcpOptions)
    {
        Console.Write("Iniciando Web Search MCP... ");

        var args = new List<string> { "run", "-i", "--rm" };
        args.AddRange(["--name", mcpOptions.Name]);
        args.AddRange(["-e", $"{mcpOptions.EnvApiName}={mcpOptions.ApiKey}"]);
        args.Add(mcpOptions.Id);

        var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
        {
            Command = "docker",
            Arguments = [.. args],
            Name = mcpOptions.Name
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