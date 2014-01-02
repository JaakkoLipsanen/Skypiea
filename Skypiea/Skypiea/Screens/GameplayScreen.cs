using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.View;
using System;

namespace Skypiea.Screens
{
    public interface IGameContainer
    {
        bool IsActive { get; }
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
            this.FadeType = Flai.ScreenManagement.FadeType.FadeToBlack;

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

        protected override void UnloadContent()
        {
            _level.World.EntityWorld.BroadcastMessage(new GameExitMessage());
        }

        protected override void Deactivate()
        {
            _level.World.EntityWorld.BroadcastMessage(new GameExitMessage());
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive && updateContext.InputState.IsBackButtonPressed)
            {
                this.Pause();
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

        public void Pause()
        {
            _isPaused = true;
            this.ScreenManager.AddScreen(new PauseScreen());
        }

        public void Restart()
        {
            _level.GameOver -= this.OnGameOver;
            _levelRenderer.Unload();
            this.LoadLevel();
        }

        private void LoadLevel()
        {
            _level = Level.Generate(_worldType);
            _levelRenderer = new LevelRenderer(_level);
            _level.EntityWorld.Services.Add<IGameContainer>(this);
            _level.UiContainer.Add(new TextButton("II", new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 16), this.Pause));

            _levelRenderer.LoadContent();
            _level.GameOver += this.OnGameOver;
        }

        private void OnGameOver()
        {
            int score = _level.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>().Score;
            this.ScreenManager.AddScreen(new GameOverScreen(this, score));
        }
    }
}
