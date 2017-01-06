using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.TextureAtlases;
using SuperWizardPlatformer.Input;
using System;

namespace SuperWizardPlatformer
{
    class Player : DrawableEntity
    {
        public Player(World world, TiledObject obj, TextureRegion2D textureRegion)
            : base(world, obj, textureRegion)
        {
            Body.BodyType = BodyType.Dynamic;
        }

        public override void Update(IScene scene, GameTime gameTime)
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

        public override bool OnCollision(IEntity other)
        {
            other.IsMarkedForRemoval = true;
            return false;
        }
    }
}
