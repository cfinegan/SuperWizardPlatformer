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
        private enum States
        {
            Standing,
            Jumping,
            Falling,
            Walking
        }

        private const float WALK_IMPULSE = 0.0035f;
        private const float JUMP_IMPULSE = 0.01f;
        private const int JUMP_FRAMES = 15;

        private States state = States.Standing;
        private int jumpFramesRemaining = 0;

        public Player(World world, TiledObject obj, TextureRegion2D textureRegion)
            : base(world, obj, textureRegion)
        {
            Body.BodyType = BodyType.Dynamic;
        }

        private void StandingUpdate()
        {
            if (ActionMapper.JustPressed(UserAction.Jump))
            {
                Jump();
            }
        }

        private void Jump()
        {
            jumpFramesRemaining = JUMP_FRAMES;
            state = States.Jumping;
            JumpingUpdate();
        }

        private void JumpingUpdate()
        {
            Body.ApplyLinearImpulse(new Vector2(0, -JUMP_IMPULSE));

            if (--jumpFramesRemaining <= 0 || !ActionMapper.IsPressed(UserAction.Jump))
            {
                state = States.Falling;
            }
        }

        private void FallingUpdate()
        {
            if (Body.LinearVelocity.Y == 0)
            {
                state = States.Standing;
            }
        }

        private void WalkingUpdate()
        {
            StandingUpdate();
        }

        public override void Update(IScene scene, GameTime gameTime)
        { 
            switch (state)
            {
                case States.Standing:
                    StandingUpdate();
                    break;
                case States.Jumping:
                    JumpingUpdate();
                    break;
                case States.Falling:
                    FallingUpdate();
                    break;
                case States.Walking:
                    WalkingUpdate();
                    break;
            }

            float xVel = 0;

            if (ActionMapper.IsPressed(UserAction.MoveLeft))
            {
                xVel -= WALK_IMPULSE;
            }

            if (ActionMapper.IsPressed(UserAction.MoveRight))
            {
                xVel += WALK_IMPULSE;
            }

            Body.ApplyLinearImpulse(new Vector2(xVel, 0));
        }

        public override bool OnCollision(IEntity other)
        {
            other.IsVisible = false;
            return true;
        }

        public override void OnSeparation(IEntity other)
        {
            other.IsVisible = true;
        }
    }
}
