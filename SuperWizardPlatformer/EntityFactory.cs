using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// Allocates Entities and tracks how many have been allocated total, so that the scene knows
    /// how large to keep its buffers in case all entities are active at once.
    /// </summary>
    class EntityFactory
    {
        private World physicsWorld;

        private List<IEntity> allocatedEntities = new List<IEntity>();

        public EntityFactory(World physicsWorld)
        {
            if (physicsWorld == null) { throw new ArgumentNullException(nameof(physicsWorld)); }
            
            this.physicsWorld = physicsWorld;
        }

        public List<IEntity> PopulateScene(TiledMap map)
        {
            if (map == null) { throw new ArgumentNullException(nameof(map)); }

            bool playerFound = false;

            foreach (var objGroup in map.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    if ("player".Equals(obj.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!playerFound)
                        {
                            allocatedEntities.Add(new Player(
                                physicsWorld, obj, map.GetTileRegion((int)obj.Gid)));
                            playerFound = true;
                        }
                        else
                        {
                            throw new InvalidSceneDataException(
                                "Cannot have more than one player.", nameof(map));
                        }
                    }
                    else if ("coin".Equals(obj.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        allocatedEntities.Add(new Coin(
                            physicsWorld, obj, map.GetTileRegion((int)obj.Gid)));
                    }
                    else if (obj.ObjectType == TiledObjectType.Tile)
                    {
                        allocatedEntities.Add(new DrawableEntity(
                            physicsWorld, obj, map.GetTileRegion((int)obj.Gid)));
                    }
                    else if (obj.ObjectType == TiledObjectType.Rectangle)
                    {
                        CreateRectangle(obj);
                    }
                    else
                    {
                        Console.WriteLine("[{0}] Discarding unsupported object of type {1}.",
                            GetType().Name, obj.ObjectType);
                    }
                }
            }

            if (!playerFound)
            {
                throw new InvalidSceneDataException("Must specify player object.", nameof(map));
            }

            return allocatedEntities;
        }

        private void CreateRectangle(TiledObject obj)
        {
            float rectWidth = ConvertUnits.ToSimUnits(obj.Width);
            float rectHeight = ConvertUnits.ToSimUnits(obj.Height);

            var body = BodyFactory.CreateRectangle(
                physicsWorld, rectWidth, rectHeight, obj.GetDensity());

            body.Position = obj.GetObjectCenter();
            body.FixedRotation = true;
            body.BodyType = BodyType.Static;
        }
    }
}
