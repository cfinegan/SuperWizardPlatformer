using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;

namespace SuperWizardPlatformer
{
    class TextureDrawable : IDrawable
    {
        private IEntity parent;
        private TextureRegion2D textureRegion;
        private SpriteBatch spriteBatch;

        public TextureDrawable(IEntity parent, TextureRegion2D textureRegion, SpriteBatch spriteBatch)
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
            if (parent.IsVisible)
            {

                var bodyCenter = ConvertUnits.ToDisplayUnits(parent.Position);

                var texturePos = new Vector2(
                    bodyCenter.X - ConvertUnits.ToDisplayUnits(parent.Size.X) / 2.0f, 
                    bodyCenter.Y - ConvertUnits.ToDisplayUnits(parent.Size.Y) / 2.0f);

                spriteBatch.Draw(textureRegion.Texture, texturePos, null, textureRegion.Bounds);
            }
        }
    }
}
