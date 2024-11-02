namespace WFGL.Utilities;

internal enum TextColor
{
    White = 37,
    Black = 30,
    Red = 31,
    Green = 32,
    Yellow = 33,
    Blue = 34,
    Magneta = 35,
    Cyan = 36
}

/// <summary>
/// Wfgl printer - Wrint
/// </summary>
public static class Wrint
{
    internal const string DEFAULT = "\u001b[37m";
    internal const string WHITE = "\u001b[37m";
    internal const string BLACK = "\u001b[30m";
    internal const string RED = "\u001b[31m";
    internal const string GREEN = "\u001b[32m";
    internal const string YELLOW = "\u001b[33m";
    internal const string BLUE = "\u001b[34m";
    internal const string MAGNETA = "\u001b[35m";
    internal const string CYAN = "\u001b[36m";

    internal static string FromEnum(this TextColor color) { return $"\u001b[{(int)color}m"; }
    internal static string SetColor(this string message, TextColor color) => $"{FromEnum(color)}{message}{DEFAULT}";


    public static void Info(object message) => Console.WriteLine($"[INFO] ".SetColor(TextColor.Cyan) + message);
    public static void Warring(object message) => Console.WriteLine($"[WARRING] ".SetColor(TextColor.Yellow) + message);
    public static void Error(object message) => Console.WriteLine($"[ERROR] ".SetColor(TextColor.Red) + message);
    public static string Command() 
    {
        Console.Write($"{Application.ProductName}.console> ");
        return Console.ReadLine() ?? "";
    }
}
