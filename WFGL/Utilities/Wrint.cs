namespace WFGL.Utilities;

/// <summary>
/// Wfgl printer - Wrint. 
/// </summary>
public static class Wrint
{
    private enum TextColor
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
    private static string FromEnum(this TextColor color) { return $"\u001b[{(int)color}m"; }
    private static string SetColor(this string message, TextColor color) => $"{FromEnum(color)}{message}{FromEnum(TextColor.White)}";
    private static void Print(string prefix, TextColor color, object message) => Console.WriteLine($"[{prefix}] ".SetColor(color) + message);


    public static void Info(object message)
    {
        if (WFGLSettings.ShowEvents)
            Print("INFO", TextColor.Cyan, message);
    }
    public static void Warring(object message)
    {
        if (WFGLSettings.ShowEvents)
            Print("WARRING", TextColor.Yellow, message);
    }
    public static void Error(object message)
    {
        if (WFGLSettings.ShowEvents)
            Print("ERROR", TextColor.Red, message);
    }
    public static void Register(object message)
    {
        if (WFGLSettings.ShowRegisters)
            Print("REGISTERED", TextColor.Green, message);
    }
    public static void Deregister(object message)
    {
        if (WFGLSettings.ShowRegisters)
            Print("DEREGISTERED", TextColor.Red, message);
    }


    // un used in wfgl
    #region ForUserUse
    public static void Collection<T>(IEnumerable<T> collection)
    {
        if (WFGLSettings.ShowCustoms)
            Print("COLLECTION", TextColor.Magneta, string.Join("; ", collection));
    }

    #endregion
}
