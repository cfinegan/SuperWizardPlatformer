using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SuperWizardPlatformer
{
    interface IScene : IDisposable
    {
        World PhysicsWorld { get; }

        List<IEntity> Entities { get; }

        List<IDrawable> Drawables { get; }

        bool IsReadyToQuit { get; }

        bool IsDisposed { get; }

        void Update(GameTime gameTime);

        void Draw(GraphicsDevice graphicsDevice, GameTime gameTime);
    }
}