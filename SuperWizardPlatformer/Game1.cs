using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperWizardPlatformer.Input;
using MonoGame.Extended;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        internal static Size InternalResolution { get; } = new Size(420, 240);

        private GraphicsDeviceManager graphics;
        private WindowModeAdjuster resolution;
        private RenderTarget2D renderTarget;
        private SpriteBatch spriteBatch;
        private IScene scene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            resolution = new WindowModeAdjuster(graphics, Window);

            ActionMapper.Initialize();

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            // Load default screen configuration.
            EnableDefaultScreenProperties();

            // Render target is used to separate internal resolution from display resolution.
            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                InternalResolution.Width,
                InternalResolution.Height,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                GraphicsDevice.PresentationParameters.DepthStencilFormat);

            // Output some diagnostics before returning.
            Console.WriteLine("BackbufferWidth: {0}", graphics.PreferredBackBufferWidth);
            Console.WriteLine("BackbufferHeight: {0}", graphics.PreferredBackBufferHeight);
            Console.WriteLine("IsFixedTimeStep: {0}", IsFixedTimeStep);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            LoadScene(new GameplayScene(this, "mario_test"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
            UnloadScene();
            renderTarget.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyStateTracker.Update();
            GamePadStateTracker.Update();

            if (KeyStateTracker.JustPressed(Keys.Escape))
            {
                Exit();
            }
            if (KeyStateTracker.IsAltEnterJustPressed)
            {
                resolution.ToggleBorderlessFullscreen();
            }

            scene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Draw scene to render target.
            GraphicsDevice.SetRenderTarget(renderTarget);
            scene.Draw(GraphicsDevice, gameTime);
            GraphicsDevice.SetRenderTarget(null);

            // Draw render target to screen.
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, CalculateViewRectangle(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Calculates the correct rectangle for drawing scaled graphical output to the screen
        /// without stretching.
        /// </summary>
        /// <returns>Rectangle representing the region of the screen to draw.</returns>
        private Rectangle CalculateViewRectangle()
        {
            float outputAspect = Window.ClientBounds.Width /
                (float)Window.ClientBounds.Height;

            float preferredAspect = renderTarget.Width /
                (float)renderTarget.Height;

            Rectangle dst;

            if (outputAspect <= preferredAspect)
            {
                // Output is taller than it is wide, bars on top/bottom.
                int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                dst = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // Output is wider than it is tall, bars on left/right.
                int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                dst = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            return dst;
        }

        private void LoadScene(IScene scene)
        {
            if (this.scene != null && !this.scene.IsDisposed)
            {
                this.scene.Dispose();
            }

            this.scene = scene;
            GC.Collect();
        }

        private void UnloadScene()
        {
            LoadScene(null);
        }

        /// <summary>
        /// In debug mode, the default screen config is a normal bordered window. In release mode,
        /// however, the default is fullscreen (this is a snappier presentation for people who
        /// haven't seen the game before).
        /// </summary>
        private void EnableDefaultScreenProperties()
        {
#if DEBUG
            resolution.EnableBorderedWindow();
#else
            resolution.EnableBorderlessFullscreen();
#endif
        }
    }
}
