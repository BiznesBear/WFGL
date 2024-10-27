namespace WFGL.Physics;

public class Gravity
{
    public Vec2 Velocity { get; set; }
    public float MaxVelocity { get; set; } = 0.05f; //TODO: Add more max velocitys for diffrent directions 
    public float Strenght { get; set; } = 0.003f;
    public Vec2 Direction { get; set; } = Vec2.Down;

    public Gravity() { }
    public Gravity(float strenght) 
    {
        Strenght = strenght;
    }
    public Gravity(float strenght,float maxVelocity) : this(strenght)
    {
        MaxVelocity = maxVelocity;
    }

    public Vec2 Calculate(Vec2 pos)
    {
        Velocity += Direction * Strenght;
        return pos + CheckVelocityLimit(this);
    }
    public void AddForce(float force, Vec2 direction)
    {
        Velocity += direction * force;
        Velocity = CheckVelocityLimit(this);
    }

    public void ResetVelocity() => Velocity = Direction * Strenght;
    public static Vec2 CheckVelocityLimit(Gravity grav) => new(Math.Clamp(grav.Velocity.X,-grav.MaxVelocity, grav.MaxVelocity), Math.Clamp(grav.Velocity.Y, -grav.MaxVelocity, grav.MaxVelocity));
}
