using CodeHelper.Core.Interfaces;
using CodeHelper.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Realtime;
using System.Text.Json;

namespace CodeHelper.Core.Agents;

public class NoOpRouterAgent : IRouterAgent
{
    public Task<AgentRouterResponse?> RunAsync(string input, CancellationToken ct)
    {
        return Task.FromResult<AgentRouterResponse?>(new AgentRouterResponse
        {
            NeedWebSearch = false
        });
    }
}
