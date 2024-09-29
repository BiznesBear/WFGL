namespace WFGL.Pseudo;

public static class Collisions
{
    public static bool IsColliding(this ICollide self, ICollide other, Core.GameMaster master)
    {
        float leftA = self.ColliderPosition.X;
        float rightA = self.ColliderPosition.X + self.ColliderSize.X;
        float topA = self.ColliderPosition.Y;
        float bottomA = self.ColliderPosition.Y + self.ColliderSize.Y;


        float leftB = other.ColliderPosition.X;
        float rightB = other.ColliderPosition.X + other.ColliderSize.X;
        float topB = other.ColliderPosition.Y;
        float bottomB = other.ColliderPosition.Y + other.ColliderSize.Y;

        if (rightA < leftB || leftA > rightB || bottomA < topB || topA > bottomB)
        {
            return false;
        }
        return true;
    }
}
