using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    class EntityAllocator
    {
        private World physicsWorld;

        private List<IEntity> allocatedEntities = new List<IEntity>();

        public EntityAllocator(World physicsWorld)
        {
            if (physicsWorld == null) { throw new ArgumentNullException(nameof(physicsWorld)); }
            
            this.physicsWorld = physicsWorld;
        }

        public int EntityCount
        {
            get
            {
                return allocatedEntities.Count;
            }
        }

        public List<IEntity> PopulateScene(TiledMap map)
        {
            if (map == null) { throw new ArgumentNullException(nameof(map)); }

            bool playerFound = false;

            foreach (var objGroup in map.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    Console.Write(GetLoggerInfo(obj));

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

        /// <summary>
        /// Gets a string which contains any information about this TiledObject which should be logged.
        /// </summary>
        /// <param name="obj">The TiledObject being logged.</param>
        /// <returns>A string containing diagnostic information about the object.</returns>
        private static string GetLoggerInfo(TiledObject obj)
        {
            var desc = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(obj.Name)) { desc.AppendFormat("'{0}' | ", obj.Name); }

            desc.AppendFormat("ObjectType: {0} | ", obj.ObjectType);

            if (!string.IsNullOrWhiteSpace(obj.Type)) { desc.AppendFormat("Type: {0} | ", obj.Type); }

            desc.AppendFormat("Width: {0} | Height: {1} | Rotation: {2} | Visible: {3}",
                obj.Width, obj.Height, obj.Rotation, obj.IsVisible);

            desc.AppendLine();

            foreach (var entry in obj.Properties)
            {
                var line = string.Format("\t{0}: {1}", entry.Key, entry.Value);
                desc.AppendLine(line);
            }

            return desc.ToString();
        }
    }
}
