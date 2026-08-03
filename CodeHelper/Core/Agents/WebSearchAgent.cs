using CodeHelper.Core.Interfaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace CodeHelper.Core.Agents
{
    internal class WebSearchAgent : IWebSearchAgent
    {
        private ChatClientAgent _agent;

        public WebSearchAgent(ChatClientAgent agent)
        {
            _agent = agent;
        }

        public async Task<string?> RunAsync(string input, CancellationToken ct)
        {
            var session = await _agent.CreateSessionAsync();

            var messages = new List<ChatMessage>();
            messages.Add(new(ChatRole.User, input));

            session.SetInMemoryChatHistory(messages);

            var response = await _agent.RunAsync(session, cancellationToken: ct);

            return response.Text;
        }
    }
}