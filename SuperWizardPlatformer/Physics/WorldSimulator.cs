using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SuperWizardPlatformer.Physics
{
    class WorldSimulator
    {
        public Vector2 Gravity { get; set; }

        public WorldSimulator(Vector2 gravity)
        {
            Gravity = gravity;
        }

        public void Update(float updateTimeMs, IReadOnlyList<IPhysicsObject> objects)
        {
            throw new NotImplementedException();
        }
    }
}
