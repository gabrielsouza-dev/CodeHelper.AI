using System.ComponentModel;

namespace CodeHelper.Core.Models;

public class AgentRouterResponse
{
    [Description("Indica se o prompt do usuario necessita pesquisa web para exatidão em resposta.")]
    public bool NeedWebSearch { get; set; }
}
