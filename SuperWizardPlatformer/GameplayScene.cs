﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Maps.Tiled;
using FarseerPhysics.Dynamics;
using System.Linq;

namespace SuperWizardPlatformer
{
    class GameplayScene : IScene, IEntityContainer
    {
        private const int CAPACITY_DEFAULT = 32;
        private const float GRAVITY_Y_DEFAULT = 9.8f;

        private Color bgColor = Color.Black;

        private ContentManager content;
        private TiledMap map;
        private SpriteBatch spriteBatch;
        private GameObjectFactory factory;

        /// <summary>
        /// Constructs a new scene.
        /// </summary>
        /// <param name="game">Game object that will be managing this scene.</param>
        /// <param name="mapName">URI indicating the name of the Tiled .TMX resource to load.</param>
        public GameplayScene(Game game, string mapName)
        {
            if (game == null) { throw new ArgumentNullException(nameof(game)); }
            if (string.IsNullOrWhiteSpace(mapName))
            {
                throw new ArgumentNullException(string.Format("{0}: '{1}'", nameof(mapName), mapName));
            }

            content = new ContentManager(game.Services, game.Content.RootDirectory);
            map = content.Load<TiledMap>(mapName);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            factory = new GameObjectFactory(this);

            if (map.BackgroundColor != null)
            {
                bgColor = (Color)map.BackgroundColor;
            }

            MapBoundaryFactory.CreateAllBoundaries(PhysicsWorld, map);
            factory.PopulateScene(map);
        }

        public List<IEntity> Entities { get; private set; } = new List<IEntity>(CAPACITY_DEFAULT);

        public List<IDrawable> Drawables { get; private set; } = new List<IDrawable>(CAPACITY_DEFAULT);

        public World PhysicsWorld { get; private set; } = new World(new Vector2(0, GRAVITY_Y_DEFAULT));

        /// <summary>
        /// Indicates whether the scene is ready to exit and return control to the parent Game object.
        /// </summary>
        public bool IsReadyToQuit { get; private set; } = false;

        /// <summary>
        /// Indicates whether this IDisposable has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;

        /// <summary>
        /// Halts the physics simulation and disposes of all unmanaged scene resources (textures, audio, etc).
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                PhysicsWorld.ClearForces();
                PhysicsWorld.Clear();
                content.Dispose();
                spriteBatch.Dispose();

                IsDisposed = true;
            }
        }

        /// <summary>
        /// Moves the scene forward by one frame.
        /// </summary>
        /// <param name="gameTime">Information about how much real-world time has passed since the last update.</param>
        public void Update(GameTime gameTime)
        {
            // Delete entities that have been marked for removal from the scene.
            for (int i = 0; i < Entities.Count; ++i)
            {
                if (Entities[i].IsMarkedForRemoval)
                {
                    var tmp = Entities[i];
                    Entities[i] = Entities[Entities.Count - 1];
                    Entities[Entities.Count - 1] = tmp;

                    // Remove body entity from physics sim before removing from entities list.
                    PhysicsWorld.RemoveBody(Entities[Entities.Count].Body);
                    Entities.RemoveAt(Entities.Count - 1);
                    --i;
                }
            }

            // Update all remaining entities.
            foreach (var entity in Entities)
            {
                entity.Update(this, gameTime);
            }

            // One step of the physics simulation forward
            PhysicsWorld.Step(1.0f / 60.0f);
        }

        /// <summary>
        /// Draws the scene, taking into account the background, camera, and all active entities
        /// and drawables.
        /// </summary>
        /// <param name="graphicsDevice">The Monogame GraphicsDevice to use for drawing.</param>
        /// <param name="gameTime">Information about how much real-world time has passed since the last update.</param>
        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            // Draw background info from TMX map.
            map.Draw(spriteBatch, new Rectangle(0, 0, 640, 480));

            // Delete drawables that have been marked for removal from the scene.
            for (int i = 0; i < Drawables.Count; ++i)
            {
                if (Drawables[i].IsMarkedForRemoval)
                {
                    var tmp = Drawables[i];
                    Drawables[i] = Drawables[Drawables.Count - 1];
                    Drawables[Drawables.Count - 1] = tmp;
                    Drawables.RemoveAt(Drawables.Count - 1);
                    --i;
                }
            }

            // Draw all remaining drawables.
            foreach (var drawable in Drawables)
            {
                drawable.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
