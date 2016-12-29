using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace SuperWizardPlatformer
{
    class DrawableEntity : Entity, IDrawable
    {
        private Vector2 halfSize;
        private TextureRegion2D textureRegion;

        public DrawableEntity(Body body, Vector2 size, TextureRegion2D textureRegion, bool isVisible = true)
            : base(body, isVisible)
        {
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            halfSize = new Vector2(size.X * 0.5f, size.Y * 0.5f);
            this.textureRegion = textureRegion;
        }

        public override void Update(IScene scene, GameTime gameTime)
        {
            //IsMarkedForRemoval = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                var texturePos = new Vector2(
                    Body.Position.X - halfSize.X, 
                    Body.Position.Y - halfSize.Y);

                spriteBatch.Draw(textureRegion, ConvertUnits.ToDisplayUnits(texturePos), Color.White);
            }
        }
    }
}
