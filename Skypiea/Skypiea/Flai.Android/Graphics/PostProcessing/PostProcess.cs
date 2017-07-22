using System;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics.PostProcessing
{
    public abstract class PostProcess : IComparable<PostProcess>
    {
        protected readonly GraphicsDevice _graphicsDevice;

        public int Priority { get; set; }
        public bool Enabled { get; set; }

        protected PostProcess()
        {
            Enabled = true;
            Priority = 0;
            _graphicsDevice = FlaiGame.Current.GraphicsDevice;
        }

        public void ToggleEnabled()
        {
            this.Enabled = !this.Enabled;
        }

        public virtual void LoadContent(FlaiServiceContainer services) { }
        public virtual void Update(UpdateContext updateContext) { }
        public abstract void Apply(GraphicsContext graphicsContext, RenderTarget2D input, RenderTarget2D output);

        public int CompareTo(PostProcess other)
        {
            return this.Priority - other.Priority;
        }
    }
}
