using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperWizardPlatformer
{
    class DrawableTexture : IDrawable
    {
        private IEntity parent;
        private TextureRegion2D textureRegion;
        private SpriteBatch spriteBatch;

        public DrawableTexture(IEntity parent, TextureRegion2D textureRegion, SpriteBatch spriteBatch)
        {
            if (parent == null) { throw new ArgumentNullException("parent"); }
            if (textureRegion == null) { throw new ArgumentNullException("textureRegion"); }
            if (spriteBatch == null) { throw new ArgumentNullException("spriteBatch"); }

            this.parent = parent;
            this.textureRegion = textureRegion;
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            if (parent.IsVisible)
            {
                Rectangle destRect = new Rectangle(
                    (int)parent.Position.X,
                    (int)parent.Position.Y,
                    (int)parent.Size.X,
                    (int)parent.Size.Y);

                spriteBatch.Draw(textureRegion.Texture, null, destRect, textureRegion.Bounds);
            }
        }
    }
}
