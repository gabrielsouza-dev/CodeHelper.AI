using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace CodeHelper.Core.Agents;

public class CodeHelperAgent : ICodeHelperAgent
{
    public ChatClientAgent Agent { get; set; }
    private readonly ChatClientAgentRunOptions _agentOptions;
    
    public CodeHelperAgent(ChatClientAgent agent)
    {
        Agent = agent;

        _agentOptions = new ChatClientAgentRunOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(CodeHelperResponse)))
        };
    }

    public async IAsyncEnumerable<string> RunAsync(string input, [EnumeratorCancellation] CancellationToken ct)
    {
        var session = await Agent.CreateSessionAsync();

        var messages = new List<ChatMessage>();
        messages.Add(new(ChatRole.User, input));

        session.SetInMemoryChatHistory(messages);

        await foreach (var update in Agent.RunStreamingAsync(session, _agentOptions, ct))
        {
            yield return update.Text;
        }
    }
}