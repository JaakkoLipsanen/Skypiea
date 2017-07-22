using Flai.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public sealed class GraphicsContext
    { 
        private readonly FlaiGame _game;
        private readonly PrimitiveRenderer _primitiveRenderer;
        private readonly FontContainer _fontContainer;

        private GameTime _gameTime; // TODO: Own "GameClock" maybe?
        private FlaiSpriteBatch _spriteBatch;
        private Texture2D _blankTexture;

        public GameTime GameTime
        {
            get { return _gameTime; }
        }

        public float DeltaSeconds
        {
            get { return (float)_gameTime.ElapsedGameTime.TotalSeconds; }
        }

        public float TotalSeconds
        {
            get { return (float)_gameTime.TotalGameTime.TotalSeconds; }
        }

        public Texture2D BlankTexture
        {
            get { return _blankTexture; }
        }

        public IContentProvider ContentProvider
        {
            get { return _game.Services.Get<IContentProvider>(); }
        }

        public FlaiSpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public PrimitiveRenderer PrimitiveRenderer
        {
            get { return _primitiveRenderer; }
        }

        public FontContainer FontContainer
        {
            get { return _fontContainer; }
        }

        public FlaiServiceContainer Services
        {
            get { return _game.Services; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _game.GraphicsDevice; }
        }

        public Viewport Viewport
        {
            get { return _game.GraphicsDevice.Viewport; }
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

        internal GraphicsContext(FlaiGame game)
        {
            Ensure.NotNull(game);

            _game = game;
            _fontContainer = new FontContainer(_game.Services);
            _game.Services.Add(this); // GraphicsContext, not IGraphicsContext
            _primitiveRenderer = new PrimitiveRenderer(this);
        }

        internal void LoadContent()
        {
            _spriteBatch = new FlaiSpriteBatch(this.GraphicsDevice);

            _blankTexture = new Texture2D(this.GraphicsDevice, 1, 1) { Tag = "BlankTexture" };
            _blankTexture.SetData(new Color[] { Color.White });
        }

        internal void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
        }
    }
}
