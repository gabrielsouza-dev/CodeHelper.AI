using CodeHelper.Core.Interfaces;

namespace CodeHelper.Core.Agents;

public class NoOpWebSearchAgent : IWebSearchAgent
{
    public Task<string?> RunAsync(string input, CancellationToken ct)
    {
        return Task.FromResult<string?>(null);
    }
}