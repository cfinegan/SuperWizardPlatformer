using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    static class InputMapper
    {
        private static readonly int NUM_ACTIONS =
            Enum.GetValues(typeof(UserAction)).Cast<int>().Max() + 1;

        private static Keys[] keyMappings = new Keys[NUM_ACTIONS];

        private static GamePadButtons[] padMappings = new GamePadButtons[NUM_ACTIONS];

        public static void Initialize()
        {           
            // Gameplay key mappings.
            // TODO: Make configurable by user.
            keyMappings[(int)UserAction.None] = Keys.None;
            keyMappings[(int)UserAction.MoveLeft] = Keys.Left;
            keyMappings[(int)UserAction.MoveRight] = Keys.Right;
            keyMappings[(int)UserAction.Jump] = Keys.Up;
            keyMappings[(int)UserAction.Duck] = Keys.Down;

            // Menu key mappings.
            // These should stay hard-coded so that users can't break the menus.
            keyMappings[(int)UserAction.MenuDown] = Keys.Down;
            keyMappings[(int)UserAction.MenuUp] = Keys.Up;
            keyMappings[(int)UserAction.MenuLeft] = Keys.Left;
            keyMappings[(int)UserAction.MenuRight] = Keys.Right;
            keyMappings[(int)UserAction.MenuAccept] = Keys.Enter;
            keyMappings[(int)UserAction.MenuCancel] = Keys.Escape;

            // GamePad button mappings.
            // These can be hard-coded for now, customizable gamepad input would be nice.
            padMappings[(int)UserAction.MoveLeft] = GamePadButtons.Left;
            padMappings[(int)UserAction.MoveRight] = GamePadButtons.Right;
            padMappings[(int)UserAction.Duck] = GamePadButtons.Down;
            padMappings[(int)UserAction.Jump] = GamePadButtons.A;

            // GamePad menu button mappings.
            padMappings[(int)UserAction.MenuUp] = GamePadButtons.Up;
            padMappings[(int)UserAction.MenuDown] = GamePadButtons.Down;
            padMappings[(int)UserAction.MenuLeft] = GamePadButtons.Left;
            padMappings[(int)UserAction.MenuRight] = GamePadButtons.Right;
            padMappings[(int)UserAction.MenuAccept] = GamePadButtons.A;
            padMappings[(int)UserAction.MenuCancel] = GamePadButtons.B;
        }

        public static bool IsPressed(UserAction action)
        {
            return KeyStateTracker.IsPressed(keyMappings[(int)action]) || 
                GamePadStateTracker.IsPressed(padMappings[(int)action]);
        }

        public static bool JustPressed(UserAction action)
        {
            return KeyStateTracker.JustPressed(keyMappings[(int)action]) ||
                GamePadStateTracker.JustPressed(padMappings[(int)action]);
        }

        public static bool JustReleased(UserAction action)
        {
            return KeyStateTracker.JustReleased(keyMappings[(int)action]) ||
                GamePadStateTracker.JustReleased(padMappings[(int)action]);
        }

        private struct KeyPair
        {
            public Keys first;
            public Keys second;

            public KeyPair(Keys first, Keys second = Keys.None)
            {
                this.first = first;
                this.second = second;
            }

            internal static KeyPair Empty = new KeyPair
            {
                first = Keys.None,
                second = Keys.None
            };
        }
    }
}
