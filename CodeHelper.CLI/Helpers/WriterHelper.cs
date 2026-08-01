public static class WriterHelper
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

        WriteCentralized(lines);
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
