namespace WFGL;

internal static class TextColors
{
    public const string DEFAULT = "\u001b[37m";
    public const string WHITE = "\u001b[37m";
    public const string BLACK = "\u001b[30m";
    public const string RED = "\u001b[31m";
    public const string GREEN = "\u001b[32m";
    public const string YELLOW = "\u001b[33m";
    public const string BLUE = "\u001b[34m";
    public const string MAGNETA = "\u001b[35m";
    public const string CYAN = "\u001b[36m";

    public static string FromEnum(this TextColor color) { return $"\u001b[{(int)color}m"; }
    internal static string SetColor(this string message,TextColor color) => $"{FromEnum(color)}{message}{DEFAULT}";

}
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