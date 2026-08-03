using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Models;
using System.Runtime.CompilerServices;

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

    public async IAsyncEnumerable<string> RunAsync(string input, [EnumeratorCancellation] CancellationToken ct)
    {
        var route = await GetRouteAsync(input, ct);
        
        if(route.NeedWebSearch)
        {
            //logica de pesquisa
        }

        await foreach (var chunk in _codeHelperAgent.RunAsync(input, ct))
        {
            yield return chunk;
        }
    }

    private async Task<AgentRouterResponse> GetRouteAsync(string input, CancellationToken ct)
    {
        AgentRouterResponse? route = null;
        for (int i = 0; i < 3; i++)
        {
            if(ct.IsCancellationRequested)
               throw new OperationCanceledException(); 

            route = await _routerAgent.RunAsync(input, ct);

            if (route != null) break;
        }
        if (route == null)
            throw new GetRouterException("Falha ao verificar rotas necessarias em 3 tentativas.");

        return route;
    }
}

public class GetRouterException : Exception 
{
    public GetRouterException(string message) : base(message: message) { }
}