using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace SuperWizardPlatformer
{
    class GameplayCamera : Camera2D
    {
        private IEntity toFollow;
        private Rectangle boundingRect;
        private Size size;

        public GameplayCamera(GraphicsDevice graphics, Rectangle boundingRect, Size cameraSize, 
            IEntity toFollow = null) : base(graphics)
        {
            this.toFollow = toFollow;
            this.boundingRect = boundingRect;
            size = cameraSize;
        }

        public void Follow(IEntity entity)
        {
            toFollow = entity;
        }

        public void UpdatePosition()
        {
            if (toFollow != null)
            {
                LookAt(ConvertUnits.ToDisplayUnits(toFollow.Body.Position));

                float adjustX = 0;
                float adjustY = 0;

                if (Position.X < boundingRect.X)
                {
                    adjustX = boundingRect.X - Position.X;
                }
                else if (Position.X + size.Width > boundingRect.X + boundingRect.Width)
                {
                    adjustX = boundingRect.X - Position.X + size.Width;
                }
                
                if (Position.Y < boundingRect.Y)
                {
                    adjustY = boundingRect.Y - Position.Y;
                }
                else if (Position.Y + size.Height > boundingRect.Y + boundingRect.Height)
                {
                    adjustY = boundingRect.Y - Position.Y + size.Height;
                }

                Move(new Vector2(adjustX, adjustY));
            }
        }
    }
}
