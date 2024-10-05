using WFGL.Core;
namespace WFGL;
public interface IAsset<T> 
{
    T GetSource();
    void Load(string filePath);
}

public interface IDrawable
{
    public Hroup? Hroup { get; set; }
    public void Draw(GameMaster m, Graphics r);
}
public interface IParentable // TODO: make this work
{
    public Transform? Parent { get; set; }
    public void BindParent(Transform? parent = null);
    public void UpdateToParent(GameMaster m);
}
public interface IPivotable // TODO: make this work
{
    public Vector2 Pivot { get; set; } // RealPos -> Position + Size * Pivot | Size is not Scale 
    public Vector2 RealPosition { get; }
}
