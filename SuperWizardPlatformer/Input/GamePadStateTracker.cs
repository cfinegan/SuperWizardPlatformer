using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer.Input
{
    /// <summary>
    /// Tracks the current state of all gamepads connected to the system. Reduces the XNA API
    /// to a flat set of binary values ("buttons") that are enumerated in GamePadButtons. Each
    /// virtual button can be queried with the IsPressed, JustPressed, and JustReleased methods.
    /// </summary>
    /// <seealso cref="GamePadButtons"/>
    static class GamePadStateTracker
    {
        private const float TRIGGER_DEADZONE = 0.0f;
        private const float STICK_DEADZONE = 0.0f;

        // Get the total number of virtual buttons for use as array bound.
        private static readonly int NUM_BUTTONS = 
            Enum.GetValues(typeof(GamePadButtons)).Cast<int>().Max() + 1;

        // Each item in array should default to 'false' in both fields.
        private static ButtonState[] buttons = new ButtonState[NUM_BUTTONS];

        /// <summary>
        /// Updates the tracker so that it reports correct values for the current frame. This
        /// method should be called once and only once per frame, at the beginning of the update 
        /// cycle.
        /// </summary>
        public static void Update()
        {
            // Set 'PressedLastFrame' field for each key, and clear values for current frame.
            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].PressedLastFrame = buttons[i].PressedThisFrame;
                buttons[i].PressedThisFrame = false;
            }

            // Query all connected gamepads for current frame info.
            for (int i = 0; i <= GamePad.MaximumGamePadCount; ++i)
            {
                var state = GamePad.GetState(i);

                if (state.IsConnected)
                {
                    ApplyButtonStates(state);
                }
            }
        }

        /// <summary>
        /// Gathers button state info for the current frame.
        /// </summary>
        /// <param name="state">The game pad to query.</param>
        private static void ApplyButtonStates(GamePadState state)
        {
            var pressed = Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            if (state.Buttons.A == pressed)
            {
                buttons[(int)GamePadButtons.A].PressedThisFrame = true;
            }

            if (state.Buttons.B == pressed)
            {
                buttons[(int)GamePadButtons.B].PressedThisFrame = true;
            }

            if (state.Buttons.X == pressed)
            {
                buttons[(int)GamePadButtons.X].PressedThisFrame = true;
            }

            if (state.Buttons.Y == pressed)
            {
                buttons[(int)GamePadButtons.Y].PressedThisFrame = true;
            }

            if (state.Buttons.Start == pressed)
            {
                buttons[(int)GamePadButtons.Start].PressedThisFrame = true;
            }

            if (state.Buttons.Back == pressed)
            {
                buttons[(int)GamePadButtons.Select].PressedThisFrame = true;
            }

            if (state.Buttons.LeftShoulder == pressed || state.Triggers.Left > TRIGGER_DEADZONE)
            {
                buttons[(int)GamePadButtons.L].PressedThisFrame = true;
            }

            if (state.Buttons.RightShoulder == pressed || state.Triggers.Right > TRIGGER_DEADZONE)
            {
                buttons[(int)GamePadButtons.R].PressedThisFrame = true;
            }

            if (state.DPad.Up == pressed || state.ThumbSticks.Left.Y > STICK_DEADZONE)
            {
                buttons[(int)GamePadButtons.Up].PressedThisFrame = true;
            }

            if (state.DPad.Down == pressed || state.ThumbSticks.Left.Y < 0 - STICK_DEADZONE)
            {
                buttons[(int)GamePadButtons.Down].PressedThisFrame = true;
            }

            if (state.DPad.Left == pressed || state.ThumbSticks.Left.X < 0 - STICK_DEADZONE)
            {
                buttons[(int)GamePadButtons.Left].PressedThisFrame = true;
            }

            if (state.DPad.Right == pressed || state.ThumbSticks.Left.X > STICK_DEADZONE)
            {
                buttons[(int)GamePadButtons.Right].PressedThisFrame = true;
            }
        }
        
        /// <summary>
        /// Determines whether the specified virtual button is currently being held down. This
        /// will return true even if the key has just been pressed.
        /// </summary>
        /// <param name="button">The button being queried for.</param>
        /// <returns>true if 'button' is currently being held down, false otherwise.</returns>
        public static bool IsPressed(GamePadButtons button)
        {
            return buttons[(int)button].PressedThisFrame;
        }

        /// <summary>
        /// Determines whether the specified virtual button was just pressed this frame. Useful
        /// for game events that need to execute when the button is first pressed, but shouldn't
        /// re-execute every frame that the button is held down.
        /// </summary>
        /// <param name="button">The button being queried for.</param>
        /// <returns>true if 'button' was just pressed this frame, false otherwise.</returns>
        public static bool JustPressed(GamePadButtons button)
        {
            return buttons[(int)button].PressedThisFrame && 
                (!buttons[(int)button].PressedLastFrame);
        }

        /// <summary>
        /// Determines whether the specified virtual button was just released this frame. Useful
        /// for game events that need to execute when the key is first released, but shouldn't 
        /// re-execute every frame that the key is  not pressed.
        /// </summary>
        /// <param name="button">The button being queried for.</param>
        /// <returns>true if 'button' was just released this frame, false otherwise.</returns>
        public static bool JustReleased(GamePadButtons button)
        {
            return (!buttons[(int)button].PressedThisFrame) &&
                buttons[(int)button].PressedLastFrame;
        }

        /// <summary>
        /// Simple value type that encapsulates the current and previous state of a virtual button.
        /// </summary>
        private struct ButtonState
        {
            public bool PressedThisFrame;
            public bool PressedLastFrame;
        }
    }
}
