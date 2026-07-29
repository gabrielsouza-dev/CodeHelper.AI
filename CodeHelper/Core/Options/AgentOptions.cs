namespace CodeHelper.Core.Options;

public class AgentOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string Model { get; set; } = "openai/gpt-4o-mini";
    public string ProgrammingLanguage { get; set; } = "C#";
    public AgentOptions(string apiKey, string? apiUrl = null)
    {
        ApiKey = apiKey;

        if(apiUrl != null)
            ApiUrl = apiUrl;
    }

}