using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.TextureAtlases;

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
        private List<IEntity> entities;
        private List<IDrawable> drawables;
        private World physicsWorld;

        public GameObjectFactory(IScene scene)
        {
            if (scene == null) { throw new ArgumentNullException(nameof(scene)); }

            entities = scene.Entities;
            drawables = scene.Drawables;
            physicsWorld = scene.PhysicsWorld;
        }

        public void CreatePlayer(Vector2 position, TextureRegion2D textureRegion)
        {
            float bodyWidth = ConvertUnits.ToSimUnits(textureRegion.Width);
            float bodyHeight = ConvertUnits.ToSimUnits(textureRegion.Height);
            var body = BodyFactory.CreateRectangle(physicsWorld, bodyWidth, bodyHeight, 1.0f);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Position = ConvertUnits.ToSimUnits(position);
            body.OnCollision += ContactListener.OnCollision;

            Player player = new Player(body, textureRegion);

            entities.Add(player);
            drawables.Add(player);
        }

        public void PopulateScene(TiledMap map)
        {
            const float DENSITY_DEFAULT = 1.0f;
            const string PHYSICS_DEFAULT = "static";

            foreach (var objGroup in map.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    // Get density from properties (or default).
                    float density = DENSITY_DEFAULT;
                    string strDensity = DENSITY_DEFAULT.ToString();
                    obj.Properties.TryGetValue("density", out strDensity);
                    float.TryParse(strDensity, out density);

                    // Get physics from properties (or default).
                    string physics;
                    if (!obj.Properties.TryGetValue("physics", out physics))
                    {
                        physics = PHYSICS_DEFAULT;
                    }

                    if (obj.Type.ToLower().Equals("player"))
                    {
                        var position = new Vector2(obj.X, obj.Y);
                        var textureRegion = map.GetTileRegion((int)obj.Gid);
                        CreatePlayer(position, textureRegion);
                    }
                    else
                    {
                        switch (obj.ObjectType)
                        {
                            case TiledObjectType.Tile:
                                if (obj.Gid == null)
                                {
                                    throw new ArgumentNullException(
                                        "obj.Gid", string.Format("ID: {0}, Name: {1}", obj.Id, obj.Name));
                                }

                                // Note that for TiledObjects of type Tile, obj.Y is the BOTTOM of the rectangle.
                                var bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                                    obj.X + obj.Width / 2.0f,
                                    obj.Y - obj.Height / 2.0f));

                                float bodyWidth = ConvertUnits.ToSimUnits(obj.Width);
                                float bodyHeight = ConvertUnits.ToSimUnits(obj.Height);
                                var body = BodyFactory.CreateRectangle(physicsWorld, bodyWidth, bodyHeight, density, bodyCenter);

                                switch (physics)
                                {
                                    case "dynamic":
                                        body.BodyType = BodyType.Dynamic;
                                        break;
                                    case "kinematic":
                                        body.BodyType = BodyType.Kinematic;
                                        break;
                                    case "static":
                                        body.BodyType = BodyType.Static;
                                        break;
                                    default:
                                        // This should only happen if the physics value read from the TMX data is invalid.
                                        body.BodyType = BodyType.Static;
                                        Console.Write("[{0}] Warning! ", GetType().Name);
                                        Console.WriteLine("Unrecognized physics value of '{0}' - Defaulting to {1}", physics, body.BodyType);
                                        break;
                                }

                                body.FixedRotation = true;
                                body.OnCollision += ContactListener.OnCollision;

                                var entity = new DrawableEntity(body, new Vector2(bodyWidth, bodyHeight), map.GetTileRegion((int)obj.Gid));

                                entities.Add(entity);
                                drawables.Add(entity);
                                break;

                            case TiledObjectType.Rectangle:
                                float rectWidth = ConvertUnits.ToSimUnits(obj.Width);
                                float rectHeight = ConvertUnits.ToSimUnits(obj.Height);
                                body = BodyFactory.CreateRectangle(physicsWorld, rectWidth, rectHeight, density);

                                // Note that for Rectangle TiledObjects, obj.Y is the TOP of the rectangle.
                                bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                                    obj.X + obj.Width / 2.0f,
                                    obj.Y + obj.Height / 2.0f));

                                body.Position = bodyCenter;

                                body.BodyType = BodyType.Static;
                                break;

                            default:
                                Console.Write("[{0}] Warning! ", GetType().Name);
                                Console.WriteLine("Discarding unsupported object of type {0}",
                                    obj.ObjectType);
                                break;
                        }
                    }
                }
            }
        }
    }
}
