using WFGL.Core;
namespace WFGL;
public interface IAsset<T>
{
    T Source { get; }
    void Load(string filePath);
}
public interface IObject
{
    public void Create(Hierarchy hierarchy);
    public void Destroy(Hierarchy hierarchy);
    public virtual void OnCreate() { }
    public virtual void OnDestroy() { }
}
public interface IUpdatable
{
    public virtual void OnUpdate(GameMaster m) { }
}
public interface IDrawable
{
    public virtual void OnDraw(GameMaster m) { }
}
public interface IParentable
{
    public Transform? Parent { get; set; }
    public void BindParent(Transform? parent = null);
    public void UpdateToParent(GameMaster m);
}
public interface IPivotable
{
    public Vector2 Pivot { get; set; } // RealPos -> Position + Size * Pivot | Size is not Scale 
    public Vector2 RealPosition { get; }
}
