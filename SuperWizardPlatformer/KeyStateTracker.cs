using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    class KeyStateTracker
    {
        private static int NUM_KEYS = Enum.GetValues(typeof(Keys)).Cast<int>().Max() + 1;

        private KeyState[] keys = new KeyState[NUM_KEYS];

        public KeyStateTracker()
        {
            // Diagnostic info to make sure NUM_KEYS is a reasonable size.
            Console.WriteLine("Initializing {0} | {1} = {2}", 
                nameof(KeyStateTracker), nameof(NUM_KEYS), NUM_KEYS);

            // Technically may be redundant since .NET initializes all memory to zero,
            // but best to be safe.
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i] = KeyState.NullValue;
            }
        }

        public void Update()
        {
            for (int i = 0; i < keys.Length; ++i)
            {
                keys[i].JustPressed = false;
                keys[i].JustReleased = false;
            }

            for (int i = 0; i < keys.Length; ++i)
            {
                if (Keyboard.GetState().IsKeyDown((Keys)i))
                {
                    if (!keys[i].IsPressed)
                    {
                        keys[i].JustPressed = true;
                    }
                    keys[i].IsPressed = true;
                }
                else
                {
                    if (keys[i].IsPressed)
                    {
                        keys[i].JustReleased = true;
                    }
                    keys[i].IsPressed = false;
                }
            }
        }

        public bool IsPressed(Keys key)
        {
            return keys[(int)key].IsPressed;
        }

        public bool JustPressed(Keys key)
        {
            return keys[(int)key].IsPressed && keys[(int)key].JustPressed;
        }

        public bool JustReleased(Keys key)
        {
            return (!keys[(int)key].IsPressed) && keys[(int)key].JustReleased;
        }

        private struct KeyState
        {
            public bool JustPressed;
            public bool JustReleased;
            public bool IsPressed;

            internal static KeyState NullValue = new KeyState()
            {
                JustPressed = false,
                JustReleased = false,
                IsPressed = false
            };
        }
    }
}
