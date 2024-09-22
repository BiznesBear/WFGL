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

    public virtual void OnUpdate(GameMaster m) { }
    public virtual void OnDraw(GameMaster m) { }


}
public interface IDrawable
{

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
public interface ICollide
{
    public Vector2 ColliderSize { get; set; }
    public Vector2 ColliderOffset { get; set; }
}
public interface ICircleCollide : ICollide
{
    public float Radius { get; set; }
}