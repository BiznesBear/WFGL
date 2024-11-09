﻿using WFGL.Core;
using WFGL.Objects;
using WFGL.Physics;

namespace WFGL.Rendering;

/// <summary>
/// Bitmap rendered directly on renderer.
/// </summary>
public class BitmapRenderer : Transform, IDrawable
{
    public Bitmap Source { get; set; }
    public Hroup? Hroup { get; set; }

    // TODO: Rework how real size of any object is get
    /// <summary>
    /// Viewed size of sprite on the screen
    /// </summary>
    public Point RealSize => new((int)(Source.Width * Scale.X), (int)(Source.Height * Scale.Y));


    public BitmapRenderer(Bitmap bitmap)
    {
        Source = bitmap;
    }
    public BitmapRenderer(string filePath) : this(new Bitmap(filePath)) { }
    public void Draw(GameMaster m, Graphics r)
    {
        if (Hroup != null) return;
        Point size = RealSize.VirtualizePixel(m.MainCamera);
        Point pos = Position.ToPoint(m);
        r.DrawImage(Source, pos.X, pos.Y, size.X, size.Y);
    }
}
/// <summary>
/// Bitmap with built-in collider.
/// </summary>
public class CollidingBitmap : BitmapRenderer, ICollide
{
    public Vec2 ColliderSize => RealSize.VirtualizePixel(GetMaster().MainCamera).ToVec2(GetMaster());
    public Vec2 ColliderPosition => Position;

    public CollidingBitmap(string filePath) : base(filePath) { }
    public CollidingBitmap(Bitmap bitmap) : base(bitmap) { }
}
