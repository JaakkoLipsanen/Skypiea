using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework.Input.Touch;
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
        private readonly WorldType _worldType;

        private Level _level;
        private LevelRenderer _levelRenderer;
        private bool _isPaused = false;
        private bool _isDrawing = true;

        public GameplayScreen(WorldType worldType)
        {
            _worldType = worldType;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = true;

            this.EnabledGestures = GestureType.DoubleTap;
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
            if (this.IsActive && updateContext.InputState.IsBackButtonPressed)
            {
                _isPaused = true;
                this.ScreenManager.AddScreen(new PauseScreen());
            }
            else if (_isPaused && this.IsActive)
            {
                _isPaused = false;
            }

            if (updateContext.InputState.HasGesture(GestureType.DoubleTap))
            {
                _isDrawing = !_isDrawing;
            }

            if (!_isPaused)
            {
                _level.Update(updateContext);
                _levelRenderer.Update(updateContext);
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            if (_isDrawing)
            {
                _levelRenderer.Draw(graphicsContext);
            }
        }

       

        public void Restart()
        {
            _levelRenderer.Unload();
            this.LoadLevel();
        }

        private void LoadLevel()
        {
            _level = Level.Generate(_worldType);
            _levelRenderer = new LevelRenderer(_level);

            _level.Initialize();
            _levelRenderer.LoadContent();
            _level.GameOver += this.OnGameOver;
        }

        private void OnGameOver()
        {
            base.ScreenManager.AddScreen(new GameOverScreen(this));
        }
    }
}
