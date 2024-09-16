using WFGL.Core;
namespace WFGL;
public interface IAsset<T>
{
    T Source { get; }
    void Load(string filePath);
}

public interface IParentable
{
    public Transform? Parent { get; set; }
    public void BindParent(Transform? parent = null);
    public void UpdateToParent(GameMaster m);
}
