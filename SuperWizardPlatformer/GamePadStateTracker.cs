using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace SuperWizardPlatformer
{
    enum GamePadButtons
    {
        A,
        B,
        X,
        Y,
        Start,
        Back,
        L,
        R,
        Up,
        Down,
        Left,
        Right
    }

    static class GamePadStateTracker
    {
        private const float TRIGGER_DEADZONE = 0.0f;
        private const float STICK_DEADZONE = 0.0f;

        private static readonly int NUM_BUTTONS = 
            Enum.GetValues(typeof(GamePadButtons)).Cast<int>().Max() + 1;

        private static ButtonState[] buttons = new ButtonState[NUM_BUTTONS];

        public static void Update()
        {
            for (int i = 0; i < buttons.Length; ++i)
            {
                buttons[i].PressedLastFrame = buttons[i].PressedThisFrame;
                buttons[i].PressedThisFrame = false;
            }

            for (int i = 0; i <= GamePad.MaximumGamePadCount; ++i)
            {
                var state = GamePad.GetState(i);

                if (state.IsConnected)
                {
                    ApplyButtonStates(state);
                }
            }
        }

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
                buttons[(int)GamePadButtons.Back].PressedThisFrame = true;
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

        public static bool IsPressed(GamePadButtons button)
        {
            return buttons[(int)button].PressedThisFrame;
        }

        public static bool JustPressed(GamePadButtons button)
        {
            return buttons[(int)button].PressedThisFrame && 
                (!buttons[(int)button].PressedLastFrame);
        }

        public static bool JustReleased(GamePadButtons button)
        {
            return (!buttons[(int)button].PressedThisFrame) &&
                buttons[(int)button].PressedLastFrame;
        }

        private struct ButtonState
        {
            public bool PressedThisFrame;
            public bool PressedLastFrame;
        }
    }
}
