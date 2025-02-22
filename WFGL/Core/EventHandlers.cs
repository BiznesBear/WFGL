using WFGL.Objects;
namespace WFGL.Core;

public delegate void GameMasterEventHandler(GameMaster master);
public delegate void EntityEventHandler(Entity entity);
public delegate void TransformEventHandler(Transform transform);
public delegate void HierarchyEventHandler(Hierarchy hierarchy);
