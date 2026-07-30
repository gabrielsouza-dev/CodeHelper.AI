public static class ConsoleWriter
{
    public static void Header()
    {
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
        "--------------------------------------------------------------------------------------"

    };

        foreach (var line in lines)
        {
            int left = Math.Max((Console.WindowWidth - line.Length) / 2, 0);

            Console.SetCursorPosition(left, Console.CursorTop);
            Console.WriteLine(line);
        }
    }
}
