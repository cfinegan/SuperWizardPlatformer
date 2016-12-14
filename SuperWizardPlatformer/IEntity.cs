using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    interface IEntity
    {
        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        bool IsVisible { get; set; }

        void Update(GameTime gameTime);
    }
}
