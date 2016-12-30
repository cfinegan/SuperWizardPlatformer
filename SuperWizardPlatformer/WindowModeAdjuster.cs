using System;
using Microsoft.Xna.Framework;

namespace SuperWizardPlatformer
{
    class WindowModeAdjuster
    {
        private Game game;
        private GraphicsDeviceManager graphics;
        private Point lastWindowPosition;
        private int lastWindowWidth;
        private int lastWindowHeight;

        public WindowModeAdjuster(Game game)
        {
            if (game == null) { throw new ArgumentNullException(nameof(game)); }

            this.game = game;
            graphics = game.Services.GetService<GraphicsDeviceManager>();

            if (graphics == null)
            {
                throw new ArgumentException(
                    "Could not find GraphicsDeviceManager service in Game.");
            }            

            lastWindowWidth = 840;
            lastWindowHeight = 480;

            lastWindowPosition = new Point(
                game.GraphicsDevice.DisplayMode.Width / 2 - lastWindowWidth / 2,
                game.GraphicsDevice.DisplayMode.Height / 2 - lastWindowHeight / 2);

            game.Window.ClientSizeChanged += OnClientSizeChanged;

            game.Window.AllowUserResizing = true;
        }

        public bool IsBorderlessFullscreen
        {
            get
            {
                return game.Window.IsBorderless;
            }
        }

        public bool IsBorderedWindow
        {
            get
            {
                return !game.Window.IsBorderless;
            }
        }

        public void EnableBorderlessFullscreen()
        {
            if (!game.Window.IsBorderless)
            {
                lastWindowWidth = graphics.PreferredBackBufferWidth;
                lastWindowHeight = graphics.PreferredBackBufferHeight;
                lastWindowPosition = game.Window.Position;

                game.Window.IsBorderless = true;
                game.Window.Position = Point.Zero;

                graphics.PreferredBackBufferWidth = game.GraphicsDevice.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = game.GraphicsDevice.DisplayMode.Height;
                graphics.ApplyChanges();

                game.IsMouseVisible = false;
            }
        }

        public void EnableBorderedWindow()
        {
            game.Window.IsBorderless = false;
            game.Window.Position = lastWindowPosition;
            graphics.PreferredBackBufferWidth = lastWindowWidth;
            graphics.PreferredBackBufferHeight = lastWindowHeight;
            graphics.ApplyChanges();
            game.IsMouseVisible = true;
        }

        public void ToggleBorderlessFullscreen()
        {
            if (!game.Window.IsBorderless)
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
            if (graphics.PreferredBackBufferWidth != game.Window.ClientBounds.Width ||
                graphics.PreferredBackBufferHeight != game.Window.ClientBounds.Height)
            {
                graphics.PreferredBackBufferWidth = game.Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = game.Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }
    }
}
