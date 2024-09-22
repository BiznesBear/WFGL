namespace WFGL;


public sealed class Layer(byte drawWeight)
{
    public readonly static Layer Defalut = new(0);
    public static List<Layer> List { get; private set; } = [Defalut];

    public short DrawWeight { get; } = drawWeight;


    /// <summary>
    /// Sets, and updates list of layers (with defalut, layer)
    /// </summary>
    public static List<Layer> Registration
    {
        set
        {
            List = [Defalut];
            List.AddRange(value);
            UpdateList();
        }
    }

    /// <summary>
    /// Registers new layer
    /// </summary>
    /// <param name="layer">Layer to register</param>
    public void Register(Layer layer)
    {
        List.Add(layer);
        UpdateList();
    }
    /// <summary>
    /// Unregisters layer
    /// </summary>
    /// <param name="layer">Layer to unregister</param>
    public void Unregister(Layer layer)
    {
        List.Remove(layer);
        UpdateList();
    }

    /// <summary>
    /// Sorts layers list by draw weight
    /// </summary> 
    public static void UpdateList() => List.Sort((a, b) => a.DrawWeight.CompareTo(b.DrawWeight));
}