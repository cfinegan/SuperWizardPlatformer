using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    static class InputMapper
    {
        private static readonly int NUM_ACTIONS =
            Enum.GetValues(typeof(UserAction)).Cast<int>().Max() + 1;

        private static KeyPair[] keyMappings = new KeyPair[NUM_ACTIONS];

        private static GamePadButtons[] padMappings = new GamePadButtons[NUM_ACTIONS];

        public static void Initialize()
        {
            // Gameplay key mappings.
            // TODO: Make configurable by user.
            keyMappings[(int)UserAction.MoveLeft] = new KeyPair(Keys.Left);
            keyMappings[(int)UserAction.MoveRight] = new KeyPair(Keys.Right);
            keyMappings[(int)UserAction.Jump] = new KeyPair(Keys.Space, Keys.Up);
            keyMappings[(int)UserAction.Duck] = new KeyPair(Keys.Down);

            AddMenuKeyMappings();
            AddGamePadMappings();
        }

        private static void AddMenuKeyMappings()
        {
            // Menu key mappings.
            // These should stay hard-coded so that users can't break the menus.
            keyMappings[(int)UserAction.MenuDown] = new KeyPair(Keys.Down);
            keyMappings[(int)UserAction.MenuUp] = new KeyPair(Keys.Up);
            keyMappings[(int)UserAction.MenuLeft] = new KeyPair(Keys.Left);
            keyMappings[(int)UserAction.MenuRight] = new KeyPair(Keys.Right);
            keyMappings[(int)UserAction.MenuAccept] = new KeyPair(Keys.Enter, Keys.Space);
            keyMappings[(int)UserAction.MenuCancel] = new KeyPair(Keys.Escape);
        }

        private static void AddGamePadMappings()
        {
            // GamePad button mappings.
            // These can be hard-coded for now, customizable gamepad input would be nice.
            padMappings[(int)UserAction.MoveLeft] = GamePadButtons.Left;
            padMappings[(int)UserAction.MoveRight] = GamePadButtons.Right;
            padMappings[(int)UserAction.Duck] = GamePadButtons.Down;
            padMappings[(int)UserAction.Jump] = GamePadButtons.A;

            // GamePad menu button mappings, should stay hard-coded.
            padMappings[(int)UserAction.MenuUp] = GamePadButtons.Up;
            padMappings[(int)UserAction.MenuDown] = GamePadButtons.Down;
            padMappings[(int)UserAction.MenuLeft] = GamePadButtons.Left;
            padMappings[(int)UserAction.MenuRight] = GamePadButtons.Right;
            padMappings[(int)UserAction.MenuAccept] = GamePadButtons.A;
            padMappings[(int)UserAction.MenuCancel] = GamePadButtons.B;
        }

        public static bool IsPressed(UserAction action)
        {
            return KeyStateTracker.IsPressed(keyMappings[(int)action].first) || 
                KeyStateTracker.IsPressed(keyMappings[(int)action].second) ||
                GamePadStateTracker.IsPressed(padMappings[(int)action]);
        }

        public static bool JustPressed(UserAction action)
        {
            return KeyStateTracker.JustPressed(keyMappings[(int)action].first) ||
                KeyStateTracker.JustPressed(keyMappings[(int)action].second) ||
                GamePadStateTracker.JustPressed(padMappings[(int)action]);
        }

        public static bool JustReleased(UserAction action)
        {
            return KeyStateTracker.JustReleased(keyMappings[(int)action].first) ||
                KeyStateTracker.JustReleased(keyMappings[(int)action].second) ||
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
        }
    }
}
