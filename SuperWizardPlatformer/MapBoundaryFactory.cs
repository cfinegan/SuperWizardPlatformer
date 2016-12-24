using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// Creates map boundaries so that physics objects don't go flying off the map into
    /// and endless void. These boundaries are basic physics rectangles that enclose the
    /// map, but don't have any entities associated with them.
    /// </summary>
    static class MapBoundaryFactory
    {
        /// <summary>
        /// Creates boundaries for the top, bottom, left-hand side, and right-hand side of
        /// the map.
        /// </summary>
        /// <param name="physicsWorld">Physics world to register the boundaries with.</param>
        /// <param name="map">Map info used for placement and sizing of the boundary objects.</param>
        public static void CreateAllBoundaries(World physicsWorld, TiledMap map)
        {
            CreateLeftBoundary(physicsWorld, map);
            CreateRightBoundary(physicsWorld, map);
            CreateTopBoundary(physicsWorld, map);
            CreateBottomBoundary(physicsWorld, map);
        }

        /// <summary>
        /// Creates a boundary for the left-hand side of the map.
        /// </summary>
        /// <param name="physicsWorld">Physics world to register the boundary with.</param>
        /// <param name="map">Map info used for placement and sizing of the boundary object.</param>
        public static void CreateLeftBoundary(World physicsWorld, TiledMap map)
        {
            float width = 10.0f;
            float height = ConvertUnits.ToSimUnits(map.HeightInPixels);

            var body = BodyFactory.CreateRectangle(physicsWorld, width, height, 1.0f);
            body.BodyType = BodyType.Static;
            body.Position = new Vector2(0 - (width * 0.5f), height * 0.5f);
        }

        /// <summary>
        /// Creates a boundary for the right-hand side of the map.
        /// </summary>
        /// <param name="physicsWorld">Physics world to register the boundary with.</param>
        /// <param name="map">Map info used for placement and sizing of the boundary object.</param>
        public static void CreateRightBoundary(World physicsWorld, TiledMap map)
        {
            float mapWidth = ConvertUnits.ToSimUnits(map.WidthInPixels);
            float width = 10.0f;
            float height = ConvertUnits.ToSimUnits(map.HeightInPixels);

            var body = BodyFactory.CreateRectangle(physicsWorld, width, height, 1.0f);
            body.BodyType = BodyType.Static;
            body.Position = new Vector2(mapWidth + (width * 0.5f), height * 0.5f);
        }

        /// <summary>
        /// Creates a boundary for the top of the map.
        /// </summary>
        /// <param name="physicsWorld">Physics world to register the boundary with.</param>
        /// <param name="map">Map info used for placement and sizing of the boundary object.</param>
        public static void CreateTopBoundary(World physicsWorld, TiledMap map)
        {
            float width = ConvertUnits.ToSimUnits(map.WidthInPixels);
            float height = 10.0f;

            var body = BodyFactory.CreateRectangle(physicsWorld, width, height, 1.0f);
            body.BodyType = BodyType.Static;
            body.Position = new Vector2(width * 0.5f, 0 - (height * 0.5f));
        }

        /// <summary>
        /// Creates a boundary for the bottom of the map.
        /// </summary>
        /// <param name="physicsWorld">Physics world to register the boundary with.</param>
        /// <param name="map">Map info used for placement and sizing of the boundary object.</param>
        public static void CreateBottomBoundary(World physicsWorld, TiledMap map)
        {
            float mapHeight = ConvertUnits.ToSimUnits(map.HeightInPixels);
            float width = ConvertUnits.ToSimUnits(map.WidthInPixels);
            float height = 10.0f;

            var body = BodyFactory.CreateRectangle(physicsWorld, width, height, 1.0f);
            body.BodyType = BodyType.Static;
            body.Position = new Vector2(width * 0.5f, mapHeight + (height * 0.5f));
        }
    }
}
