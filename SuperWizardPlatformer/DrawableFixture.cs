using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperWizardPlatformer
{
    class DrawableFixture : IEntity
    {
        public DrawableFixture(Vector2 position, Vector2 size, bool isVisible = true)
        {
            if (position == null) { throw new ArgumentNullException("position"); }
            if (size == null) { throw new ArgumentNullException("size"); }

            Position = position;
            Size = size;
            IsVisible = isVisible;
        }

        public IDrawable Drawable { get; set; }

        public bool IsVisible { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

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
    }
}
