using WFGL.Core;
namespace WFGL.Rendering;

public interface IDrawable
{
    public void Draw(GameMaster m, Graphics r);
}

public interface IBrushDrawable : IDrawable
{
    public Brush Brush { get; set; }
}

public interface IPenDrawable : IDrawable
{
    public Pen Pen { get; set; }
}

public interface ITextureBrushDrawable : IDrawable
{
    public TextureBrush TextureBrush { get; set; }
}