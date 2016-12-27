using System;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace SuperWizardPlatformer
{
    static class InputMapper
    {
        private static readonly int NUM_ACTIONS =
            Enum.GetValues(typeof(UserAction)).Cast<int>().Max() + 1;

        private static Keys[] keyMappings = new Keys[NUM_ACTIONS];

        public static void Initialize()
        {
            keyMappings[(int)UserAction.None] = Keys.None;
            keyMappings[(int)UserAction.MoveLeft] = Keys.Left;
            keyMappings[(int)UserAction.MoveRight] = Keys.Right;
            keyMappings[(int)UserAction.Jump] = Keys.Up;
            keyMappings[(int)UserAction.Duck] = Keys.Down;
        }

        public static bool IsPressed(UserAction action)
        {
            return KeyStateTracker.IsPressed(keyMappings[(int)action]);
        }

        public static bool JustPressed(UserAction action)
        {
            return KeyStateTracker.JustPressed(keyMappings[(int)action]);
        }

        public static bool JustReleased(UserAction action)
        {
            return KeyStateTracker.JustReleased(keyMappings[(int)action]);
        }
    }
}
