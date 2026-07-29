using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace CodeHelper.Core;

public class CodeHelper : ICodeHelper
{
    public ChatClientAgent Agent { get; set; }
    private readonly ChatClientAgentRunOptions _agentOptions;
    

    public CodeHelper(ChatClientAgent agent)
    {
        Agent = agent;


        _agentOptions = new ChatClientAgentRunOptions()
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(CodeHelperResponse)))
        };
    }

    public async IAsyncEnumerable<string> RunAsync(string input)
    {
        var session = new CodeHelperSession();
        
        var messages = new List<ChatMessage>();
        messages.Add(new(ChatRole.User, input));

        session.SetInMemoryChatHistory(messages);

        await foreach (var update in Agent.RunStreamingAsync(session, _agentOptions))
        {
            yield return update.Text;
        }
    }
}