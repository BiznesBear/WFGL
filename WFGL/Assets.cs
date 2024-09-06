using System.Media;

namespace WFGL;
// TODO: Rework this managment 
public static class AssetsManager
{
    public readonly static Dictionary<string, Asset> Assets=new();
    public static void LoadAllAssets()
    {
        foreach(var asset in Assets.Values)
        {
            asset.Load();
        }
    }
}
public class Asset
{
    public string Name { get; } 
    public string FileName { get; } 
    public AssetType FileType { get; }

    private AssetSource Source = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"> File path for the file</param>
    /// <param name="name"> Accessible name for assets manager</param>
    /// <param name="fileType"> </param>
    public Asset(string path,string name,AssetType fileType)
    {
        FileName = path;
        FileType = fileType;
        Name = name;
    }
    public void Load()
    {
        if (!File.Exists(FileName)) return;
        switch (FileType)
        {
            case AssetType.Text:
                Source.Text = File.ReadAllText(FileName);
                break;
            case AssetType.Image:
                Source.Img = Image.FromFile(FileName);
                break;
            case AssetType.Audio:
                Source.Audio = new(FileName);
                break;
        }
    }
    //public object GetContent()
    //{
    //    return FileType switch
    //    {
    //        AssetType.Text => Text,
    //        AssetType.Image => Img ?? throw new Exception($"Can't read {Name} asset"),
    //        AssetType.Audio => Audio ?? throw new Exception($"Can't read {Name} asset"),
    //        _ => throw new Exception($"Can't read {Name} asset")
    //    };
    //}
}
public enum AssetType
{
    Text,
    Image,
    Audio
}
public struct AssetSource()
{
    internal string Text { get;  set; } = string.Empty;
    internal Image? Img { private get ; set; }
    internal SoundPlayer? Audio { private get; set; }
}