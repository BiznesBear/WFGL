namespace WFGL.Pseudo.Physics;

public struct Force(float strenght, Vec2 direction)
{
    public float Strenght { get; set; } = strenght;
    public Vec2 Direction { get; set; } = direction;
}