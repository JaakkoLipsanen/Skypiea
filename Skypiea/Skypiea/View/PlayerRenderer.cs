using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Leaderboards;
using Skypiea.Misc;
using Skypiea.Model.Weapons;
using Skypiea.Systems.Zombie;

namespace Skypiea.View
{
    public class PlayerRenderer : FlaiRenderer
    {
        private readonly Entity _player;
        private readonly IHighscoreManager _highscoreManager;

        public PlayerRenderer(EntityWorld entityWorld)
        {
            _player = entityWorld.FindEntityByName(EntityNames.Player);
            _highscoreManager = entityWorld.Services.Get<IHighscoreManager>();

            CCamera2D.Active = _player.Get<CCamera2D>();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            Color color = Color.White;
            if (!playerInfo.IsAlive)
            {
                color = Color.DarkGray * 0.75f;
            }

            const float GlobalScale = 0.8f;
            if (playerInfo.IsAlive && playerInfo.IsVisuallyInvulnerable)
            {
                const float Scale = 50 * GlobalScale;
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, _player.Transform.Position, new Color(32, 32, 255) * 0.5f, graphicsContext.TotalSeconds * 5f, (1f + FlaiMath.Sin(graphicsContext.TotalSeconds * 6) / 6f) * Scale);
            }

            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Zombie"), _player.Transform.Position, color, _player.Transform.Rotation, GlobalScale);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            this.DrawLives(graphicsContext, playerInfo);
            this.DrawScore(graphicsContext, playerInfo);
            this.DrawWeaponInfo(graphicsContext);

            // * TEMPORARY * //
            IZombieStatsProvider zombieStatsProvider = _player.EntityWorld.Services.Get<IZombieStatsProvider>();

            GraphicalGuidelines.DecimalPrecisionInText = 3;
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Time: ", zombieStatsProvider.TotalTime, new Vector2(graphicsContext.ScreenSize.Width/2f, 416), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Speed: ", zombieStatsProvider.SpeedMultiplier, new Vector2(graphicsContext.ScreenSize.Width / 2f, 440), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Spawn Rate: ", zombieStatsProvider.SpawnRate, new Vector2(graphicsContext.ScreenSize.Width / 2f, 464), Color.White);
        }

        private void DrawLives(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            const int Size = 24;
            const int OffsetFromBorder = 8;

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

        private void DrawScore(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer.DefaultFont, playerInfo.Score, new Vector2(8, 32), Color.White);
            graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer.DefaultFont, "Best: " + _highscoreManager.Highscore, new Vector2(8, 64), Color.White);
        }

        private void DrawWeaponInfo(GraphicsContext graphicsContext)
        {
            const string FontName = "SegoeWP.16";
            CWeapon weaponComponent = _player.Get<CWeapon>();

            SpriteFont font = graphicsContext.FontContainer[FontName];
            graphicsContext.SpriteBatch.DrawString(font, weaponComponent.Weapon.Type.GetDisplayName(), new Vector2(graphicsContext.ScreenSize.Width - 8, 8), Corner.TopRight, Color.White);

            Vector2 position = new Vector2(graphicsContext.ScreenSize.Width - 8, 32);
            int? bulletsRemaining = weaponComponent.Weapon.AmmoRemaining;
            if (bulletsRemaining.HasValue)
            {
                graphicsContext.SpriteBatch.DrawString(font, bulletsRemaining.Value, font.AdjustCorner(bulletsRemaining.Value, Corner.TopRight, position), Color.White);
            }
            else
            {
                const string InfinityDisplayString = "Infinity";
                graphicsContext.SpriteBatch.DrawString(font, InfinityDisplayString, font.AdjustCorner(InfinityDisplayString, Corner.TopRight, position), Color.White);
            }
        }
    }
}
