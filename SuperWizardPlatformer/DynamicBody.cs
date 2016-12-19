using System;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace SuperWizardPlatformer
{
    class DynamicBody : IEntity
    {
        public DynamicBody(Body body, Vector2 size, bool isVisible = true)
        {
            if (body == null) { throw new ArgumentNullException(nameof(body)); }
            if (size == null) { throw new ArgumentNullException(nameof(size)); }

            IsVisible = isVisible;
            Body = body;
            Size = size;
        }

        public Body Body { get; private set; }

        public IDrawable Drawable { get; set; }

        public bool IsMarkedForRemoval { get; set; } = false;

        public bool IsVisible { get; set; }

        public Vector2 Size { get; private set; }

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

        public void Update(IScene scene, GameTime gameTime)
        {
            // Do nothing for now.
        }
    }
}
