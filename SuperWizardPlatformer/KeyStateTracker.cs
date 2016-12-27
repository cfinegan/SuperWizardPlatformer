using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    static class KeyStateTracker
    {
        private static readonly int NUM_KEYS = Enum.GetValues(typeof(Keys)).Cast<int>().Max() + 1;

        private static KeyState[] keys = new KeyState[NUM_KEYS];

        public static void Update()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i].PressedLastFrame = keys[i].PressedThisFrame;
                keys[i].PressedThisFrame = false;
            }

            foreach (var k in Keyboard.GetState().GetPressedKeys())
            {
                keys[(int)k].PressedThisFrame = true;
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
