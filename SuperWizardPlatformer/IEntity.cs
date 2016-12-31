using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    interface IEntity
    {
        Body Body { get; }

        bool IsMarkedForRemoval { get; set; }

        bool IsVisible { get; set; }

        void Update(IScene scene, GameTime gameTime);

        bool OnCollision(IEntity other);
    }
}
