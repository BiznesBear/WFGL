using WFGL.Objects;

namespace WFGL.Rendering;

public sealed class LayerMaster
{
    private List<Layer> LayersList { get; set; } = [Layer.Defalut];

    /// <summary>
    /// Sets, and updates list of layers (with defalut, layer)
    /// </summary>
    public List<Layer> Layers
    {
        get => LayersList;
        set
        {
            LayersList = [Layer.Defalut];
            LayersList.AddRange(value);
            UpdateList();
        }
    }

    /// <summary>
    /// Registers new layer
    /// </summary>
    /// <param name="layer">Layer to register</param>
    public void Register(Layer layer)
    {
        LayersList.Add(layer);
        UpdateList();
    }
    /// <summary>
    /// Deregisters layer
    /// </summary>
    /// <param name="layer">Layer to unregister</param>
    public void Deregister(Layer layer)
    {
        LayersList.Remove(layer);
        UpdateList();
    }

    /// <summary>
    /// Sorts layers list by draw weight
    /// </summary> 
    public void UpdateList() => LayersList.Sort((a, b) => a.DrawWeight.CompareTo(b.DrawWeight));

    /// <summary>
    /// Sorts objects by priority of drawing.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Dictionary<Layer, List<IObject>> SortObjectList(List<IObject> list) => list.GroupBy(o => o.Layer).ToDictionary(g => g.Key, g => g.ToList());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lm"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public static IEnumerable<IObject> GetObjectsFrom(LayerMaster lm, Dictionary<Layer, List<IObject>> order)
    {
        foreach (Layer layer in lm.LayersList)
        {
            if (order.TryGetValue(layer, out var objects))
            {
                foreach (IObject obj in objects)
                    yield return obj;
            }
        }
    }
}
/// <summary>
/// Specifies the priority of drawing the object.
/// </summary>
/// <param name="drawWeight">Priority of drawing the object</param>
public class Layer(short drawWeight)
{
    public readonly static Layer Defalut = new(0);
    public short DrawWeight { get; } = drawWeight;
}