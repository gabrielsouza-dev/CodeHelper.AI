public class CodeHelperSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string AgentModel { get; set; } = "openai/gpt-4.1-mini";
    public string RouterModel { get; set; } = "openai/gpt-4.1-nano";
    public string ProgrammingLanguage = "C#";
}