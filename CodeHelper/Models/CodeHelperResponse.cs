using System.ComponentModel;

namespace CodeHelper.Models;
public class CodeHelperResponse
{
    [Description("Título curto que resume a solução.")]
    public string Title { get; set; } = string.Empty;

    [Description("Explicação objetiva sobre o conceito ou solução apresentada.")]
    public string Explanation { get; set; } = string.Empty;

    [Description("Trecho de código completo e funcional. Retorne apenas código válido, sem markdown.")]
    public string Code { get; set; } = string.Empty;

    [Description("Lista de observações, boas práticas, limitações ou cuidados importantes.")]
    public List<string> Notes { get; set; } = new();
}
