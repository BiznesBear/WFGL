namespace WFGL;

/// <summary>
/// Simple console messages 
/// </summary>
public static class Output
{
    public static void Info(object message) => Console.WriteLine($"{TextColors.CYAN}[INFO]{TextColors.DEFAULT} " + message);
    public static void Warring(object message) => Console.WriteLine($"{TextColors.YELLOW}[WARRING]{TextColors.DEFAULT} " + message);
    public static void Error(object message) => Console.WriteLine($"{TextColors.RED}[ERROR]{TextColors.DEFAULT} " + message);
    public static string Command() 
    {
        Console.Write($"{Application.ProductName}.console> ");
        return Console.ReadLine() ?? "";
    }
}
