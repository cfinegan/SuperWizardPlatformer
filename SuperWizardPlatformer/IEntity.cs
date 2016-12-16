using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    interface IEntity
    {
        Vector2 Position { get; set; }

        Vector2 Size { get; }

        bool IsVisible { get; set; }

        void Update(GameTime gameTime);
    }
}
