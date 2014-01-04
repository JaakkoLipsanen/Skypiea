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
        private const float UIOffsetFromBorder = 16;

        private readonly Entity _player;
        private readonly IHighscoreManager _highscoreManager;

        public PlayerRenderer(EntityWorld entityWorld)
        {
            _player = entityWorld.FindEntityByName(EntityNames.Player);
            _highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();

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
                Vector2 position = Vector2i.Round(_player.Transform.Position);
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, position, new Color(32, 32, 255) * 0.5f, graphicsContext.TotalSeconds * 5f, (1f + FlaiMath.Sin(graphicsContext.TotalSeconds * 6) / 6f) * Scale);
            }

            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Zombie"), _player.Transform.Position, color, _player.Transform.Rotation, GlobalScale);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            this.DrawLives(graphicsContext, playerInfo);
            this.DrawScore(graphicsContext, playerInfo);
            this.DrawWeaponInfo(graphicsContext);

            // * TEMPORARY * //
            if (TestingGlobals.Debug)
            {
                IZombieStatsProvider zombieStatsProvider = _player.EntityWorld.Services.Get<IZombieStatsProvider>();

                GraphicalGuidelines.DecimalPrecisionInText = 3;
                graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Time: ", zombieStatsProvider.TotalTime, new Vector2(graphicsContext.ScreenSize.Width / 2f, 416), Color.White);
                graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Speed: ", zombieStatsProvider.SpeedMultiplier, new Vector2(graphicsContext.ScreenSize.Width / 2f, 440), Color.White);
                graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Spawn Rate: ", zombieStatsProvider.SpawnRate, new Vector2(graphicsContext.ScreenSize.Width / 2f, 464), Color.White);
            }
        }

        private void DrawLives(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            const int Size = 24;

            TextureDefinition heartTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Heart");
            for (int i = 0; i < playerInfo.TotalLives; i++)
            {
                Color color = Color.White;
                if (playerInfo.LivesRemaining < i + 1)
                {
                    color = new Color(72, 72, 72) * 0.75f;
                }

                graphicsContext.SpriteBatch.DrawCentered(heartTexture, new Vector2(UIOffsetFromBorder - 10 + (i + 0.5f) * Size, UIOffsetFromBorder + 0.5f * Size), color, 0, 2);
            }
        }

        private void DrawScore(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            const string FontName = "Minecraftia.20";
            graphicsContext.SpriteBatch.DrawStringFaded(graphicsContext.FontContainer[FontName], playerInfo.Score, new Vector2(8, 32));
        }

        private void DrawWeaponInfo(GraphicsContext graphicsContext)
        {
            const string FontName = "Minecraftia.20";

            CWeapon weaponComponent = _player.Get<CWeapon>();
            SpriteFont font = graphicsContext.FontContainer[FontName];

            // weapon name
            graphicsContext.SpriteBatch.DrawStringFaded(font, weaponComponent.Weapon.Type.GetDisplayName(), new Vector2(8, 72));

            // bullets remaining
            int? bulletsRemaining = weaponComponent.Weapon.AmmoRemaining;
            if (bulletsRemaining.HasValue)
            {
                graphicsContext.SpriteBatch.DrawStringFaded(font, bulletsRemaining.Value, font.AdjustCorner(bulletsRemaining.Value, Corner.TopLeft, new Vector2(8, 104)));
            }
        }
    }
}
