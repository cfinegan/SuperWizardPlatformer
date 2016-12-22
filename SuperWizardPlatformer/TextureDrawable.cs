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

        public TextureDrawable(IEntity parent, TextureRegion2D textureRegion)
        {
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            this.parent = parent;
            this.textureRegion = textureRegion;
        }

        public bool IsMarkedForRemoval { get; set; }

        public void Draw(SpriteBatch spriteBatch)
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
