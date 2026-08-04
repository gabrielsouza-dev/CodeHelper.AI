using System.Runtime.InteropServices;
using System.Text;

namespace CodeHelper.CLI.Helpers;
public static class ConsoleHelper
{
    private const int STD_INPUT_HANDLE = -10;

    private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
    private const uint ENABLE_EXTENDED_FLAGS = 0x0080;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(
        IntPtr hConsoleHandle,
        out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(
        IntPtr hConsoleHandle,
        uint dwMode);


    public static void DisableQuickEdit()
    {
        var handle = GetStdHandle(STD_INPUT_HANDLE);

        if (!GetConsoleMode(handle, out uint mode))
            return;

        // Necessário para conseguir alterar o Quick Edit
        mode |= ENABLE_EXTENDED_FLAGS;

        // Remove o modo seleção por mouse
        mode &= ~ENABLE_QUICK_EDIT_MODE;

        SetConsoleMode(handle, mode);
    }


    public static void EnableQuickEdit()
    {
        var handle = GetStdHandle(STD_INPUT_HANDLE);

        if (!GetConsoleMode(handle, out uint mode))
            return;

        // Necessário para conseguir alterar o Quick Edit
        mode |= ENABLE_EXTENDED_FLAGS;

        // Habilita seleção por mouse
        mode |= ENABLE_QUICK_EDIT_MODE;

        SetConsoleMode(handle, mode);
    }

    public static void Initialize()
    {
        DisableQuickEdit();
        Console.Title = "CodeHelper - Assistente de Desenvolvimento";

        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            Console.WindowWidth = 140;
            Console.WindowHeight = 40;

            Console.BufferWidth = 140;
            Console.BufferHeight = 500;
        }
        catch
        {
            Console.WriteLine("Não foi possível alterar o tamanho do console.\nPressione qualquer tecla para continuar..");
            Console.ReadKey();
        }

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        Console.Clear();

        Console.CursorVisible = true;
    }
}