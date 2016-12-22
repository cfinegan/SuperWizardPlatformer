using Microsoft.Xna.Framework.Graphics;

namespace SuperWizardPlatformer
{
    interface IDrawable
    {
        bool IsMarkedForRemoval { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
