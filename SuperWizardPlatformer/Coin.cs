using FarseerPhysics.Dynamics;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.TextureAtlases;

namespace SuperWizardPlatformer
{
    class Coin : DrawableEntity
    {
        public Coin(World world, TiledObject obj, TextureRegion2D textureRegion) 
            : base(world, obj, textureRegion)
        {
        }

        public override bool OnCollision(IEntity other)
        {
            var player = other as Player;
            if (player != null)
            {
                ++player.CoinsCollected;
                IsMarkedForRemoval = true;
            }

            return false;
        }
    }
}
