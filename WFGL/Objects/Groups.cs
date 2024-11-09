using WFGL.Core;
namespace WFGL.Objects;

/// <summary>
/// Allows to group objects by type.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Group<T> : HashSet<T>
{
    public HashSet<Hierarchy> Hierarchys { get; } = new();

    public event HierarchyEventHandler? HierarchyAdded;
    public event HierarchyEventHandler? HierarchyRemoved;

    public Group(params Hierarchy[] hierarchys)
    {
        foreach (Hierarchy hierarchy in hierarchys)
            Register(hierarchy);
        Update();
    }

    public void Register(Hierarchy hierarchy)
    {
        Hierarchys.Add(hierarchy);
        HierarchyAdded?.Invoke(hierarchy);
    }

    public void Unregister(Hierarchy hierarchy)
    {
        Hierarchys.Remove(hierarchy);
        HierarchyRemoved?.Invoke(hierarchy);
    }

    /// <summary>
    /// Update object list.
    /// </summary>
    public void Update()
    {
        Clear();
        foreach (var hierarchy in Hierarchys)
            foreach (var obj in hierarchy.GetAllObjectsWithType<T>())
                Add(obj);
    }
}
/// <summary>
/// Auto updates when new hierarchy or object is added in any hierarchy of group.
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutoGroup<T> : Group<T>
{
    public AutoGroup(params Hierarchy[] hierarchys) : base(hierarchys)
    {
        HierarchyAdded += AddedHierarchy;
        HierarchyRemoved += RemovedHierarchy;
    }
    private void AddedHierarchy(Hierarchy h)
    {
        h.ChangedList += Update;
        Update();
    }

    private void RemovedHierarchy(Hierarchy h)
    {
        h.ChangedList -= Update;
        Update();
    }
}