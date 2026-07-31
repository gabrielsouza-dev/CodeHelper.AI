using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace CodeHelper.Core.Agents;

public class RouterAgent : IRouterAgent
{
    public ChatClientAgent Agent { get; set; }
    private readonly ChatClientAgentRunOptions _agentOptions;
    
    public RouterAgent(ChatClientAgent agent)
    {
        Agent = agent;

        _agentOptions = new ChatClientAgentRunOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(AgentRouterResponse)))
        };
    }

    public async Task<AgentRouterResponse?> RunAsync(string input)
    {
        var session = await Agent.CreateSessionAsync();

        var messages = new List<ChatMessage>();
        messages.Add(new(ChatRole.User, input));

        session.SetInMemoryChatHistory(messages);

        var response = await Agent.RunAsync(session, _agentOptions);
        var objectResponse = JsonSerializer.Deserialize<AgentRouterResponse>(response.Text) ?? null;

        return objectResponse;
    }
}
