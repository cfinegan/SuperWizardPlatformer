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

        public GameObjectFactory(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public Tuple<List<IEntity>, List<IDrawable>> CreateScene(TiledMap map, World physicsWorld)
        {
            const float DENSITY_DEFAULT = 1.0f;

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

                    switch (obj.ObjectType)
                    {
                        case TiledObjectType.Tile:
                            if (obj.Gid == null) { throw new ArgumentNullException("obj.Gid"); }

                            Console.WriteLine("Pre-mult body size: {0}, {1}", obj.Width, obj.Height);
                            float bodyWidth = ConvertUnits.ToSimUnits(obj.Width);
                            float bodyHeight = ConvertUnits.ToSimUnits(obj.Height);
                            Console.WriteLine("Body Size: {0}, {1}", bodyWidth, bodyHeight);
                            var body = BodyFactory.CreateRectangle(physicsWorld, bodyWidth, bodyHeight, density);

                            body.BodyType = BodyType.Dynamic;
                            body.Position = new Vector2(ConvertUnits.ToSimUnits(obj.X), ConvertUnits.ToSimUnits(obj.Y));

                            var entity = new DynamicBody(body, new Vector2(bodyWidth, bodyHeight));
                            var drawable = new DrawableTexture(entity, map.GetTileRegion((int)obj.Gid), spriteBatch);

                            entity.Drawable = drawable;
                            entities.Add(entity);
                            drawables.Add(drawable);
                            break;

                        case TiledObjectType.Rectangle:
                            float rectWidth = ConvertUnits.ToSimUnits(obj.Width);
                            float rectHeight = ConvertUnits.ToSimUnits(obj.Height);
                            body = BodyFactory.CreateRectangle(physicsWorld, rectWidth, rectHeight, density);

                            body.BodyType = BodyType.Static;

                            float rectX = ConvertUnits.ToSimUnits(obj.X);
                            float rectY = ConvertUnits.ToSimUnits(obj.Y);
                            body.Position = new Vector2(rectX, rectY);
                            break;

                        default:
                            break;
                    }
                }
            }

            return Tuple.Create(entities, drawables);
        }
    }
}
