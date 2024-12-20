﻿using WFGL.Core;
using WFGL.Input;
using WFGL.Objects;
using WFGL.Physics;

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
public class RectangleButton : ButtonBase<Color>
{
    private SolidBrush GetBrush() => new(displayed);
    public RectangleButton(Color defalut,Color pointed,Color clicked) : base(defalut, pointed, clicked) { }
    public RectangleButton(Color color) : this(color, color, color) { }
    public override void OnDraw()
    {
        Master.Renderer.FillRectangle(GetBrush(), Bounds);
    }
}
public class BitmapButton : ButtonBase<Bitmap>
{
    public BitmapButton(Bitmap defalutBitmap, Bitmap pointedBitmap, Bitmap clickedBitmap) : base(defalutBitmap, pointedBitmap, clickedBitmap) { }
    public BitmapButton(Bitmap bmp) : this(bmp,bmp,bmp) { }
    public override void OnDraw()
    {
        Master.DrawBitmap(displayed, Bounds.Location);
    }
}
public class TextRectButton : RectangleButton
{
    public Font font = new(StringRenderer.DEFALUT_FONT_NAME,12);
    public readonly StringRenderer stringRenderer;
    public TextRectButton(string text,Color defalut, Color pointed, Color clicked) : base(defalut, pointed, clicked) { stringRenderer = new(font, text); }
    public TextRectButton(string text,Color color) : this(text,color, color, color) { }

    public override void OnUpdate()
    {
        base.OnUpdate();
        stringRenderer.OnUpdate();
    }
    public override void OnDraw()
    {
        base.OnDraw();
        stringRenderer.Draw(Master,Master.Renderer);
    }
}
public class TextBitmapButton : BitmapButton
{
    public Font font = new(StringRenderer.DEFALUT_FONT_NAME, 12);
    public readonly StringRenderer stringRenderer;
    public TextBitmapButton(string text,Bitmap defalutBmp, Bitmap pointedBmp, Bitmap clickedBmp) : base(defalutBmp, pointedBmp, clickedBmp) { stringRenderer = new(font, text);}
    public TextBitmapButton(string text, Bitmap bmp) : this(text,bmp, bmp, bmp) { }

    public override void OnUpdate()
    {
        base.OnUpdate();
        stringRenderer.OnUpdate();
    }
    public override void OnDraw()
    {
        base.OnDraw();
        stringRenderer.Draw(Master, Master.Renderer);
    }
}