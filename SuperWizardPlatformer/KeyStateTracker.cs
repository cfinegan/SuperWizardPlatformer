using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    static class KeyStateTracker
    {
        // Get the total number of keys recognized by the framework for use as array bound.
        private static readonly int NUM_KEYS = Enum.GetValues(typeof(Keys)).Cast<int>().Max() + 1;

        // Each item in array should default to 'false' in both fields.
        private static KeyState[] keys = new KeyState[NUM_KEYS];

        private static bool isAltEnterPressedThisFrame = false;
        private static bool isAltEnterPressedLastFrame = false;

        public static void Update()
        {
            // Set 'PressedLastFrame' field for each key, and clear values for current frame.
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i].PressedLastFrame = keys[i].PressedThisFrame;
                keys[i].PressedThisFrame = false;
            }

            // Set 'PressedThisFrame' field for each key that is held down.
            foreach (var k in Keyboard.GetState().GetPressedKeys())
            {
                keys[(int)k].PressedThisFrame = true;
            }

            // If user typed Alt+Enter, then set IsAltEnterJustPressed property, but discard the 
            // 'enter' keystroke, so that pressing Alt+Enter doesn't accidentally cause the game
            // to confirm menu selections or do other things that may be bound to 'enter'.
            isAltEnterPressedLastFrame = isAltEnterPressedThisFrame;
            isAltEnterPressedThisFrame = false;
            if ((IsPressed(Keys.LeftAlt) || IsPressed(Keys.RightAlt)) && JustPressed(Keys.Enter))
            {
                isAltEnterPressedThisFrame = true;
                keys[(int)Keys.Enter].PressedThisFrame = false;
            }
        }

        public static bool IsAltEnterJustPressed
        {
            get
            {
                return isAltEnterPressedThisFrame && (!isAltEnterPressedLastFrame);
            }
        }

        public static bool IsPressed(Keys key)
        {
            return keys[(int)key].PressedThisFrame;
        }

        public static bool JustPressed(Keys key)
        {
            return keys[(int)key].PressedThisFrame && (!keys[(int)key].PressedLastFrame);
        }

        public static bool JustReleased(Keys key)
        {
            return (!keys[(int)key].PressedThisFrame) && keys[(int)key].PressedLastFrame;
        }

        private struct KeyState
        {
            public bool PressedThisFrame;
            public bool PressedLastFrame;
        }
    }
}
