namespace WFGL;

public static class Debug
{
    public static void Info(string message) => Console.WriteLine($"{TextColors.CYAN}[INFO]{TextColors.DEFAULT} " + message);
    public static void Warring(string message) => Console.WriteLine($"{TextColors.YELLOW}[WARRING]{TextColors.DEFAULT} " + message);
    public static void Error(string message) => Console.WriteLine($"{TextColors.RED}[ERROR]{TextColors.DEFAULT} " + message);
}
