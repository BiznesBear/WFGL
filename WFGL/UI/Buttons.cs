﻿using WFGL.Core;
using WFGL.Input;
using WFGL.Objects;

namespace WFGL.UI;

public enum ButtonState
{
    Defalut,
    Pointed,
    Pressed
}
public abstract class ButtonBase<T> : Transform
{
    public ButtonState State { get; private set; }
    public Vec2 RectSize { get; set; } = new(1.5f, 0.5f);
    public Size RealRectSize => RectSize.ToSize(Master.VirtualScale);
    public Rectangle Bounds => new(RealPosition.X, RealPosition.Y, RealRectSize.Width, RealRectSize.Height);

    public event Action? OnClick;
    public event Action? OnRelease;
    public event Action? OnPoint;

    public T displayed;
    public T Defalut;
    public T Pointed;
    public T Clicked;

    public ButtonBase(T defalut,T pointed,T clicked)
    {
        Defalut = defalut;
        Pointed = pointed;
        Clicked = clicked;
        displayed = Defalut;
    }

    public override void OnUpdate()
    {
        if (!Master.InputMaster.MouseInside)
        {
            ChangeState(ButtonState.Defalut);
            return;
        }
        if (Bounds.IntersectsWithMouse(Master.InputMaster))
        {
            if (Mouse.IsButtonPressed(MouseButtons.Left))
            {
                ChangeState(ButtonState.Pressed);
                return;
            }
            ChangeState(ButtonState.Pointed);
            return;
        }
        ChangeState(ButtonState.Defalut);
    }

    private void ChangeState(ButtonState state)
    {
        OnStateChanged(State, state);
        State = state;
    }

    public virtual void OnStateChanged(ButtonState oldState, ButtonState newState) {

        displayed = newState switch
        {
            ButtonState.Defalut => Defalut,
            ButtonState.Pointed => Pointed,
            ButtonState.Pressed => Clicked,
            _ => Defalut
        };

        switch (newState)
        {
            case ButtonState.Defalut:
                OnRelease?.Invoke();
                break;
            case ButtonState.Pointed:
                OnPoint?.Invoke();
                break;
            case ButtonState.Pressed:
                OnClick?.Invoke();
                break;
        }
    }
}
public class RectangleButton(Color defalut, Color pointed, Color clicked) : ButtonBase<Color>(defalut, pointed, clicked)
{
    private SolidBrush GetBrush() => new(displayed);
    public RectangleButton(Color color) : this(color, color, color) { }
    public override void OnDraw() => Master.Renderer.FillRectangle(GetBrush(), Bounds); 
}
public class BitmapButton(Bitmap defalutBitmap, Bitmap pointedBitmap, Bitmap clickedBitmap) : ButtonBase<Bitmap>(defalutBitmap, pointedBitmap, clickedBitmap)
{
    public BitmapButton(Bitmap bmp) : this(bmp,bmp,bmp) { }
    public override void OnDraw() => Master.Drawer.DrawBitmap(displayed, Bounds.Location.ToVec2(Master.VirtualScale));
}
public class TextRectButton : RectangleButton
{
    public Font font = new(StringRenderer.DEFALUT_FONT_NAME,12);

    public StringRenderer stringRenderer;
    public TextRectButton(string text,Color defalut, Color pointed, Color clicked) : base(defalut, pointed, clicked) { stringRenderer = new(font, text); }
    public TextRectButton(string text,Color color) : this(text,color, color, color) { }

    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        h.Register(stringRenderer);
    }

    public override void OnDestroy(Hierarchy h, GameMaster m)
    {
        base.OnDestroy(h, m);
        h.Deregister(stringRenderer);
    }

}
public class TextBitmapButton : BitmapButton
{
    public Font font = new(StringRenderer.DEFALUT_FONT_NAME, 12);

    public StringRenderer stringRenderer;
    public TextBitmapButton(string text,Bitmap defalutBmp, Bitmap pointedBmp, Bitmap clickedBmp) : base(defalutBmp, pointedBmp, clickedBmp) { stringRenderer = new(font, text);}
    public TextBitmapButton(string text, Bitmap bmp) : this(text,bmp, bmp, bmp) { }

    public override void OnCreate(Hierarchy h, GameMaster m)
    {
        base.OnCreate(h, m);
        h.Register(stringRenderer);
    }

    public override void OnDestroy(Hierarchy h, GameMaster m)
    {
        base.OnDestroy(h, m);
        h.Deregister(stringRenderer);
    }
}