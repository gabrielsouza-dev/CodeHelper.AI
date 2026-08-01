using CodeHelper.Core.Models;

namespace CodeHelper.Core.Interfaces;

public interface IRouterAgent
{
    Task<AgentRouterResponse?> RunAsync(string input, CancellationToken ct);
}