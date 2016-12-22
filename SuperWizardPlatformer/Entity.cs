using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

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

        public IDrawable Drawable { get; set; }

        public bool IsMarkedForRemoval { get; set; } = false;

        public bool IsVisible { get; set; }

        public Vector2 Position
        {
            get
            {
                return Body.Position;
            }

            set
            {
                Body.Position = value;
            }
        }

        public Vector2 Size { get; private set; }

        public abstract void Update(IScene scene, GameTime gameTime);
    }
}
