using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics.PostProcessing
{
    public class PostProcessRenderer : FlaiRenderer
    {
        private readonly List<PostProcess> _postProcesses;
        private readonly int? _renderTargetAntiAliasingCount;

        private RenderTarget2D _deviceRenderTarget = null;
        private RenderTarget2D _sourceRenderTarget = null;

        private bool _isLoaded = false;
        private Size? _overiddenRenderTargetSize = null;


        public PostProcessRenderer()
            : this(null, null)
        {
        }

        public PostProcessRenderer(int antiAliasingCount)
            : this(antiAliasingCount, null)
        {
        }

        public PostProcessRenderer(int antiAliasingCount, IEnumerable<PostProcess> postProcesses)
            : this((int?)antiAliasingCount, postProcesses)
        {
        }

        private PostProcessRenderer(int? antiAliasingCount, IEnumerable<PostProcess> postProcesses)
        {
            _renderTargetAntiAliasingCount = antiAliasingCount;
            _postProcesses = new List<PostProcess>(postProcesses ?? Enumerable.Empty<PostProcess>());
        }

        public PostProcessRenderer(int antiAliasingCount, IEnumerable<PostProcess> postProcesses, Size renderTargetSize)
            : this(antiAliasingCount, postProcesses)
        {
            _overiddenRenderTargetSize = renderTargetSize;
        }

        protected override void LoadContentInner()
        {
            IGraphicsDeviceService graphicsDeviceService = _serviceContainer.Get<IGraphicsDeviceService>();
            this.CreateRenderTargets(graphicsDeviceService.GraphicsDevice);
            graphicsDeviceService.DeviceReset += (o, e) => this.CreateRenderTargets(graphicsDeviceService.GraphicsDevice);

            foreach (PostProcess pp in _postProcesses)
            {
                pp.LoadContent(_serviceContainer);
            }

            _isLoaded = true;
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            foreach (PostProcess pp in _postProcesses.Where(pp => pp.Enabled))
            {
                pp.Update(updateContext);
            }
        }

        public void BeginDraw(GraphicsContext graphicsContext)
        {
            graphicsContext.GraphicsDevice.SetRenderTarget(_sourceRenderTarget);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_postProcesses.Count != 0)
            {
                foreach (PostProcess postProcess in _postProcesses.Where(pp => pp.Enabled).OrderBy(pp => pp.Priority))
                {
                    graphicsContext.GraphicsDevice.SetRenderTarget(_deviceRenderTarget);
                    graphicsContext.GraphicsDevice.Clear(Color.Black);
                    postProcess.Apply(graphicsContext, _sourceRenderTarget, _deviceRenderTarget);
                    graphicsContext.GraphicsDevice.SetRenderTarget(null);

                    // Swap render targets
                    Common.SwapReferences(ref _deviceRenderTarget, ref _sourceRenderTarget);
                }
            }
            else
            {
                graphicsContext.GraphicsDevice.SetRenderTarget(null);
            }


            graphicsContext.GraphicsDevice.SetRenderTarget(null);
            graphicsContext.SpriteBatch.Begin();
            graphicsContext.SpriteBatch.Draw(_sourceRenderTarget, Vector2.Zero);
            graphicsContext.SpriteBatch.End();
        }

        public void AddPostProcess(PostProcess postProcess)
        {
            _postProcesses.Add(postProcess);
            // Can't sort here because List<T>.Sort() is not a stable sort and can't be bothered to implement one

            if (_isLoaded)
            {
                postProcess.LoadContent(_serviceContainer);
            }
        }

        public T GetPostProcess<T>()
            where T : PostProcess
        {
            return (T)_postProcesses.FirstOrDefault(postProcess => postProcess.GetType() == typeof(T));
        }

        public bool TryGetPostProcess<T>(out T postProcess)
            where T : PostProcess
        {
            postProcess = this.GetPostProcess<T>();
            return postProcess != null;
        }

        private void CreateRenderTargets(GraphicsDevice graphicsDevice)
        {         
            PresentationParameters pp = graphicsDevice.PresentationParameters;
            Size size = _overiddenRenderTargetSize.HasValue ? _overiddenRenderTargetSize.Value : new Size(pp.BackBufferWidth, pp.BackBufferHeight);
            int antiAliasingCount = _renderTargetAntiAliasingCount.HasValue ? _renderTargetAntiAliasingCount.Value : pp.MultiSampleCount;

            if (_deviceRenderTarget != null)
            {
                _deviceRenderTarget.Dispose();
                _deviceRenderTarget = null;
            }
            _deviceRenderTarget = new RenderTarget2D(graphicsDevice, size.Width, size.Height, false, pp.BackBufferFormat, pp.DepthStencilFormat, antiAliasingCount, pp.RenderTargetUsage);

            if (_sourceRenderTarget != null)
            {
                _sourceRenderTarget.Dispose();
                _sourceRenderTarget = null;
            }
            _sourceRenderTarget = new RenderTarget2D(graphicsDevice, size.Width, size.Height, false, pp.BackBufferFormat, pp.DepthStencilFormat, antiAliasingCount, pp.RenderTargetUsage);
        }
    }
}
