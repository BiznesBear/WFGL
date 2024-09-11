namespace WFGL;

public interface IAsset<T> 
{
    T GetSource();
    void Load(string filePath);
}
