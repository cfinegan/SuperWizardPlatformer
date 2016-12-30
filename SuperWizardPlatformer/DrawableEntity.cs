using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;

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
                double posX = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.X - halfSize.X));
                double posY = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.Y - halfSize.Y));

                spriteBatch.Draw(textureRegion, new Vector2((float)posX, (float)posY), Color.White);
            }
        }
    }
}
