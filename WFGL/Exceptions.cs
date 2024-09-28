namespace WFGL;

[Serializable]
public class WFGLError : Exception
{
	internal virtual TextColor Color { get; set; } = TextColor.Red;
	internal virtual string Prefix { get; set; } = "[Error]";
	public WFGLError() { }
	public WFGLError(string message) : base(message) { }
	public WFGLError(string message, Exception inner) : base(message, inner) { }
    public override string ToString() => $"{Prefix} {Message}; \n{StackTrace}".SetColor(Color);
}

[Serializable]
public class WFGLNullInstanceError : WFGLError
{
	internal override string Prefix { get; set; } = "[NullInstanceError]";
    public WFGLNullInstanceError() { }
    public WFGLNullInstanceError(string message) : base(message) { }
    public WFGLNullInstanceError(string message, Exception inner) : base(message, inner) { }
}