using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using System;
using System.Collections.Generic;

namespace SuperWizardPlatformer
{
    class GameplayScene : IScene
    {
        private const int CAPACITY_DEFAULT = 32;
        private const float GRAVITY_DEFAULT = 9.8f;
        private static readonly Color BGCOLOR_DEFAULT = Color.Black;

        private World physicsWorld = new World(new Vector2(0, GRAVITY_DEFAULT));
        private Color bgColor;
        private ContentManager content;
        private TiledMap map;
        private SpriteBatch spriteBatch;
        private EntityFactory factory;
        private EntityContainer container;
        private GameplayCamera camera;

        /// <summary>
        /// Constructs a new scene.
        /// </summary>
        /// <param name="game">Game object that will be managing this scene.</param>
        /// <param name="mapName">URI indicating the name of the Tiled .TMX resource to load.</param>
        public GameplayScene(Game game, string mapName)
        {
            // Log start-of-load message.
            var logLoadBanner = string.Format("====== Loading TMX map '{0}' ======", mapName);
            Console.WriteLine(logLoadBanner);

            // Check arguments for null.
            if (game == null) { throw new ArgumentNullException(nameof(game)); }
            if (string.IsNullOrWhiteSpace(mapName))
            {
                throw new ArgumentNullException(nameof(mapName));
            }

            // Instantiate member objects.
            content = new ContentManager(game.Services, game.Content.RootDirectory);
            map = content.Load<TiledMap>(mapName);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            factory = new EntityFactory(physicsWorld);
            container = new EntityContainer(factory.PopulateScene(map));
            bgColor = map.BackgroundColor ?? BGCOLOR_DEFAULT;

            Console.WriteLine("Background Color: {0}", map.BackgroundColor);
            
            MapBoundaryFactory.CreateAllBoundaries(physicsWorld, map);

            // Allocate and assign camera.
            camera = new GameplayCamera(game.GraphicsDevice, 
                new Rectangle(0, 0, map.WidthInPixels, map.HeightInPixels),
                Game1.InternalResolution);

            camera.Follow(container.Player);

            // Log end-of-load message.
            Console.WriteLine("Load successful.");
            for (int i = 0; i < logLoadBanner.Length; ++i) { Console.Write('='); }
            Console.WriteLine();
        }

        public bool IsReadyToQuit { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// Moves the scene forward by one frame.
        /// </summary>
        /// <param name="gameTime">Information about how much real-world time has passed since the last update.</param>
        public void Update(GameTime gameTime)
        {
            container.RemoveMarkedElements();

            int count = container.ActiveEntities.Count;

            // Using explicit indexing because ActiveEntities is an interface.
            for (int i = 0; i < count; ++i)
            {
                container.ActiveEntities[i].Update(this, gameTime);
            }

            physicsWorld.Step(1.0f / 60.0f);
        } 
        
        /// <summary>
        /// Draws the scene, taking into account the background, camera, and all active drawables.
        /// </summary>
        /// <param name="graphicsDevice">The Monogame GraphicsDevice to use for drawing.</param>
        /// <param name="gameTime">Information about how much real-world time has passed since the last update.</param>
        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            camera.UpdatePosition();

            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, 
                null, null, null, camera.GetViewMatrix());

            map.Draw(spriteBatch, camera);

            int count = container.ActiveDrawables.Count;

            // Using explicit indexing becuase ActiveDrawables is an interface.
            for (int i = 0; i < count; ++i)
            {
                container.ActiveDrawables[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                physicsWorld.ClearForces();
                physicsWorld.Clear();
                content.Dispose();
                spriteBatch.Dispose();

                IsDisposed = true;
            }
        }
    }
}
