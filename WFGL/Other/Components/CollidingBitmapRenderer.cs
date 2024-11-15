using WFGL.Core;
using WFGL.Physics;
using WFGL.Rendering;
namespace WFGL.Other.Components;

/// <summary>
/// Bitmap with built-in collider.
/// </summary>
public class CollidingBitmapRenderer : BitmapRenderer, ICollide
{
    public Vec2 ColliderSize => RealSize.VirtualizePixel(GetMaster().MainView).ToVec2(GetMaster().VirtualScale);
    public Vec2 ColliderPosition => Position;

    public CollidingBitmapRenderer(string filePath) : base(filePath) { }
    public CollidingBitmapRenderer(Bitmap bitmap) : base(bitmap) { }
}