using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Models;
using Microsoft.Extensions.AI.Evaluation;
using System.ComponentModel;

namespace CodeHelper.Core;

public class CodeHelper : ICodeHelper
{
    private readonly ICodeHelperAgent _codeHelperAgent;
    private readonly IRouterAgent _routerAgent;

    public CodeHelper(ICodeHelperAgent assistantAgent, IRouterAgent routerAgent)
    {
        _codeHelperAgent = assistantAgent;
        _routerAgent = routerAgent;
    }

    public async IAsyncEnumerable<string> RunAsync(string input)
    {
        var route = await GetRouteAsync(input);
        
        if(route.NeedWebSearch)
        {
            //logica de pesquisa
        }

        await foreach (var chunk in RunAssistentAsync(input))
        {
            yield return chunk;
        }
    }

    private async Task<AgentRouterResponse> GetRouteAsync(string input)
    {
        AgentRouterResponse? route = null;
        for (int i = 0; i < 3; i++)
        {
            route = await _routerAgent.RunAsync(input);

            if (route != null) break;
        }
        if (route == null)
            throw new GetRouterException("Falha ao verificar rotas necessarias em 3 tentativas.");

        return route;
    }

    private async IAsyncEnumerable<string> RunAssistentAsync(string input)
    {
        await foreach (var chunk in _codeHelperAgent.RunAsync(input))
        {
            yield return chunk;
        }
    }
}

public class GetRouterException : Exception 
{
    public GetRouterException(string message) : base(message: message) { }
}