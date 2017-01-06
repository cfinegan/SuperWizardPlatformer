using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using SuperWizardPlatformer.Input;
using System;

namespace SuperWizardPlatformer
{
    class Player : IEntity, IDrawable
    {
        private Vector2 halfSize;
        private Sprite sprite;

        public Player(Body body, TextureRegion2D textureRegion)
        {
            if (body == null) { throw new ArgumentNullException(nameof(body)); }
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            Body = body;
            Body.UserData = this;
            sprite = new Sprite(textureRegion);
        }

        public Player(World world, TiledObject obj, TextureRegion2D textureRegion)
        {
            if (world == null) { throw new ArgumentNullException(nameof(world)); }
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }
            if (textureRegion == null) { throw new ArgumentNullException(nameof(textureRegion)); }

            sprite = new Sprite(textureRegion);

            float bodyWidth = ConvertUnits.ToSimUnits(textureRegion.Width);
            float bodyHeight = ConvertUnits.ToSimUnits(textureRegion.Height);

            Body = BodyFactory.CreateRectangle(world, bodyWidth, bodyHeight, 1.0f);

            halfSize = new Vector2(bodyWidth / 2.0f, bodyHeight / 2.0f);

            // Note that for TiledObjects of type 'Tiled', obj.Y is the BOTTOM of the rectangle.
            var bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                obj.X + halfSize.X,
                obj.Y - halfSize.Y));

            Body.Position = ConvertUnits.ToSimUnits(new Vector2(obj.X, obj.Y));

            Body.BodyType = BodyType.Dynamic;
            Body.OnCollision += ContactListener.OnCollision;
            Body.FixedRotation = true;
            Body.UserData = this;
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
            if (IsVisible)
            {
                double posX = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.X));
                double posY = Math.Round(ConvertUnits.ToDisplayUnits(Body.Position.Y));

                sprite.Position = new Vector2((float)posX, (float)posY);

                spriteBatch.Draw(sprite);
            }
        }

        public bool OnCollision(IEntity other)
        {
            other.IsMarkedForRemoval = true;
            return false;
        }
    }
}
