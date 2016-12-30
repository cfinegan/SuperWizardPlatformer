using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using SuperWizardPlatformer.Input;
using System;

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

            if (ActionMapper.IsPressed(UserAction.MoveLeft))
            {
                xVel -= velFactor;
            }

            if (ActionMapper.IsPressed(UserAction.MoveRight))
            {
                xVel += velFactor;
            }

            if (ActionMapper.IsPressed(UserAction.Jump))
            {
                yVel -= velFactor * 5;
            }

            if (ActionMapper.IsPressed(UserAction.Duck))
            {
                yVel += velFactor * 5;
            }

            Body.ApplyLinearImpulse(new Vector2(xVel, yVel));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            double posX = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.X));
            double posY = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.Y));

            sprite.Position = new Vector2((float)posX, (float)posY);
            
            spriteBatch.Draw(sprite);
        }
    }
}
