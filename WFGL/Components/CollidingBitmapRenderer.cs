using WFGL.Pseudo.Physics;
using WFGL.Rendering;
namespace WFGL.Components;

/// <summary>
/// Bitmap with built-in collider.
/// </summary>
public class CollidingBitmapRenderer : BitmapRenderer, ICollide
{
    public Vec2 ColliderSize => VecSize;
    public Vec2 ColliderPosition => Position;

    public CollidingBitmapRenderer(string filePath) : base(filePath) { }
    public CollidingBitmapRenderer(Bitmap bitmap) : base(bitmap) { }
}