using CodeHelper.Core.Agents;
using CodeHelper.Core.Interfaces;
using CodeHelper.Exceptions;
using CodeHelper.Models;
using System.Runtime.CompilerServices;

namespace CodeHelper.Core;

public class CodeHelper : ICodeHelper
{
    private readonly ICodeHelperAgent _codeHelperAgent;
    private readonly IRouterAgent _routerAgent;
    private readonly IWebSearchAgent _webSearchAgent;

    public CodeHelper(ICodeHelperAgent assistantAgent, IRouterAgent routerAgent, IWebSearchAgent webSearchAgent)
    {
        _codeHelperAgent = assistantAgent;
        _routerAgent = routerAgent;
        _webSearchAgent = webSearchAgent;
    }

    public async IAsyncEnumerable<string> RunAsync(string input, [EnumeratorCancellation] CancellationToken ct)
    {
        string? webSearchResponse = null;
        if(_routerAgent is RouterAgent)
        {

            yield return EmitStartExecutionState("[Router] Necessita Pesquisa Web?");
            var route = await GetRouteAsync(input, ct);
            var routerWebSearchResponse = route.NeedWebSearch ? "Sim" : "Não";
            yield return EmitEndExecutionState(" - " + routerWebSearchResponse);

            if (route.NeedWebSearch)
            {
                yield return EmitCompletedExecutionState("[WebSearch] Rodando Pesquisa Web...");
                webSearchResponse = await _webSearchAgent.RunAsync(input, ct);
            }
        }

        await foreach (var chunk in _codeHelperAgent.RunAsync(input, webSearchResponse, ct))
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

    private string EmitCompletedExecutionState(string message) =>
        $"{{ \"executionstate\": \"{message}\" }}";

    private string EmitStartExecutionState(string message) =>
        $"{{ \"executionstate\": \"{message}";

    private string EmitEndExecutionState(string? message) =>
        $" {message ?? ""}\" }}";
}
