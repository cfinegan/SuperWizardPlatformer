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
    /// Used to insert new game objects into an existing scene, for the purposes of both constructing
    /// the scene and adding new objects to it during gameplay. The eventual goal of this class is
    /// for all garbage to be generated during construction and the PopulateScene method, so that
    /// individual calls to methods that insert new objects do not trigger GC collections.
    /// </summary>
    class GameObjectFactory
    {
        private World physicsWorld;

        public GameObjectFactory(IScene scene)
        {
            if (scene == null) { throw new ArgumentNullException(nameof(scene)); }
            
            physicsWorld = scene.PhysicsWorld;
        }

        public List<IEntity> AllocatedEntities { get; private set; } = new List<IEntity>();

        public void PopulateScene(TiledMap map)
        {
            foreach (var objGroup in map.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    Console.Write(GetLoggerInfo(obj));

                    if ("player".Equals(obj.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        AllocatedEntities.Add(new Player(
                            physicsWorld, obj, map.GetTileRegion((int)obj.Gid)));
                    }
                    else if (obj.ObjectType == TiledObjectType.Tile)
                    {
                        AllocatedEntities.Add(new DrawableEntity(
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
