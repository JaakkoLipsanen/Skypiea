﻿using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.View
{
    public class PlayerRenderer : FlaiRenderer
    {
        private readonly Entity _player;
        public PlayerRenderer(EntityWorld entityWorld)
        {
            _player = entityWorld.FindEntityByName(EntityNames.Player);
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
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, _player.Transform.Position, new Color(48, 48, 255) * 0.4f, graphicsContext.TotalSeconds * 5f, (1f + FlaiMath.Sin(graphicsContext.TotalSeconds * 6) / 6f) * Scale);
            }

            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ZombieShadow"), _player.Transform.Position, Color.White * 0.5f, 0, 1.4f * GlobalScale);
            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Zombie"), _player.Transform.Position, color, _player.Transform.Rotation, GlobalScale);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            CPlayerInfo playerInfo = _player.Get<CPlayerInfo>();
            this.DrawLives(graphicsContext, playerInfo);
            this.DrawScore(graphicsContext, playerInfo);
            this.DrawWeaponInfo(graphicsContext);
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
        }

        private void DrawWeaponInfo(GraphicsContext graphicsContext)
        {
            const string FontName = "SegoeWP.16";
            CWeapon weaponComponent = _player.Get<CWeapon>();

            SpriteFont font = graphicsContext.FontContainer[FontName];
            graphicsContext.SpriteBatch.DrawString(font, weaponComponent.Weapon.Type.GetDisplayName(), new Vector2(graphicsContext.ScreenSize.Width - 8, 8), Color.White, Corner.TopRight);

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