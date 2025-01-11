using WFGL.Objects;

namespace WFGL.Rendering;

public sealed class LayerMaster
{
    private List<Layer> LayersList { get; set; } = [Layer.Defalut];

    /// <summary>
    /// Gets or sets layers list with defalut layer.
    /// </summary>
    public List<Layer> Layers
    {
        get => LayersList;
        set
        {
            LayersList = [Layer.Defalut];
            foreach(var l in value)
                Register(l);
            UpdateList();
        }
    }

    /// <summary>
    /// Registers new layer.
    /// </summary>
    /// <param name="layer">Layer to register</param>
    public void Register(Layer layer, bool updateList = true)
    {
        LayersList.Add(layer);
        if(updateList) UpdateList();
        Wrint.Register("New " + layer);
    }
    /// <summary>
    /// Deregisters existing layer.
    /// </summary>
    /// <param name="layer">Layer to unregister</param>
    public void Deregister(Layer layer, bool updateList = true)
    {
        LayersList.Remove(layer);
        if(updateList) UpdateList();
        Wrint.Deregister("New " + layer);
    }

    /// <summary>
    /// Sorts layers list by draw weight.
    /// </summary> 
    public void UpdateList() => LayersList.Sort((a, b) => a.DrawWeight.CompareTo(b.DrawWeight));

    /// <summary>
    /// Sorts objects by priority of drawing.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Dictionary<Layer, List<Entity>> SortObjectList(List<Entity> list) => list.GroupBy(o => o.Layer).ToDictionary(g => g.Key, g => g.ToList());

    public static IEnumerable<Entity> GetObjectsFrom(LayerMaster lm, Dictionary<Layer, List<Entity>> order)
    {
        foreach (Layer layer in lm.LayersList)
        {
            if (order.TryGetValue(layer, out var objects))
                foreach (Entity obj in objects)
                    yield return obj;
        }
    }
}

/// <summary>
/// Specifies the priority of drawing the objects.
/// </summary>
/// <param name="drawWeight">Priority of drawing the object</param>
public record Layer(int DrawWeight)
{
    public static Layer Defalut => new(0);
    public override string ToString() => $"Layer({DrawWeight})";
}