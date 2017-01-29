using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer.Physics
{
    interface IPhysicsObject
    {
        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        Vector2 Velecity { get; set; }

        Vector2 Acceleration { get; set; }

        Vector2 Density { get; set; }

        MovementType MovementType { get; set; }

        float Friction { get; set; }
    }
}
