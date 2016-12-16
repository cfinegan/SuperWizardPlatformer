using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperWizardPlatformer
{
    class Player : IEntity
    {
        public Player(Vector2 position, Vector2 size, bool isVisible = true)
        {
            if (position == null) { throw new ArgumentNullException(nameof(position)); }
            if (size == null) { throw new ArgumentNullException(nameof(size)); }

            Position = position;
            Size = size;
        }

        public bool IsVisible { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; private set; }

        public IDrawable drawable { get; set; }

        public void Update(GameTime gameTime)
        {
            int xVel = 0, yVel = 0;
            int velFactor = 3;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                xVel -= velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                xVel += velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                yVel -= velFactor;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                yVel += velFactor;
            }

            Position = new Vector2(Position.X + xVel, Position.Y + yVel);
        }

        public void RemoveFrom(IScene scene)
        {

        }
    }
}
