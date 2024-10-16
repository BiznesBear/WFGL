namespace WFGL;

public sealed class LayerMaster
{
    private List<Layer> layers { get; set; } = [Layer.Defalut];

    /// <summary>
    /// Sets, and updates list of layers (with defalut, layer)
    /// </summary>
    public List<Layer> Layers
    {
        get => layers;
        set
        {
            layers = [Layer.Defalut];
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
    /// Deregisters layer
    /// </summary>
    /// <param name="layer">Layer to unregister</param>
    public void Deregister(Layer layer)
    {
        layers.Remove(layer);
        UpdateList();
    }

    /// <summary>
    /// Sorts layers list by draw weight
    /// </summary> 
    public void UpdateList() => layers.Sort((a, b) => a.DrawWeight.CompareTo(b.DrawWeight));

    public static Dictionary<Layer, List<IObject>> SortObjectList(List<IObject> list) => list.GroupBy(o => o.Layer).ToDictionary(g => g.Key, g => g.ToList());

    public static IEnumerable<IObject> GetObjectsFrom(LayerMaster lm,Dictionary<Layer, List<IObject>> order)
    {
        foreach (Layer layer in lm.layers)
        {
            if (order.TryGetValue(layer, out var objects))
            {
                foreach (IObject obj in objects)
                    yield return obj;
            }
        }
    }
}
public class Layer(short drawWeight)
{
    public readonly static Layer Defalut = new(0);
    public short DrawWeight { get; } = drawWeight;
}   