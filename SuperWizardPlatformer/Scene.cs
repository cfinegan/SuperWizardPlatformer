using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Maps.Tiled;
using System;

namespace SuperWizardPlatformer
{
    class Scene : IDisposable, IScene
    {
        private ContentManager content;
        private TiledMap tmxData;
        private SpriteBatch spriteBatch;
        private Color bgColor = Color.Black;

        public Scene(Game game, string mapName)
        {
            content = new ContentManager(game.Services, game.Content.RootDirectory);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            tmxData = content.Load<TiledMap>(mapName);

            Console.WriteLine("Loading map: {0}", mapName);
            Console.WriteLine("Map width: {0}px", tmxData.WidthInPixels);
            Console.WriteLine("Map height: {0}px", tmxData.HeightInPixels);

            if (tmxData.BackgroundColor != null)
            {
                bgColor = (Color)tmxData.BackgroundColor;
            }

            foreach (var objGroup in tmxData.ObjectGroups)
            {
                foreach (var obj in objGroup.Objects)
                {
                    Console.WriteLine(obj.Name);
                }
            }
        }

        public bool IsReadyToQuit { get; private set; } = false;
        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            if (!IsDisposed)
            {
                content.Dispose();

                IsDisposed = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Do nothing for now.
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();
            tmxData.Draw(spriteBatch, new Rectangle(0, 0, tmxData.WidthInPixels, tmxData.HeightInPixels));
            spriteBatch.End();
        }
    }
}
