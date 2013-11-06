using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            CameraComponent.Active = _player.Get<CameraComponent>();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            TransformComponent transform = _player.Get<TransformComponent>();
            PlayerInfoComponent playerInfo = _player.Get<PlayerInfoComponent>();
            Color color = Color.White;
            if (!playerInfo.IsAlive)
            {
                color = Color.DarkGray * 0.75f;
            }

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Zombie"), transform.Position, color, transform.Rotation, 1.5f);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            PlayerInfoComponent playerInfo = _player.Get<PlayerInfoComponent>();
            this.DrawLives(graphicsContext, playerInfo);
            this.DrawScore(graphicsContext, playerInfo);
        }

        private void DrawLives(GraphicsContext graphicsContext, PlayerInfoComponent playerInfo)
        {
            const int Size = 24;
            const int OffsetFromBorder = 8;
            const int Offset = 4;

            Texture2D heartTexture = graphicsContext.ContentProvider.DefaultManager.LoadTexture("Heart");
            for (int i = 0; i < playerInfo.TotalLives; i++)
            {
                Color color = Color.White;
                if (playerInfo.LivesRemaining < i + 1)
                {
                    color = new Color(72, 72, 72) * 0.75f;
                }

                graphicsContext.SpriteBatch.DrawCentered(heartTexture, new Vector2(OffsetFromBorder + (i + 0.5f) * Size, OffsetFromBorder + 0.5f * Size), color, 0, 2);
            }
        }

        private void DrawScore(GraphicsContext graphicsContext, PlayerInfoComponent playerInfo)
        {
            graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer.DefaultFont, playerInfo.Score, new Vector2(8, 32), Color.White);
        }
    }
}
