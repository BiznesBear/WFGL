namespace WFGL;

public interface IAsset<T> 
{
    T Source { get; }
    void Load(string filePath);
}
