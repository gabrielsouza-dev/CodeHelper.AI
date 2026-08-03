using Microsoft.Extensions.AI;
using System.Runtime.CompilerServices;

namespace CodeHelper.Options;

public class CodeHelperOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1";
    public string AgentModel { get; set; } = "deepseek/deepseek-v4-flash-0731";
    public string ProgrammingLanguage = "C#";
    public string? RouterModel { get; set; }
    public string? WebSearchModel { get; set; }
    public IList<AITool>? WebSearchTools { get; set; }
}