using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using MonoGame.Extended.TextureAtlases;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using FarseerPhysics;

namespace SuperWizardPlatformer
{
    class Player : IEntity, IDrawable
    {
        private Sprite sprite;

        public Player(Body body, TextureRegion2D textureRegion)
        {
            if (body == null) { throw new ArgumentNullException(nameof(body)); }
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            Body = body;
            Body.UserData = this;
            sprite = new Sprite(textureRegion);
        }

        public Body Body { get; private set; }

        public bool IsMarkedForRemoval { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public void Update(IScene scene, GameTime gameTime)
        {
            float xVel = 0, yVel = 0;
            float velFactor = 0.0025f;

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

            Body.ApplyLinearImpulse(new Vector2(xVel, yVel));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Position = ConvertUnits.ToDisplayUnits(Body.Position);
            spriteBatch.Draw(sprite);
        }
    }
}
