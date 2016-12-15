using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Maps.Tiled;
using FarseerPhysics.Dynamics;

namespace SuperWizardPlatformer
{
    class GameplayScene : IScene
    {
        private static Color defaultBgColor = Color.Black;

        private ContentManager content;
        private TiledMap map;
        private SpriteBatch spriteBatch;
        private World physicsWorld;
        private GameObjectFactory factory;
        private Color bgColor;

        private List<IEntity> entities;
        private List<IDrawable> drawables;

        public GameplayScene(Game game, string mapName)
        {
            float GRAVITY = 9.8f;

            content = new ContentManager(game.Services, game.Content.RootDirectory);
            map = content.Load<TiledMap>(mapName);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            physicsWorld = new World(new Vector2(0, GRAVITY));

            bgColor = map.BackgroundColor != null ? (Color)map.BackgroundColor : defaultBgColor;

            factory = new GameObjectFactory(spriteBatch);
            var results = factory.CreateScene(map, physicsWorld);
            entities = results.Item1;
            drawables = results.Item2;
        }

        public bool IsReadyToQuit { get; private set; } = false;

        public bool IsDisposed { get; private set; } = false;

        public bool IsDebugViewEnabled { get; set; } = true;

        public void Dispose()
        {
            if (!IsDisposed)
            {
                content.Dispose();
                spriteBatch.Dispose();

                IsDisposed = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            physicsWorld.Step(1.0f / 60.0f);
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            map.Draw(spriteBatch, new Rectangle(0, 0, 640, 480));

            foreach (var drawable in drawables)
            {
                drawable.Draw();
            }

            spriteBatch.End();
        }
    }
}
