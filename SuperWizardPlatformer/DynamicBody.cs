using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace SuperWizardPlatformer
{
    class DynamicBody : IEntity
    {
        Body body;

        public DynamicBody(Body body, Vector2 size, bool isVisible = true)
        {
            if (body == null) { throw new ArgumentNullException(nameof(body)); }
            if (size == null) { throw new ArgumentNullException(nameof(size)); }

            IsVisible = isVisible;
            this.body = body;
            Size = size;
        }

        public IDrawable Drawable { get; set; }

        public bool IsVisible { get; set; }

        public Vector2 Size { get; set; }

        public Vector2 Position
        {
            get
            {
                return body.Position;
            }
            set
            {
                body.Position = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            return; // Do nothing for now.
        }
    }
}
