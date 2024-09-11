using WFGL.Core;
namespace WFGL.UI;
// NOTE: Maybe delete canvas and just let user create own canvas as separate hierarchy
public class Canvas : Hierarchy
{
    public Camera Camera;

    public Canvas(GameMaster master) : base(master)
    {
        Camera = master.MainCamera;
    }
    public Canvas(GameMaster master, Camera camera) : base(master)
    {
        Camera = camera;
    }
}