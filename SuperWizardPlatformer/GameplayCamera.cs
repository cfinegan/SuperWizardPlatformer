using FarseerPhysics;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace SuperWizardPlatformer
{
    class GameplayCamera : Camera2D
    {
        private IEntity toFollow;

        public GameplayCamera(GraphicsDevice graphics, IEntity toFollow = null) 
            : base(graphics)
        {
            this.toFollow = toFollow;
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
            }
        }
    }
}
