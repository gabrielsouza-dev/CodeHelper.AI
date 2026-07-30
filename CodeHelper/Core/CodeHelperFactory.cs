using CodeHelper.Core.Options;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace CodeHelper.Core;

public static class CodeHelperFactory
{
    public static AgentOptions ConfigureClient(string apiKey, string? apiUrl = null) => new(apiKey, apiUrl);

    public static AgentOptions WithLanguage(this AgentOptions options, string language)
    {
        options.ProgrammingLanguage = language;
        return options;
    }

    public static CodeHelper Build(this AgentOptions options)
    {
        var instruction = GetCodeHelperInstruction(options.ProgrammingLanguage);

        var openAiClient = new OpenAIClient(
            new ApiKeyCredential(options.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(options.ApiUrl)
            });

        IChatClient chatClient = openAiClient
            .GetChatClient(options.Model)
            .AsIChatClient();

        var agent = chatClient.AsAIAgent(instructions: instruction, name: "CodeHelper");

        return new(agent);
    }

    private static string GetCodeHelperInstruction(string programmingLanguage)
    {
        var instructionsPath = Path.Combine(AppContext.BaseDirectory, "Instructions", "CodeHelper.md");
        var instruction = File.ReadAllText(instructionsPath);
        instruction = string.Format(instruction, programmingLanguage);

        return instruction;
    }
}
