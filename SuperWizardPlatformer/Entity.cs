using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Diagnostics;

namespace SuperWizardPlatformer
{
    abstract class Entity : IEntity
    {
        public Entity(World world, TiledObject obj)
        {
            if (world == null) { throw new ArgumentNullException(nameof(world)); }
            if (obj == null) { throw new ArgumentNullException(nameof(obj)); }

            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(obj.Width),
                ConvertUnits.ToSimUnits(obj.Height), obj.GetDensity(), this);

            Body.OnCollision += ContactListener.OnCollision;
            Body.FixedRotation = true;
        }

        public Body Body { get; private set; }

        public bool IsMarkedForRemoval { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public abstract void Update(IScene scene, GameTime gameTime);

        public abstract bool OnCollision(IEntity other);
    }
}
