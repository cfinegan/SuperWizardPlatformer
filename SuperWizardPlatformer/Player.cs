using System;
using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    class Player : IEntity
    {
        public Player(Vector2 position, Vector2 size, bool isVisible = true)
        {
            if (position == null) { throw new ArgumentNullException("position"); }
            if (size == null) { throw new ArgumentNullException("size"); }

            Position = position;
            Size = size;
        }

        public bool IsVisible { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public IDrawable drawable { get; set; }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
