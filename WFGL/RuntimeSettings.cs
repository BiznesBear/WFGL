namespace WFGL;

public static class RuntimeSettings
{
    public static bool ShowRegisters { get; set; } = false;
    public static bool ShowEvents { get; set; } = false;

    public static bool All
    {
        set
        {
            ShowRegisters = value;
            ShowEvents = value;
        }
    }
}
