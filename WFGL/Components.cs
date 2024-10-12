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
