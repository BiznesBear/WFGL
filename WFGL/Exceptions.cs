namespace WFGL;

[Serializable]
public class WFGLError : Exception
{
	internal virtual TextColor Color { get; set; } = TextColor.Red;
	internal virtual string Prefix { get; set; } = "<ERROR!>";
	public WFGLError() { }
	public WFGLError(string message) : base(message) { }
	public WFGLError(string message, Exception inner) : base(message, inner) { }
    public override string ToString() => $"{Prefix} {Message}; \n{StackTrace}".SetColor(Color);
}
