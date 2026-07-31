using CodeHelper.Core.Agents;
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

    public static AgentOptions SelectPrincipalModel(this AgentOptions options, string model)
    {
        options.AgentModel = model;
        return options;
    }

    public static AgentOptions SelectRouterModel(this AgentOptions options, string model)
    {
        options.RouterModel = model;
        return options;
    }

    public static CodeHelper Build(this AgentOptions options)
    {
        var codehelperInstrictions = GetCodeHelperInstruction(options.ProgrammingLanguage, "CodeHelper");
        var RouterInstrictions = GetCodeHelperInstruction(options.ProgrammingLanguage, "RouterAgent");

        var openAiClient = new OpenAIClient(
            new ApiKeyCredential(options.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(options.ApiUrl)
            });

        IChatClient chatClient = openAiClient
            .GetChatClient(options.AgentModel)
            .AsIChatClient();

        var codeHelperChat = chatClient.AsAIAgent(instructions: codehelperInstrictions, name: "CodeHelper");
        var codeHelper = new CodeHelperAgent(codeHelperChat);

        var routerChat = chatClient.AsAIAgent(instructions: RouterInstrictions, name: "RouterAgent");
        var routerAgent = new RouterAgent(routerChat);

        return new(codeHelper, routerAgent);
    }

    private static string GetCodeHelperInstruction(string programmingLanguage, string InstructionName)
    {
        var instructionsPath = Path.Combine(AppContext.BaseDirectory, "Instructions", $"{InstructionName}.md");
        var instruction = File.ReadAllText(instructionsPath);
        instruction = string.Format(instruction, programmingLanguage);

        return instruction;
    }
}
