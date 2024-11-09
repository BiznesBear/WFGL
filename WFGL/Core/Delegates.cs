using WFGL.Objects;
namespace WFGL.Core;

#region EventHandlers
public delegate void GameMasterEventHandler(GameMaster master);
public delegate void ObjectEventHandler(IObject obj);
public delegate void TransformEventHandler(Transform transform);
public delegate void HierarchyEventHandler(Hierarchy hierarchy);

#endregion