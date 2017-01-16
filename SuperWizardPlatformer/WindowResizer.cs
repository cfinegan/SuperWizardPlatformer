using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperWizardPlatformer
{
    class WindowResizer
    {
        private GraphicsDeviceManager graphics;
        private Game game;
        private GraphicsDevice device;
        private GameWindow window;
        private Point lastWindowPosition;
        private int lastWindowWidth;
        private int lastWindowHeight;

        public WindowResizer(GraphicsDeviceManager graphics, Game game)
        {
            if (graphics == null) { throw new ArgumentNullException(nameof(graphics)); }
            if (game == null) { throw new ArgumentNullException(nameof(game)); }

            this.graphics = graphics;
            this.game = game;
            device = game.GraphicsDevice;
            window = game.Window;

            lastWindowWidth = Game1.InternalResolution.Width * 2;
            lastWindowHeight = Game1.InternalResolution.Height * 2;

            lastWindowPosition = new Point(
                device.DisplayMode.Width / 2 - lastWindowWidth / 2,
                device.DisplayMode.Height / 2 - lastWindowHeight / 2);

            window.ClientSizeChanged += OnClientSizeChanged;
            window.AllowUserResizing = true;

            EnableDefaultScreenProperties();
            ViewRectangle = CalculateViewRectangle();
        }

        public Rectangle ViewRectangle { get; private set; }

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

                game.IsMouseVisible = false;
            }
        }

        public void EnableBorderedWindow()
        {
            window.IsBorderless = false;
            window.Position = lastWindowPosition;
            graphics.PreferredBackBufferWidth = lastWindowWidth;
            graphics.PreferredBackBufferHeight = lastWindowHeight;
            graphics.ApplyChanges();
            game.IsMouseVisible = true;
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
            ViewRectangle = CalculateViewRectangle();
        }

        /// <summary>
        /// Calculates the correct rectangle for drawing scaled graphical output to the screen
        /// without stretching.
        /// </summary>
        /// <returns>Rectangle representing the region of the screen to draw.</returns>
        private Rectangle CalculateViewRectangle()
        {
            float outputAspect = window.ClientBounds.Width /
                (float)window.ClientBounds.Height;

            float preferredAspect = Game1.InternalResolution.Width /
                (float)Game1.InternalResolution.Height;

            if (outputAspect <= preferredAspect)
            {
                // Output is taller than it is wide, bars on top/bottom.
                int presentHeight = (int)((window.ClientBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (window.ClientBounds.Height - presentHeight) / 2;
                return new Rectangle(0, barHeight, window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // Output is wider than it is tall, bars on left/right.
                int presentWidth = (int)((window.ClientBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (window.ClientBounds.Width - presentWidth) / 2;
                return new Rectangle(barWidth, 0, presentWidth, window.ClientBounds.Height);
            }
        }

        /// <summary>
        /// In debug mode, the default screen config is a normal bordered window. In release mode,
        /// however, the default is fullscreen (this is a snappier presentation for people who
        /// haven't seen the game before).
        /// </summary>
        private void EnableDefaultScreenProperties()
        {
#if DEBUG
            EnableBorderedWindow();
#else
            EnableBorderlessFullscreen();
#endif
        }
    }
}
