using CodeHelper.Core.Interfaces;
using CodeHelper.Core.Tools;
using CodeHelper.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeHelper.Extensions;
public static class CodeHelperDIExtensions
{
    public static void AddCodeHelper(this IServiceCollection services)
    {
        services.AddSingleton<ICodeHelperEngine>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<CodeHelperOptions>>().Value;
            var webSearchMCP = sp.GetService<WebSearchMCP>();

            var tools = webSearchMCP?.Tools ?? [];

            return CodeHelperFactory
                .ConfigureClient(settings.ApiKey, settings.ApiUrl)
                .SelectPrincipalModel(settings.AgentModel)
                .SelectRouterModel(settings.RouterModel)
                .SelectWebSearchModel(settings.WebSearchModel)
                .WithTools(tools)
                .WithLanguage(settings.ProgrammingLanguage)
                .Build();
        });
    }
}