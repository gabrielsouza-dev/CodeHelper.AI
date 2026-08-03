using CodeHelper.Core.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;


namespace CodeHelper.Core.Tools;
public class WebSearchMCPStartupService : IHostedService
{
    private readonly WebSearchMCP _dockerMcps;
    private readonly WebSearchMCPOptions _webSearchMCPOptions;

    public WebSearchMCPStartupService(WebSearchMCP dockerMcps, IOptions<WebSearchMCPOptions> webSearchMCPOptions)
    {
        _dockerMcps = dockerMcps;
        _webSearchMCPOptions = webSearchMCPOptions.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
        => _dockerMcps.StartWebSearchMCP(_webSearchMCPOptions);

    public Task StopAsync(CancellationToken cancellationToken)
        => _dockerMcps.StopMcpsAsync();
}