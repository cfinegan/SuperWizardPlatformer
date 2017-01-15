using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    interface IEntity : IRemovable
    {
        Body Body { get; }

        bool IsVisible { get; set; }

        void Update(IScene scene, GameTime gameTime);

        bool OnCollision(IEntity other);

        void OnSeparation(IEntity other);
    }
}
