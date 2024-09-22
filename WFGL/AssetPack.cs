namespace WFGL;

public class AssetPack<T> : Dictionary<string, IAsset<T>> where T : class
{
    public AssetPack() { }
    public AssetPack(string folderPath) { }

    public void LoadFromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException($"The folder '{folderPath}' does not exist.");

        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            IAsset<T> asset = CreateAssetFromFile(file);
            if (asset != null)
            {
                this[Path.GetFileNameWithoutExtension(file)] = asset;
            }
        }
    }

    private static IAsset<T> CreateAssetFromFile(string filePath)
    {
        if (typeof(T) == typeof(Image))
        {
            return new Sprite(filePath) as IAsset<T> ?? throw new Exception("");
        }
        throw new NotSupportedException($"Unsupported asset type: {typeof(T).Name}");
    }
}