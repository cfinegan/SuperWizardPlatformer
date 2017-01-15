using Microsoft.Xna.Framework.Graphics;

namespace SuperWizardPlatformer
{
    interface IDrawable : IRemovable
    {
        void Draw(SpriteBatch spriteBatch);
    }
}
