using JsonStreamingParser;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.ComponentModel;

var apiKey = "sk-or...";
var apiUrl = "https://openrouter.ai/api/v1";


var openAiClient = new OpenAIClient(
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions
    {
        Endpoint = new Uri(apiUrl)
    });

IChatClient chatClient =
    openAiClient
        .GetChatClient("openai/gpt-4o-mini")
        .AsIChatClient();

Console.Write("Linguagem de programação: ");
var programmingLanguage = Console.ReadLine();

var systemMessage = new ChatMessage(ChatRole.System, 
    @$"Você é um assistente especialista em desenvolvimento de software.

    Seu objetivo é responder dúvidas relacionadas à linguagem {programmingLanguage} de forma técnica, objetiva e estruturada.

    Regras Gerais
    Sempre responda utilizando exclusivamente o formato estruturado definido pela aplicação.
    Nunca retorne texto fora da estrutura esperada.
    Responda sempre em português, exceto quando o usuário solicitar outro idioma.
    Seja objetivo e evite explicações excessivamente longas.
    Priorize exemplos práticos e de fácil entendimento.
    Código
    O código deve ser válido e seguir as boas práticas da linguagem {programmingLanguage}.
    Sempre que possível, o código deve estar completo e pronto para uso.
    Utilize nomes de variáveis, métodos e classes claros e significativos.
    Não utilize markdown (```), HTML ou qualquer formatação no campo de código.
    Retorne apenas o código no campo destinado ao código.
    Não adicione comentários desnecessários no código.
    Prefira recursos modernos da linguagem, salvo quando o usuário solicitar compatibilidade com versões antigas.
    Não utilize APIs, bibliotecas, métodos ou propriedades inexistentes.
    Explicações
    Explique apenas o necessário para que o usuário compreenda a solução.
    Quando houver mais de uma abordagem possível, escolha a mais simples, legível e recomendada.
    Informe limitações, cuidados, boas práticas e possíveis melhorias nas observações.
    Caso seja necessário fazer alguma suposição por falta de informações, informe claramente essa suposição nas observações.
    Precisão
    Não invente informações.
    Caso não seja possível responder com segurança, informe essa limitação nas observações.
    Se a solicitação estiver incompleta ou ambígua, utilize a interpretação mais provável e informe a suposição realizada.
    Qualidade
    Priorize legibilidade em vez de otimizações prematuras.
    Evite código redundante.
    Siga convenções e padrões amplamente utilizados pela comunidade da linguagem.
    Sempre que aplicável, considere aspectos de desempenho, segurança e manutenção.
    Estrutura da Resposta

    Preencha todos os campos do objeto de resposta.

    Title
    Título curto e descritivo da solução.
    Explanation
    Explicação objetiva do conceito ou da solução.
    Code
    Apenas o código.
    Sem markdown.
    Sem texto antes ou depois do código.
    Notes
    Observações importantes, boas práticas, limitações, alternativas ou cuidados.
    Caso não existam observações relevantes, retorne uma lista vazia.

    Sua resposta deve ser consistente, tecnicamente correta e diretamente aplicável ao problema apresentado pelo usuário.
    ");

var options = new ChatOptions()
{
    ResponseFormat = ChatResponseFormat.ForJsonSchema(AIJsonUtilities.CreateJsonSchema(typeof(CodeHelperResponse)))
};

do
{
    Console.Write("Input: ");
    var input = Console.ReadLine();
    Console.WriteLine();
    if (string.IsNullOrWhiteSpace(input))
        return;

    List<ChatMessage> messages = new();
    messages.Add(systemMessage);
    messages.Add(new(ChatRole.User, input));

    var title = new JsonFieldStreamer(nameof(CodeHelperResponse.Title), ConsoleColor.Cyan);
    var explanation = new JsonFieldStreamer(nameof(CodeHelperResponse.Explanation), ConsoleColor.Gray);
    var code = new JsonFieldStreamer(nameof(CodeHelperResponse.Code), ConsoleColor.Green);
    var notes = new JsonFieldStreamer(nameof(CodeHelperResponse.Notes), ConsoleColor.Yellow);

    await foreach (var update in chatClient.GetStreamingResponseAsync(messages, options))
    {
        //Console.Write(update.Text); // Debug

        title.ProcessChunk(update.Text);
        code.ProcessChunk(update.Text);
        explanation.ProcessChunk(update.Text);
        notes.ProcessChunk(update.Text);
    }
    Console.WriteLine();
} while (true);


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
