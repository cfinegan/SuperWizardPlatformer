using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SuperWizardPlatformer
{
    class WindowModeAdjuster
    {
        private GraphicsDeviceManager graphics;
        private GraphicsDevice device;
        private GameWindow window;
        private Point lastWindowPosition;
        private int lastWindowWidth;
        private int lastWindowHeight;

        public WindowModeAdjuster(GraphicsDeviceManager graphics, GameWindow window)
        {
            device = graphics.GraphicsDevice;
            this.graphics = graphics;
            this.window = window;

            lastWindowWidth = 1024;
            lastWindowHeight = 768;

            lastWindowPosition = new Point(
                device.DisplayMode.Width / 2 - lastWindowWidth / 2,
                device.DisplayMode.Height / 2 - lastWindowHeight / 2);

            window.ClientSizeChanged += OnClientSizeChanged;
        }

        public bool IsBorderlessFullscreen
        {
            get
            {
                return window.IsBorderless;
            }
        }

        public bool IsBorderedWindow
        {
            get
            {
                return !window.IsBorderless;
            }
        }

        public void EnableBorderlessFullscreen()
        {
            if (!window.IsBorderless)
            {
                lastWindowWidth = graphics.PreferredBackBufferWidth;
                lastWindowHeight = graphics.PreferredBackBufferHeight;
                lastWindowPosition = window.Position;

                window.IsBorderless = true;
                window.Position = Point.Zero;

                graphics.PreferredBackBufferWidth = device.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = device.DisplayMode.Height;
                graphics.ApplyChanges();
            }
        }

        public void EnableBorderedWindow()
        {
            window.IsBorderless = false;
            window.Position = lastWindowPosition;
            graphics.PreferredBackBufferWidth = lastWindowWidth;
            graphics.PreferredBackBufferHeight = lastWindowHeight;
            graphics.ApplyChanges();
        }

        public void ToggleBorderlessFullscreen()
        {
            if (!window.IsBorderless)
            {
                EnableBorderlessFullscreen();
            }
            else
            {
                EnableBorderedWindow();
            }
        }

        /// <summary>
        /// Adjusts the graphics device's preferred width/height so that it never performs any
        /// scaling of the output. The if statement prevents a stack overflow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (graphics.PreferredBackBufferWidth != window.ClientBounds.Width ||
                graphics.PreferredBackBufferHeight != window.ClientBounds.Height)
            {
                graphics.PreferredBackBufferWidth = window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }
    }
}
