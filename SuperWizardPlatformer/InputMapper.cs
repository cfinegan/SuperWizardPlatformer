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
            // Gameplay key mappings
            // TODO: Make configurable by user.
            keyMappings[(int)UserAction.None] = Keys.None;
            keyMappings[(int)UserAction.MoveLeft] = Keys.Left;
            keyMappings[(int)UserAction.MoveRight] = Keys.Right;
            keyMappings[(int)UserAction.Jump] = Keys.Up;
            keyMappings[(int)UserAction.Duck] = Keys.Down;

            // Menu key mappings
            // These should stay hard-coded so that users can't break the menus.
            keyMappings[(int)UserAction.MenuDown] = Keys.Down;
            keyMappings[(int)UserAction.MenuUp] = Keys.Up;
            keyMappings[(int)UserAction.MenuLeft] = Keys.Left;
            keyMappings[(int)UserAction.MenuRight] = Keys.Right;
            keyMappings[(int)UserAction.MenuAccept] = Keys.Enter;
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
