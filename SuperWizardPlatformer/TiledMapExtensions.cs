using MonoGame.Extended.Maps.Tiled;
using System;

namespace SuperWizardPlatformer
{
    static class TiledMapExtensions
    {
        public static void WriteInfo(this TiledMap map)
        {
            Console.WriteLine("Background Color: {0}", map.BackgroundColor);
            Console.WriteLine("Width: {0}px / Height {1}px", map.WidthInPixels, map.HeightInPixels);
            Console.WriteLine("Tile Width {0}px / Height {1}px", map.TileWidth, map.TileHeight);
            Console.WriteLine("Orientation: {0}", map.Orientation);
            Console.WriteLine("Render Order: {0}", map.RenderOrder);

            if (map.Properties.Count > 0)
            {
                Console.WriteLine("Map properties:");
                WriteTiledProperties(map.Properties);
            }

            foreach (var objGroup in map.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    WriteObjectInfo(obj);
                }
            }
        }

        private static void WriteTiledProperties(TiledProperties props)
        {
            foreach (var entry in props)
            {
                Console.WriteLine("\t{0}: {1}", entry.Key, entry.Value);
            }
        }

        private static void WriteObjectInfo(TiledObject obj)
        {
            // Write object name.
            if (!string.IsNullOrWhiteSpace(obj.Name))
            {
                Console.Write("'{0}': ", obj.Name);
            }
            else
            {
                Console.Write("Unnamed Object: ");
            }

            // Write object type.
            if (!string.IsNullOrWhiteSpace(obj.Type))
            {
                Console.Write("Type: {0} | ", obj.Type);
            }
            else
            {
                Console.Write("Type: Unspecified | ");
            }

            // Write other object specs.
            Console.Write("ObjectType: {0} | ", obj.ObjectType);
            Console.Write("Pos: ({0}, {1}) | ", obj.X, obj.Y);
            Console.Write("Width: {0}px | Height: {0}px | ", obj.Width, obj.Height);
            Console.Write("Rotation: {0} | ", obj.Rotation);
            Console.Write("Visible: {0} | ", obj.IsVisible);
            Console.Write("Gid: {0}", obj.Gid?.ToString() ?? "null");
            Console.WriteLine();
            
            // Write object properties.
            if (obj.Properties.Count > 0)
            {
                WriteTiledProperties(obj.Properties);
            }
            
        }
    }
}
