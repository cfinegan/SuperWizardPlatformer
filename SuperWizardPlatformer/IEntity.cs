using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    interface IEntity
    {
        Body Body { get; }

        Vector2 Position { get; set; }

        Vector2 Size { get; }

        bool IsMarkedForRemoval { get; set; }

        bool IsVisible { get; set; }

        IDrawable Drawable { get; }

        void Update(IScene scene, GameTime gameTime);
    }
}
