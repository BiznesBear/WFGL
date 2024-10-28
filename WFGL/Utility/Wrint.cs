namespace WFGL;

/// <summary>
/// Wfgl own printer - Wrint
/// </summary>
public static class Wrint
{
    public static void Info(object message) => Console.WriteLine($"[INFO] ".SetColor(TextColor.Cyan) + message);
    public static void Warring(object message) => Console.WriteLine($"[WARRING] ".SetColor(TextColor.Yellow) + message);
    public static void Error(object message) => Console.WriteLine($"[ERROR] ".SetColor(TextColor.Red) + message);
    public static string Command() 
    {
        Console.Write($"{Application.ProductName}.console> ");
        return Console.ReadLine() ?? "";
    }
}
