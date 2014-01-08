using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;
using Skypiea.Systems.Zombie;

namespace Skypiea.View
{
    public class PlayerRenderer : FlaiRenderer
    {
        private const float UiOffsetFromBorder = 16;

        private readonly EntityWorld _entityWorld;
        private readonly Entity _player;

        public PlayerRenderer(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _player = entityWorld.FindEntityByName(EntityNames.Player);

            CCamera2D.Active = _player.Get<CCamera2D>();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            Color color = playerInfo.IsAlive ? Color.White : Color.DarkGray * 0.75f;

            const float Scale = 0.8f;

            // draw "invulnerability rectangle"
            if (playerInfo.IsAlive && playerInfo.IsVisuallyInvulnerable)
            {
                const int RectangleSize = 50;
                Vector2 position = Vector2i.Round(_player.Transform.Position);
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, position, new Color(32, 32, 255) * 0.5f, _entityWorld.TotalUpdateTime * 5f, (1f + FlaiMath.Sin(_entityWorld.TotalUpdateTime * 6) / 6f) * RectangleSize * Scale);
            }

            // draw the player texture
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Zombie"), _player.Transform.Position, color, _player.Transform.Rotation, Scale);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            this.DrawLives(graphicsContext, playerInfo);
            this.DrawScore(graphicsContext, playerInfo);
            this.DrawWeaponInfo(graphicsContext);

            // * DEBUG * //           
            if (TestingGlobals.Debug)
            {
                this.DrawZombieStatsInfo(graphicsContext);
            }
        }

        private void DrawLives(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            TextureDefinition heartTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Heart");
            for (int i = 0; i < playerInfo.TotalLives; i++)
            {
                Color color = Color.White;
                if (playerInfo.LivesRemaining <= i)
                {
                    color = new Color(72, 72, 72) * 0.75f;
                }

                const int Scale = 2;
                const int Size = 24;
                const float OffsetFromLeft = UiOffsetFromBorder - 10 + 0.5f * Size;

                graphicsContext.SpriteBatch.DrawCentered(heartTexture, new Vector2(OffsetFromLeft + i * Size, UiOffsetFromBorder + 0.5f * Size), color, 0, Scale);
            }
        }

        private void DrawScore(GraphicsContext graphicsContext, CPlayerInfo playerInfo)
        {
            graphicsContext.SpriteBatch.DrawStringFaded(graphicsContext.FontContainer["Minecraftia.20"], playerInfo.Score, new Vector2(8, 32));
        }

        private void DrawWeaponInfo(GraphicsContext graphicsContext)
        {
            CWeapon weaponComponent = _player.Get<CWeapon>();
            SpriteFont font = graphicsContext.FontContainer["Minecraftia.20"];

            // weapon name
            graphicsContext.SpriteBatch.DrawStringFaded(font, weaponComponent.Weapon.Type.GetDisplayName(), new Vector2(8, 72));

            // bullets remaining
            int? bulletsRemaining = weaponComponent.Weapon.AmmoRemaining;
            if (bulletsRemaining.HasValue) // don't draw ammo if the weapon has unlimited ammo (= assault rifle)
            {
                graphicsContext.SpriteBatch.DrawStringFaded(font, bulletsRemaining.Value, font.AdjustCorner(bulletsRemaining.Value, Corner.TopLeft, new Vector2(8, 104)));
            }
        }

        // * DEBUG * //   
        private void DrawZombieStatsInfo(GraphicsContext graphicsContext)
        {
            IZombieStatsProvider zombieStatsProvider = _player.EntityWorld.Services.Get<IZombieStatsProvider>();

            GraphicalGuidelines.DecimalPrecisionInText = 3;
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Time: ", zombieStatsProvider.TotalTime, new Vector2(graphicsContext.ScreenSize.Width / 2f, 416), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Speed: ", zombieStatsProvider.SpeedMultiplier, new Vector2(graphicsContext.ScreenSize.Width / 2f, 440), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Spawn Rate: ", zombieStatsProvider.SpawnRate, new Vector2(graphicsContext.ScreenSize.Width / 2f, 464), Color.White);
        }
    }
}
