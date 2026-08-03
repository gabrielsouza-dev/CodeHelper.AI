using CodeHelper.Models;

namespace CodeHelper.Core.Interfaces;

public interface IRouterAgent
{
    Task<AgentRouterResponse?> RunAsync(string input, CancellationToken ct);
}