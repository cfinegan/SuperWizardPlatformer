using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

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
                    if (obj.Type.ToLower().Equals("player"))
                    {
                        CreatePlayer(obj, map.GetTileRegion((int)obj.Gid));
                    }
                    else if (obj.ObjectType == TiledObjectType.Tile)
                    {
                        CreateGenericDrawable(obj, map.GetTileRegion((int)obj.Gid));
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

        private void CreatePlayer(TiledObject obj, TextureRegion2D textureRegion)
        {
            float bodyWidth = ConvertUnits.ToSimUnits(textureRegion.Width);
            float bodyHeight = ConvertUnits.ToSimUnits(textureRegion.Height);

            var body = BodyFactory.CreateRectangle(
                physicsWorld, bodyWidth, bodyHeight, Player.Density);

            body.BodyType = BodyType.Dynamic;
            body.Position = ConvertUnits.ToSimUnits(new Vector2(obj.X, obj.Y));
            SetBodyProperties(body);

            AllocatedEntities.Add(new Player(body, textureRegion));
        }

        private void CreateGenericDrawable(TiledObject obj, TextureRegion2D textureRegion)
        {
            float bodyWidth = ConvertUnits.ToSimUnits(obj.Width);
            float bodyHeight = ConvertUnits.ToSimUnits(obj.Height);

            var body = BodyFactory.CreateRectangle(
                physicsWorld, bodyWidth, bodyHeight, GetDensity(obj));

            // Note that for TiledObjects of type 'Tile', obj.Y is the BOTTOM of the rectangle.
            var bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                obj.X + obj.Width / 2.0f,
                obj.Y - obj.Height / 2.0f));

            body.BodyType = GetBodyType(obj);
            body.Position = bodyCenter;
            SetBodyProperties(body);

            AllocatedEntities.Add(
                new DrawableEntity(body, new Vector2(bodyWidth, bodyHeight), textureRegion));
        }

        private void CreateRectangle(TiledObject obj)
        {
            float rectWidth = ConvertUnits.ToSimUnits(obj.Width);
            float rectHeight = ConvertUnits.ToSimUnits(obj.Height);

            var body = BodyFactory.CreateRectangle(
                physicsWorld, rectWidth, rectHeight, GetDensity(obj));

            // Note that for TiledObjects of type 'Rectangle', obj.Y is the TOP of the rectangle.
            var bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                obj.X + obj.Width / 2.0f,
                obj.Y + obj.Height / 2.0f));

            body.Position = bodyCenter;
            body.FixedRotation = true;
            body.BodyType = BodyType.Static;
        }

        private float GetDensity(TiledObject obj)
        {
            string strDensity = string.Empty;
            obj.Properties.TryGetValue("density", out strDensity);
            float density = 1.0f;
            float.TryParse(strDensity, out density);
            return density;
        }

        private BodyType GetBodyType(TiledObject obj)
        {
            string physics = string.Empty;
            obj.Properties.TryGetValue("physics", out physics);

            BodyType result = BodyType.Static;

            switch (physics)
            {
                case "dynamic":
                    result = BodyType.Dynamic;
                    break;
                case "kinematic":
                    result = BodyType.Kinematic;
                    break;
                case "static":
                    result = BodyType.Static;
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(physics))
                    {
                        Console.WriteLine(
                            "[{0}] Unrecognized physics value '{1}' for object '{2}'",
                            GetType().Name, physics, obj.Name);
                    }
                    break;
            }

            return result;
        }

        private void SetBodyProperties(Body body)
        {
            body.FixedRotation = true;
            body.OnCollision += ContactListener.OnCollision;
        }
    }
}
