using System.ComponentModel;

namespace CodeHelper.Models;

public class CodeHelperResponse
{
    [Description("Título curto que resume a solução.")]
    public required string Title { get; set; }

    [Description("Explicação objetiva sobre o conceito ou solução apresentada.")]
    public required string Explanation { get; set; }

    [Description("Código da solução. Inclua somente quando a resposta envolver implementação ou quando um exemplo de código for útil. Retorne apenas código válido, sem markdown.")]
    public string? Code { get; set; }

    [Description("Observações relevantes, boas práticas, limitações, cuidados ou alternativas. Inclua somente quando existirem informações úteis.")]
    public List<string>? Notes { get; set; }
}