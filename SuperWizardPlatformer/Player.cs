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
        private const float MAX_HORIZONTAL_SPEED = 1.0f;
        private const float JUMP_IMPULSE = 0.01f;
        private const int JUMP_FRAMES = 13;

        private States state = States.Standing;
        private int jumpFramesRemaining = 0;
        private Vector2 lastVelocity;

        public Player(World world, TiledObject obj, TextureRegion2D textureRegion)
            : base(world, obj, textureRegion)
        {
            Body.BodyType = BodyType.Dynamic;
            lastVelocity = Body.LinearVelocity;
        }

        public int CoinsCollected { get; set; } = 0;

        private void StandingUpdate()
        {
            if (ActionMapper.IsPressed(UserAction.Jump))
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
            if (Body.LinearVelocity.Y == 0 && lastVelocity.Y > 0)
            {
                state = States.Standing;
                StandingUpdate();
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

            if (Body.LinearVelocity.X > MAX_HORIZONTAL_SPEED)
            {
                Body.LinearVelocity = new Vector2(MAX_HORIZONTAL_SPEED, Body.LinearVelocity.Y);
            }
            else if (Body.LinearVelocity.X < -MAX_HORIZONTAL_SPEED)
            {
                Body.LinearVelocity = new Vector2(-MAX_HORIZONTAL_SPEED, Body.LinearVelocity.Y);
            }

            lastVelocity = Body.LinearVelocity;
        }

        public override bool OnCollision(IEntity other)
        {
            return true;
        }

        public override void OnSeparation(IEntity other)
        {
            return;
        }
    }
}
