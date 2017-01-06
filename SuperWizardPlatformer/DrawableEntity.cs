using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.TextureAtlases;
using System;

namespace SuperWizardPlatformer
{
    class DrawableEntity : Entity, IDrawable
    {
        private Vector2 halfSize;
        private TextureRegion2D textureRegion;

        public DrawableEntity(World world, TiledObject obj, TextureRegion2D textureRegion, bool isVisible = true)
            : base(world, obj)
        {
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            halfSize = ConvertUnits.ToSimUnits(new Vector2(obj.Width / 2.0f, obj.Height / 2.0f));
            this.textureRegion = textureRegion;
            IsVisible = isVisible;
            Body.Position = obj.GetObjectCenter();
            Body.BodyType = obj.GetBodyType();
        }

        public override void Update(IScene scene, GameTime gameTime)
        {
            return; // Do nothing by default.
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

        public override bool OnCollision(IEntity other)
        {
            return true;
        }
    }
}
