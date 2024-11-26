using WFGL.Core;
using WFGL.Rendering;

namespace WFGL.Physics;

public struct Box
{
    public Vec2 Location { get; set; } 
    public Vec2 Size { get; set; }

    public Box()
    {
        Size = Vec2.One;
        Location = Vec2.Zero;
    }
    public Box(Vec2 pos, Vec2 size)
    {
        Size = size;
        Location = pos;
    }



    public readonly float Left => Location.X;
    public readonly float Top => Location.Y;
    public readonly float Right => unchecked(Location.X + Size.X);
    public readonly float Bottom => unchecked(Location.X + Size.Y);
    public readonly bool IsEmpty => Size.Y == 0 && Size.X == 0 && Location.X == 0 && Location.Y == 0;


    public readonly Rectangle GetRectangle(VirtualUnit u) => new(Location.ToPoint(u), Size.ToSize(u));
}
