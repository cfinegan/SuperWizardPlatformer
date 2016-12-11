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

        public bool IsReadyToQuit { get; private set; } = false;

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
        }

        public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            graphicsDevice.Clear(bgColor);

            spriteBatch.Begin();
            tmxData.Draw(spriteBatch, new Rectangle(0, 0, tmxData.WidthInPixels, tmxData.HeightInPixels));
            spriteBatch.End();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    content.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Scene() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
