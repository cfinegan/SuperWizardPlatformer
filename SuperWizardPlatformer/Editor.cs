using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Maps.Tiled;
using Newtonsoft.Json;
using System.IO;

namespace SuperWizardPlatformer
{
    class Editor : IScene
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;
        TiledMap map;

        public Editor(Game game, string mapName)
        {
            string mapInfoPath = string.Format("{0}.json", mapName);
            Console.WriteLine(mapInfoPath);
            Console.Out.Flush();
            MapInfo jsonData = JsonConvert.DeserializeObject<MapInfo>(File.ReadAllText(mapInfoPath));

            content = new ContentManager(game.Services, game.Content.RootDirectory);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            map = content.Load<TiledMap>(jsonData.tmxdata);
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

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
