
using System;
using Flai.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Flai
{
    public abstract class FlaiDrawableGameComponent : FlaiGameComponent
    {
        protected bool _visible = true;

        private bool _initialized = false;
        private IGraphicsDeviceService _graphicsDeviceService;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    this.OnVisibleChanged();
                }
            }
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                Ensure.NotNull(_graphicsDeviceService, "You must call Initialize before accessing GraphicsDevice -property");
                return _graphicsDeviceService.GraphicsDevice;
            }
        }

        protected FlaiDrawableGameComponent(FlaiServiceContainer services)
            : base(services)
        {
        }

        public override void Initialize()
        {
            if (!_initialized)
            {
                _graphicsDeviceService = _services.Get<IGraphicsDeviceService>();
                Ensure.NotNull(_graphicsDeviceService, "Graphics device service is missing");

                _graphicsDeviceService.DeviceCreated += new EventHandler<EventArgs>(this.GraphicsDeviceCreated);
                _graphicsDeviceService.DeviceDisposing += new EventHandler<EventArgs>(this.GraphicsDeviceDisposing);
                if (_graphicsDeviceService.GraphicsDevice != null)
                {
                    this.LoadContent();
                }
            }
        }

        private void GraphicsDeviceDisposing(object sender, EventArgs e)
        {
            this.UnloadContent();
        }

        private void GraphicsDeviceCreated(object sender, EventArgs e)
        {
            this.LoadContent();
        }

        protected virtual void LoadContent() { }
        protected virtual void UnloadContent() { }

        public virtual void Draw(GraphicsContext graphicsContext) { }
        protected virtual void OnVisibleChanged() { }

        public void ToggleVisibility()
        {
            this.Visible = !this.Visible;
        }
    }
}
