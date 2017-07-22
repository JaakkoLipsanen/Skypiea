using Flai.Input;
using Microsoft.Xna.Framework;

namespace Flai
{
    public interface IXnaGameTimeProvider
    {
        GameTime GameTime { get; }
    }

    public sealed class UpdateContext : IXnaGameTimeProvider
    {
        // basically, you can use this to simulate stuff/get a "dumb" UpdateContext to be used once
        // possibly todo: updatecontext with a specific "DeltaSeconds"/TotalSeconds value
        public static UpdateContext Null
        {
            get { return new UpdateContext(FlaiGame.Current, null); }
        }

        private readonly FlaiGame _game;
        private readonly InputState _inputState = new InputState();
        private readonly FlaiGameTime _gameTime = new FlaiGameTime();

        public IGameTime GameTime
        {
            get { return _gameTime; }
        }

        GameTime IXnaGameTimeProvider.GameTime
        {
            get { return _gameTime.XnaGameTime; }
        }

        public float DeltaSeconds
        {
            get { return this.GameTime.DeltaSeconds; }
        }

        public float TotalSeconds
        {
            get { return this.GameTime.TotalSeconds; }
        }

        public InputState InputState
        {
            get { return _inputState; }
        }

        public FlaiServiceContainer Services
        {
            get { return _game.Services; }
        }

        public Size ViewportSize
        {
            get { return _game.ViewportSize; }
        }

        public Rectangle ViewportArea
        {
            get { return _game.ViewportArea; }
        }

        public Size ScreenSize
        {
            get { return _game.ScreenSize; }
        }

        public Rectangle ScreenArea
        {
            get { return _game.ScreenArea; }
        }

        public FlaiGame Game
        {
            get { return _game; }
        }

        private UpdateContext(FlaiGame game, object nullParameter)
        {
            _game = game;
        }

        internal UpdateContext(FlaiGame game)
        {
            Ensure.NotNull(game);

            _game = game;
            _game.Services.Add<IGameTime>(_gameTime);
        }

        internal void Update(GameTime gameTime)
        {
            _gameTime.Update(gameTime);
            _inputState.Update();
        }
    }
}
