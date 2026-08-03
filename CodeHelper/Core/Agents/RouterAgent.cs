using CodeHelper.Core.Interfaces;
using CodeHelper.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace CodeHelper.Core.Agents;

public class RouterAgent : IRouterAgent
{
    private ChatClientAgent _agent { get; set; }
    private readonly ChatClientAgentRunOptions _agentOptions;
    
    public RouterAgent(ChatClientAgent agent)
    {
        _agent = agent;

        _agentOptions = new ChatClientAgentRunOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(AgentRouterResponse))),
            ChatOptions = new ChatOptions
                {
                    Temperature = 0f,
                    TopP = 1,
                }
        };
    }

    public async Task<AgentRouterResponse?> RunAsync(string input, CancellationToken ct)
    {
        var session = await _agent.CreateSessionAsync();

        var messages = new List<ChatMessage>();
        messages.Add(new(ChatRole.User, input));

        session.SetInMemoryChatHistory(messages);

        var response = await _agent.RunAsync(session, _agentOptions, ct);

        var deserializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var objectResponse = JsonSerializer.Deserialize<AgentRouterResponse>(response.Text, deserializerOptions) ?? null;
        return objectResponse;
    }
}
