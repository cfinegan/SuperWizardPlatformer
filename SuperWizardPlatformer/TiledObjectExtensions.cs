using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Text;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// Extensions for reading data from TiledObject objects.
    /// </summary>
    /// <seealso cref="TiledObject"/>
    static class TiledObjectExtensions
    {
        /// <summary>
        /// Gets the center point of the rectangle specified by the tiled object. Throws an
        /// InvalidOperationException if the object has an ObjectType other than 'Tile' or 
        /// 'Rectangle'.
        /// </summary>
        /// <param name="obj">The invoking TiledObject.</param>
        /// <returns>The center point of the object.</returns>
        public static Vector2 GetObjectCenter(this TiledObject obj)
        {
            Vector2 center;

            if (obj.ObjectType == TiledObjectType.Tile)
            {
                // Note that for TiledObjects of type 'Tile', obj.Y is the BOTTOM of the rectangle.
                center = new Vector2(obj.X + obj.Width / 2.0f, obj.Y - obj.Height / 2.0f);
            }
            else if (obj.ObjectType == TiledObjectType.Rectangle)
            {
                // Note that for TiledObjects of type 'Rectangle', obj.Y is the TOP of the rectangle.
                center = new Vector2(obj.X + obj.Width / 2.0f, obj.Y + obj.Height / 2.0f);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("Unsupported TiledObject of type: {0}", obj.ObjectType));
            }

            return ConvertUnits.ToSimUnits(center);
        }

        /// <summary>
        /// Gets a density value from the object, or returns the default.
        /// </summary>
        /// <param name="obj">The invoking TiledObject.</param>
        /// <returns>The density of the object.</returns>
        public static float GetDensity(this TiledObject obj)
        {
            string strDensity = string.Empty;
            float value = 1.0f;
            obj.Properties.TryGetValue("density", out strDensity);

            if (!string.IsNullOrWhiteSpace(strDensity))
            {
                float.TryParse(strDensity, out value);
            }

            return value;
        }

        /// <summary>
        /// Gets the physics mode of the object, or returns the default.
        /// </summary>
        /// <param name="obj">The invoking TiledObject.</param>
        /// <returns>The BodyType of the object.</returns>
        public static BodyType GetBodyType(this TiledObject obj)
        {
            string physics = string.Empty;
            obj.Properties.TryGetValue("physics", out physics);

            BodyType result = BodyType.Static;

            if ("dynamic".Equals(physics, StringComparison.OrdinalIgnoreCase))
            {
                result = BodyType.Dynamic;
            }
            else if ("kinematic".Equals(physics, StringComparison.OrdinalIgnoreCase))
            {
                result = BodyType.Kinematic;
            }
            else if ("static".Equals(physics, StringComparison.OrdinalIgnoreCase))
            {
                result = BodyType.Static;
            }
            else if (!string.IsNullOrWhiteSpace(physics))
            {
                Console.Write("[{0}] ", nameof(GameObjectFactory));
                Console.WriteLine("Unrecognized physics value '{0}' for object '{2}'.",
                    physics, obj.Name);
            }

            return result;
        }

        /// <summary>
        /// Gets a string which contains any information about this TiledObject which should be logged.
        /// </summary>
        /// <param name="obj">The invoking TiledObject.</param>
        /// <returns>A string containing diagnostic information.</returns>
        public static string GetLoggerInfo(this TiledObject obj)
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
