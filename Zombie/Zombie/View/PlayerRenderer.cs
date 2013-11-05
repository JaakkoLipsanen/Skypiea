using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.View
{
    public class PlayerRenderer : FlaiRenderer
    {
        private readonly Entity _player;
        public PlayerRenderer(EntityWorld entityWorld)
        {
            _player = entityWorld.FindEntityByName(EntityTags.Player);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            TransformComponent transform = _player.Get<TransformComponent>();
            // graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Zombie"), transform.Position, Color.Red, transform.Rotation,  Tile.Size / 2f);
            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Zombie"), transform.Position, Color.White, transform.Rotation, 2);
        }
    }
}
