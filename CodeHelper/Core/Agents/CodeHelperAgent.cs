using CodeHelper.Core.Interfaces;
using CodeHelper.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace CodeHelper.Core.Agents;

public class CodeHelperAgent : ICodeHelperAgent
{
    private ChatClientAgent _agent { get; set; }
    private readonly ChatClientAgentRunOptions _agentOptions;
    
    public CodeHelperAgent(ChatClientAgent agent)
    {
        _agent = agent;

        _agentOptions = new ChatClientAgentRunOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(CodeHelperResponse)))
        };
    }

    public async IAsyncEnumerable<string> RunAsync(string input, string? webSearchResponse, [EnumeratorCancellation] CancellationToken ct)
    {
        var session = await _agent.CreateSessionAsync();

        var messages = new List<ChatMessage>();
        messages.Add(new(ChatRole.User, input));

        session.SetInMemoryChatHistory(messages);

        await foreach (var update in _agent.RunStreamingAsync(session, _agentOptions, ct))
        {
            yield return update.Text;
        }
    }
}