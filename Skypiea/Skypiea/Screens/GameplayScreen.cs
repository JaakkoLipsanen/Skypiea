using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Skypiea.Messages;
using Skypiea.Model;
using Skypiea.View;

namespace Skypiea.Screens
{
    public interface IGameContainer
    {
        void Restart();
    }

    public class GameplayScreen : GameScreen, IGameContainer
    {
        private Level _level;
        private LevelRenderer _levelRenderer;
        private bool _isPaused = false;

        public GameplayScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = true;
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (instancePreserved)
            {
                return;
            }

            this.LoadLevel();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (updateContext.InputState.IsBackButtonPressed)
            {
                _isPaused = !_isPaused;
            }

            if (!_isPaused)
            {
                _level.Update(updateContext);
                _levelRenderer.Update(updateContext);
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            _levelRenderer.Draw(graphicsContext);
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            if (message.CPlayerInfo.LivesRemaining == 0)
            {
                base.ScreenManager.AddScreen(new GameOverScreen(this));
            }
        }

        public void Restart()
        {
            _levelRenderer.Unload();
            this.LoadLevel();
        }

        private void LoadLevel()
        {
            _level = Level.Generate();
            _levelRenderer = new LevelRenderer(_level);
            _level.Initialize();
            _levelRenderer.LoadContent();
            _level.World.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);
        }
    }
}
