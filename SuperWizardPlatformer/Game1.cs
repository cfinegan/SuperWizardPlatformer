using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SuperWizardPlatformer.Input;
using System;
using System.Diagnostics;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Size InternalResolution { get; } = new Size(420, 240);

        private GraphicsDeviceManager graphics;
        private WindowResizer resizer;
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
            resizer = new WindowResizer(graphics, this);

            // Render target is used to separate internal resolution from display resolution.
            renderTarget = new RenderTarget2D(GraphicsDevice, 
                InternalResolution.Width, InternalResolution.Height);

            ActionMapper.Initialize();

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
                resizer.ToggleBorderlessFullscreen();
            }
            if (Debugger.IsAttached && !graphics.IsFullScreen && KeyStateTracker.JustPressed(Keys.F5))
            {
                Debugger.Break();
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
            spriteBatch.Draw(renderTarget, resizer.ViewRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
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
    }
}
