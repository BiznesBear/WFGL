namespace WFGL;

[Serializable]
public class GameError : Exception
{
	internal virtual TextColor Color { get; set; } = TextColor.Red;
	internal virtual string Prefix { get; set; } = "<ERROR!>";
	public GameError() { }
	public GameError(string message) : base(message) { }
	public GameError(string message, Exception inner) : base(message, inner) { }
    public override string ToString() => $"{Prefix} {Message}; \n{StackTrace}".SetColor(Color);
}
