using Flai.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public abstract class FlaiRenderer
    {
        protected readonly FlaiServiceContainer _serviceContainer;
        protected readonly IContentProvider _contentProvider;
        protected readonly IFontContainer _fontProvider;

        protected GraphicsDevice _graphicsDevice;
        protected bool IsLoaded { get; private set; }

        protected FlaiRenderer()
        {
            _serviceContainer = FlaiGame.ServiceContainer;
            _contentProvider = _serviceContainer.Get<IContentProvider>();
            _fontProvider = _serviceContainer.Get<IFontContainer>();
        }

        public void LoadContent()
        {
            _graphicsDevice = _serviceContainer.Get<IGraphicsDeviceService>().GraphicsDevice;
            this.LoadContentInner();
            this.IsLoaded = true;
        }

        public void Unload()
        {
            this.UnloadInner();
        }

        public void Update(UpdateContext updateContext)
        {
            this.UpdateInner(updateContext);
        }

        public void Draw(GraphicsContext graphicsContext)
        {
            this.DrawInner(graphicsContext);
        }

        protected virtual void LoadContentInner() { }
        protected virtual void UnloadInner() { }
        protected virtual void UpdateInner(UpdateContext updateContext) { }
        protected virtual void DrawInner(GraphicsContext graphicsContext) { }
    }
}
