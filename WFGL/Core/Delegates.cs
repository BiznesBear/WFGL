using WFGL.Objects;

namespace WFGL.Core;
public delegate void GameMasterEventHandler(GameMaster master);
public delegate void ObjectEventHandler(IObject obj);
public delegate void TransformEventHandler(Transform transform);