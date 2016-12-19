﻿using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;

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

        public GameObjectFactory(IScene scene, SpriteBatch spriteBatch)
        {
            entities = scene.Entities;
            drawables = scene.Drawables;
            physicsWorld = scene.PhysicsWorld;
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

                    switch (obj.ObjectType)
                    {
                        case TiledObjectType.Tile:
                            if (obj.Gid == null)
                            {
                                var errMsg = string.Format("obj.Gid (ID: {0}, Name: {1})", obj.Id, obj.Name);
                                throw new ArgumentNullException(errMsg);
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
                                    Console.WriteLine("Unrecognized physics value of '{0}' - Defaulting to {1}", physics, body.BodyType);
                                    break;
                            }

                            body.FixedRotation = true;

                            var entity = new DynamicBody(body, new Vector2(bodyWidth, bodyHeight));
                            var drawable = new TextureDrawable(entity, map.GetTileRegion((int)obj.Gid));

                            body.UserData = entity;
                            entity.Drawable = drawable;

                            entities.Add(entity);
                            drawables.Add(drawable);
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
                            Console.Write("[GameObjectFactory] Warning! ");
                            Console.WriteLine("Discarding unsupported object of type {1}",  
                                obj.ObjectType);
                            break;
                    }
                }
            }
        }
    }
}
