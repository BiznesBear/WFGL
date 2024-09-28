namespace WFGL;


public sealed class Layer(byte drawWeight)
{
    public readonly static Layer Defalut = new(0);
    public static List<Layer> layers { get; private set; } = [Defalut];

    public short DrawWeight { get; } = drawWeight;


    /// <summary>
    /// Sets, and updates list of layers (with defalut, layer)
    /// </summary>
    public static List<Layer> Layers
    {
        get => layers;
        set
        {
            layers = [Defalut];
            layers.AddRange(value);
            UpdateList();
        }
    }

    /// <summary>
    /// Registers new layer
    /// </summary>
    /// <param name="layer">Layer to register</param>
    public void Register(Layer layer)
    {
        layers.Add(layer);
        UpdateList();
    }
    /// <summary>
    /// Unregisters layer
    /// </summary>
    /// <param name="layer">Layer to unregister</param>
    public void Unregister(Layer layer)
    {
        layers.Remove(layer);
        UpdateList();
    }

    /// <summary>
    /// Sorts layers list by draw weight
    /// </summary> 
    public static void UpdateList() => layers.Sort((a, b) => a.DrawWeight.CompareTo(b.DrawWeight));


    public static Dictionary<Layer, List<IObject>> SortObjectList(List<IObject> list) => list.GroupBy(o => o.Layer).ToDictionary(g => g.Key, g => g.ToList());

    public static IEnumerable<IObject> GetObjectsFrom(Dictionary<Layer, List<IObject>> order)
    {
        foreach (Layer layer in Layer.layers)
        {
            if (order.TryGetValue(layer, out var objects))
            {
                foreach (IObject obj in objects)
                    yield return obj;
            }
        }
    }
}    