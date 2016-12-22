using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using MonoGame.Extended.TextureAtlases;

namespace SuperWizardPlatformer
{
    class Player : DrawableEntity
    {
        private Vector2 halfSize;
        private TextureRegion2D textureRegion;
        private int health = 3;

        public Player(Body body, Vector2 size, TextureRegion2D textureRegion, bool isVisible = true)
            : base(body, size, textureRegion, isVisible)
        {
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            halfSize = new Vector2(size.X * 0.5f, size.Y * 0.5f);
            this.textureRegion = textureRegion;
        }

        public override void Update(IScene scene, GameTime gameTime)
        {
            int xVel = 0, yVel = 0;
            int velFactor = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                xVel -= velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                xVel += velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                yVel -= velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                yVel += velFactor;
            }

            Body.Position = new Vector2(Body.Position.X + xVel, Body.Position.Y + yVel);
        }
    }
}
