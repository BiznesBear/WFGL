using WFGL.Objects;

namespace WFGL.Physics;
public abstract class GravityTransform : Transform
{
    public Vec2 Velocity { get; set; }
    public float MaxVelocity { get; set; } = 0.1f;
    public float GravityStrenght { get; set; } = 0.15f;
    public Vec2 GravityDirection { get; set; } = Vec2.Down;

    public override void OnUpdate()
    {
        base.OnUpdate();
        Velocity += GravityDirection * GravityStrenght * GetMaster().TimeMaster.DeltaTimeF;
        Velocity = CheckVelocityLimit(Velocity);
        Position += Velocity;
    }

    public void AddForce(Force force)
    {
        Velocity += force.Direction * force.Strenght * GetMaster().TimeMaster.DeltaTimeF;
        Velocity = CheckVelocityLimit(Velocity);
    }

    public void ResetVelocity() => Velocity = 0;
    private Vec2 CheckVelocityLimit(Vec2 velocity) 
    {
        return new Vec2(
        Math.Clamp(velocity.X, -MaxVelocity, MaxVelocity),
        Math.Clamp(velocity.Y, -MaxVelocity, MaxVelocity)); 
    }
}
