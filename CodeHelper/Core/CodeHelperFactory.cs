using CodeHelper.Core.Agents;
using CodeHelper.Core.Interfaces;
using CodeHelper.Options;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace CodeHelper.Core;

public static class CodeHelperFactory
{
    public static CodeHelperOptions ConfigureClient(string apiKey, string? apiUrl = null)
    {
        return new CodeHelperOptions
        {
            ApiKey = apiKey,
            ApiUrl = apiUrl ?? "https://api.openai.com/v1"
        };
    }

    public static CodeHelperOptions WithLanguage(this CodeHelperOptions options, string language)
    {
        options.ProgrammingLanguage = language;
        return options;
    }

    public static CodeHelperOptions SelectPrincipalModel(this CodeHelperOptions options, string model)
    {
        options.AgentModel = model;
        return options;
    }

    public static CodeHelperOptions SelectRouterModel(this CodeHelperOptions options, string? model)
    {
        options.RouterModel = model;
        return options;
    }

    public static CodeHelperOptions SelectWebSearchModel(this CodeHelperOptions options, string? webSearchModel)
    {
        options.WebSearchModel = webSearchModel;
        return options;
    }

    public static CodeHelperOptions WithTools(this CodeHelperOptions options, IList<AITool>? tools)
    {
        options.WebSearchTools = tools;
        return options;
    }


    public static ICodeHelper Build(this CodeHelperOptions options)
    {
        var codehelperInstrictions = GetAgentInstructions("CodeHelper", options.ProgrammingLanguage);

        var openAiClient = new OpenAIClient(
            new ApiKeyCredential(options.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(options.ApiUrl)
            });

        IChatClient principalClient = openAiClient
            .GetChatClient(options.AgentModel)
            .AsIChatClient();

        var codeHelperChat = principalClient.AsAIAgent(instructions: codehelperInstrictions, name: "CodeHelper");

        var codeHelper = new CodeHelperAgent(codeHelperChat);

        IRouterAgent? routerAgent = null;
        IWebSearchAgent? webSearchAgent = null;
        if (options.RouterModel is null
            || options.WebSearchModel is null
            || options.WebSearchTools is null
            || options.WebSearchTools?.Count == 0)
        {
            routerAgent = new NoOpRouterAgent();
            webSearchAgent = new NoOpWebSearchAgent();
        } else
        {
            var RouterInstructions = GetAgentInstructions("RouterAgent");
            var WebSearchInstructions = GetAgentInstructions("WebSearchAgent");

            IChatClient routerClient = openAiClient
            .GetChatClient(options.RouterModel)
            .AsIChatClient();

            IChatClient webSearchClient = openAiClient
                .GetChatClient(options.WebSearchModel)
                .AsIChatClient();

            var routerChat = routerClient.AsAIAgent(instructions: RouterInstructions, name: "RouterAgent");
            var webSearchChat = webSearchClient.AsAIAgent(instructions: WebSearchInstructions, name: "WebSearchAgent", tools: options.WebSearchTools);

            routerAgent = new RouterAgent(routerChat);
            webSearchAgent = new WebSearchAgent(webSearchChat);
        }

        return new CodeHelper(codeHelper, routerAgent, webSearchAgent);
    }

    private static string GetAgentInstructions(string InstructionName, string? programmingLanguage = null)
    {
        var instructionsPath = Path.Combine(AppContext.BaseDirectory, "Instructions", $"{InstructionName}.md");
        var instruction = File.ReadAllText(instructionsPath);

        if (programmingLanguage != null)
            instruction = string.Format(instruction, programmingLanguage);

        return instruction;
    }
}
