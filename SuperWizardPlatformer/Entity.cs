using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;

namespace SuperWizardPlatformer
{
    abstract class Entity : IEntity
    {
        public Entity(Body body, bool isVisible = true)
        {
            if (body == null) { throw new ArgumentNullException(nameof(body)); }

            IsVisible = isVisible;
            Body = body;
            Body.UserData = this;
        }

        public Body Body { get; private set; }

        public bool IsMarkedForRemoval { get; set; } = false;

        public bool IsVisible { get; set; }

        public abstract void Update(IScene scene, GameTime gameTime);

        public abstract bool OnCollision(IEntity other);
    }
}
