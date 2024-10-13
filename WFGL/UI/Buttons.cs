using WFGL.Core;
using WFGL.Input;
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
    public Size RealRectSize => RectSize.ToSize(GetMaster());
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

    public override void OnUpdate(GameMaster m)
    {
        if (!m.InputMaster.MouseInside)
        {
            ChangeState(ButtonState.Defalut);
            return;
        }
        if (Bounds.IntersectsWithMouse(m.InputMaster))
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
    public RectangleButton(Color defalut,Color pointed,Color clicked) : base(defalut, pointed, clicked) { }
    public RectangleButton(Color color) : this(color, color, color) { }
    public override void OnDraw(GameMaster m)
    {
        m.DrawRectangle(displayed,Bounds);
    }
}
public class SpriteButton : ButtonBase<Sprite>
{
    public SpriteButton(Sprite defalutSprite, Sprite pointedSprite,Sprite clickedSprite) : base(defalutSprite, pointedSprite, clickedSprite) { }
    public SpriteButton(Sprite sprite) : this(sprite,sprite,sprite) { }
    public override void OnDraw(GameMaster m)
    {
        m.DrawSprite(displayed, Bounds.Location);
    }
}