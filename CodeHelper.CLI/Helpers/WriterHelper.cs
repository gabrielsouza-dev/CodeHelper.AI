using CodeHelper.Options;

namespace CodeHelper.CLI.Helpers;
public static class WriterHelper
{
    public static void Header()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;

        string[] lines =
        {
            "",
            " ▄▄▄▄▄▄▄          ▄▄       ▄▄▄   ▄▄▄       ▄▄                       ▄▄▄▄▄▄▄ ▄▄▄      ▄▄▄▄▄",
            "███▀▀▀▀▀          ██       ███   ███       ██                      ███▀▀▀▀▀ ███       ███ ",
            "███      ▄███▄ ▄████ ▄█▀█▄ █████████ ▄█▀█▄ ██ ████▄ ▄█▀█▄ ████▄    ███      ███       ███ ",
            "███      ██ ██ ██ ██ ██▄█▀ ███▀▀▀███ ██▄█▀ ██ ██ ██ ██▄█▀ ██ ▀▀    ███      ███       ███ ",
            "▀███████ ▀███▀ ▀████ ▀█▄▄▄ ███   ███ ▀█▄▄▄ ██ ████▀ ▀█▄▄▄ ██    ██ ▀███████ ████████ ▄███▄",
            "                                              ██                                          ",
            "                                              ▀▀                                          ",
            "======================================================================================",
            "Agente assistente para desenvolvimento",
            "----------------------------------------------------------------------------------"
        };

        WriteCentralized(lines);
    }

    public static async Task PrintSettings(CodeHelperOptions settings)
    {
        Console.WriteLine();
        Console.WriteLine("Configurações do CodeHelper:");
        Console.WriteLine($"  Modelo do Agente Principal: {settings.AgentModel}");
        await Task.Delay(300);
        Console.WriteLine($"  Modelo do Agente Router: {settings.RouterModel}");
        await Task.Delay(300);
        Console.WriteLine($"  Modelo do Agente Web Search: {settings.WebSearchModel}");
        await Task.Delay(300);
        Console.WriteLine($"  Linguagem de Programação: {settings.ProgrammingLanguage}");
        await Task.Delay(3000);
    }

    private static void WriteCentralized(string[] lines)
    {
        foreach (var line in lines)
        {
            int left = Math.Max((Console.WindowWidth - line.Length) / 2, 0);

            Console.SetCursorPosition(left, Console.CursorTop);
            Console.WriteLine(line);
        }
    }
}
