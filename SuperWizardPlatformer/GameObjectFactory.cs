using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperWizardPlatformer
{
    class GameObjectFactory
    {
        private SpriteBatch spriteBatch;
        private World physicsWorld;

        public GameObjectFactory(World physicsWorld, SpriteBatch spriteBatch)
        {
            this.physicsWorld = physicsWorld;
            this.spriteBatch = spriteBatch;
        }

        public Tuple<List<IEntity>, List<IDrawable>> CreateScene(TiledMap map)
        {
            const float DENSITY_DEFAULT = 1.0f;
            const string PHYSICS_DEFAULT = "static";

            var entities = new List<IEntity>();
            var drawables = new List<IDrawable>();

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
                            if (obj.Gid == null) { throw new ArgumentNullException("obj.Gid"); }

                            // Note that for TiledObjects of type Tile, obj.Y is the BOTTOM of the rectangle.
                            var bodyCenter = ConvertUnits.ToSimUnits(new Vector2(
                                obj.X + obj.Width / 2.0f, 
                                obj.Y - obj.Height / 2.0f));

                            float bodyWidth = ConvertUnits.ToSimUnits(obj.Width);
                            float bodyHeight = ConvertUnits.ToSimUnits(obj.Height);
                            var body = BodyFactory.CreateRectangle(physicsWorld, bodyWidth, bodyHeight, density);

                            switch (physics)
                            {
                                case "dynamic":
                                    body.BodyType = BodyType.Dynamic;
                                    break;
                                case "kinematic":
                                    body.BodyType = BodyType.Kinematic;
                                    break;
                                case "static":
                                default:
                                    body.BodyType = BodyType.Static;
                                    break;
                            }


                            body.FixedRotation = true;
                            body.Position = bodyCenter;

                            var entity = new DynamicBody(body, new Vector2(bodyWidth, bodyHeight));
                            var drawable = new TextureDrawable(entity, map.GetTileRegion((int)obj.Gid), spriteBatch);

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
                            Console.WriteLine(
                                "[GameObjectFactory] Warning! Discarding unsupported {0} of type {1}", 
                                nameof(obj.ObjectType), obj.ObjectType);
                            break;
                    }
                }
            }

            return Tuple.Create(entities, drawables);
        }
    }
}
