using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer.Input
{
    /// <summary>
    /// Tracks the current and previous state of all keys recognizable by the framework.
    /// </summary>
    static class KeyStateTracker
    {
        // Get the total number of keys recognized by the framework for use as array bound.
        private static readonly int NUM_KEYS = Enum.GetValues(typeof(Keys)).Cast<int>().Max() + 1;

        // Each item in array should default to 'false' in both fields.
        private static KeyState[] keys = new KeyState[NUM_KEYS];

        private static bool isAltEnterPressedThisFrame = false;
        private static bool isAltEnterPressedLastFrame = false;

        /// <summary>
        /// Updates the tracker so that it reports correct values for the current frame. This
        /// method should be called once and only once per frame, at the very beginning of the
        /// update cycle.
        /// </summary>
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

        /// <summary>
        /// Denotes whether the user pressed Alt+Enter since the last update cycle. This is
        /// considered a special value separate from other keystrokes so that the game can
        /// recognize the difference between the user pressing Alt+Enter and just pressing Enter.
        /// </summary>
        public static bool IsAltEnterJustPressed
        {
            get
            {
                return isAltEnterPressedThisFrame && (!isAltEnterPressedLastFrame);
            }
        }

        /// <summary>
        /// Determines whether the specified key is currently being held down. This will return
        /// true even if the key has just been pressed.
        /// </summary>
        /// <param name="key">The key value being queried for.</param>
        /// <returns>true if 'key' is currently being held down, false otherwise.</returns>
        public static bool IsPressed(Keys key)
        {
            return keys[(int)key].PressedThisFrame;
        }

        /// <summary>
        /// Determines whether the specified key was just presesed this frame. Useful for game
        /// events that need to execute when the key is first pressed, but shouldn't re-execute
        /// every frame that the key is held down.
        /// </summary>
        /// <param name="key">The key value being queried for.</param>
        /// <returns>true if 'key' was just pressed this frame, false otherwise.</returns>
        public static bool JustPressed(Keys key)
        {
            return keys[(int)key].PressedThisFrame && (!keys[(int)key].PressedLastFrame);
        }

        /// <summary>
        /// Determines whether the specified key was just released this frame. Useful for game
        /// events that need to execute when the key is first released, but shouldn't re-execute
        /// every frame that the key is not pressed.
        /// </summary>
        /// <param name="key">The key value being queried for.</param>
        /// <returns>true if 'key' was just released this frame, false otherwise.</returns>
        public static bool JustReleased(Keys key)
        {
            return (!keys[(int)key].PressedThisFrame) && keys[(int)key].PressedLastFrame;
        }

        /// <summary>
        /// Simple value type that encapsulates the current and previous state of a key.
        /// </summary>
        private struct KeyState
        {
            public bool PressedThisFrame;
            public bool PressedLastFrame;
        }
    }
}
