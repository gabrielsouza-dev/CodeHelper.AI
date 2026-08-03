using Microsoft.Extensions.AI;

namespace CodeHelper.Core.Options;

public class AgentOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string AgentModel { get; set; } = "openai/gpt-4.1-mini";
    public string RouterModel { get; set; } = "openai/gpt-4.1-nano";
    public string ProgrammingLanguage { get; set; } = "C#";
    public IList<AITool> WebSearchTools { get; set; } = new List<AITool>();

    public AgentOptions(string apiKey, string? apiUrl = null)
    {
        ApiKey = apiKey;

        if(apiUrl != null)
            ApiUrl = apiUrl;
    }

}