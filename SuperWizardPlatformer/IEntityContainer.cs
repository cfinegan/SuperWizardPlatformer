using FarseerPhysics.Dynamics;
using System.Collections.Generic;

namespace SuperWizardPlatformer
{
    interface IEntityContainer
    {
        List<IEntity> Entities { get; }

        List<IDrawable> Drawables { get; }

        World PhysicsWorld { get; }
    }
}
