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
                // Make sure to round pixel values after converting from world units.
                // Otherwise subtle camera bugs manifest (maybe library default is flooring them?).
                double bodyPosX = Math.Round(ConvertUnits.ToDisplayUnits(toFollow.Body.Position.X));
                double bodyPosY = Math.Round(ConvertUnits.ToDisplayUnits(toFollow.Body.Position.Y));
                LookAt(new Vector2((float)bodyPosX, (float)bodyPosY));

                float adjustX = 0;
                float adjustY = 0;

                if (Position.X < boundingRect.X)
                {
                    adjustX = boundingRect.X - Position.X;
                }
                else if (Position.X + size.Width > boundingRect.X + boundingRect.Width)
                {
                    adjustX = boundingRect.X + boundingRect.Width - (Position.X + size.Width);
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
