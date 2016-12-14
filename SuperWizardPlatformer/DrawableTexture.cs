using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;

namespace SuperWizardPlatformer
{
    class DrawableTexture : IDrawable
    {
        private IEntity parent;
        private TextureRegion2D textureRegion;
        private SpriteBatch spriteBatch;

        public DrawableTexture(IEntity parent, TextureRegion2D textureRegion, SpriteBatch spriteBatch)
        {
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }
            if (spriteBatch == null) { throw new ArgumentNullException(nameof(spriteBatch)); }

            this.parent = parent;
            this.textureRegion = textureRegion;
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            Console.WriteLine(parent.IsVisible);
            if (parent.IsVisible)
            {
                Console.WriteLine(parent.Size);
                Rectangle destRect = new Rectangle(
                    (int)ConvertUnits.ToDisplayUnits(parent.Position.X),
                    (int)ConvertUnits.ToDisplayUnits(parent.Position.Y),
                    (int)ConvertUnits.ToDisplayUnits(parent.Size.X),
                    (int)ConvertUnits.ToDisplayUnits(parent.Size.Y));

                Console.WriteLine(destRect);

                spriteBatch.Draw(textureRegion.Texture, null, destRect, textureRegion.Bounds);
            }
        }
    }
}
