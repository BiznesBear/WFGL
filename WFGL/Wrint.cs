namespace WFGL;

/// <summary>
/// Wfgl printer - Wrint. 
/// </summary>
public static class Wrint
{

    /// <summary>
    /// Enables info when new object is registered or deregistered.
    /// </summary>
    public static bool ShowRegisters { get; set; } = false;

    /// <summary>
    /// Enable Infos, Warrings and Errors.
    /// </summary>
    public static bool ShowEvents { get; set; } = false;

    /// <summary>
    /// Enable other custom wrints.
    /// </summary>
    public static bool ShowCustoms { get; set; } = true;

    /// <summary>
    /// Turn on/off all settings.
    /// </summary>
    public static bool All
    {
        set
        {
            ShowRegisters = value;
            ShowEvents = value;
            ShowCustoms = value;
        }
    }
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
    private static string SetColor(this string message, TextColor color) => $"{color.FromEnum()}{message}{TextColor.White.FromEnum()}";
    private static void Print(string prefix, TextColor color, object message) => Console.WriteLine($"[{prefix}] ".SetColor(color) + message);


    public static void Info(object message)
    {
        if (ShowEvents)
            Print("INFO", TextColor.Cyan, message);
    }
    public static void Warring(object message)
    {
        if (ShowEvents)
            Print("WARRING", TextColor.Yellow, message);
    }
    public static void Error(object message)
    {
        if (ShowEvents)
            Print("ERROR", TextColor.Red, message);
    }
    public static void Register(object message)
    {
        if (ShowRegisters)
            Print("REGISTERED", TextColor.Green, message);
    }
    public static void Deregister(object message)
    {
        if (ShowRegisters)
            Print("DEREGISTERED", TextColor.Red, message);
    }


    #region ForUserUse
    public static void Collection<T>(IEnumerable<T> collection)
    {
        if (ShowCustoms)
            Print("COLLECTION", TextColor.Magneta, string.Join("; ", collection));
    }

    #endregion
}
