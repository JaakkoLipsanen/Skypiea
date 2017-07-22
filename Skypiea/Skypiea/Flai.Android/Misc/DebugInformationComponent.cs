
using System;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Misc
{
    public enum DebugInformationLevel
    {
        None,
        FPS,
        DetailedFPS,
        DetailedFPSAndMemory,
        All,
    }

    public sealed class DebugInformationComponent : FlaiDrawableGameComponent
    {
        private static readonly TimeSpan UpdateFrequency = TimeSpan.FromSeconds(0.5f);

        private string _font = "Default";

        private int _frameRate = 0;
        private int _frameCounter = 0;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        private long _previousMemory = 0L;
        private int _garbageCollections = 0;

        private long _previousKilobytes = 0L;
        private int _newKilobytesPerSecond = 0;

        public DebugInformationLevel DebugInformationLevel { get; set; }
        public int FPS { get; private set; }
        public Vector2 DisplayPosition { get; set; }

        public string Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public DebugInformationComponent(FlaiServiceContainer services)
            : base(services)
        {
            this.FPS = 0;
            this.DisplayPosition = new Vector2(9, 4);
        }

        public sealed override void Update(UpdateContext updateContext)
        {
            long currentMemory = GC.GetTotalMemory(false);
            _elapsedTime += TimeSpan.FromTicks(updateContext.GameTime.DeltaTicks);
            if (_elapsedTime > DebugInformationComponent.UpdateFrequency)
            {
                _elapsedTime -= DebugInformationComponent.UpdateFrequency;
                _frameRate = _frameCounter;
                _frameCounter = 0;

                this.FPS = (int)(_frameRate / UpdateFrequency.TotalSeconds);

                // Memory
                int newKilobytesPerSecond = (int)((currentMemory - _previousKilobytes) / DebugInformationComponent.UpdateFrequency.TotalSeconds / 1024);
                if (newKilobytesPerSecond >= 0)
                {
                    _newKilobytesPerSecond = newKilobytesPerSecond;
                }
                _previousKilobytes = currentMemory;
            }

            // GC
            if (currentMemory < _previousMemory)
            {
                _garbageCollections++;
            }
            _previousMemory = currentMemory;
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            _frameCounter++;

            SpriteFont font = graphicsContext.FontContainer[_font];
            graphicsContext.SpriteBatch.Begin();

            float characterHeight = font.GetCharacterHeight() * 0.75f;
            if (this.DebugInformationLevel >= DebugInformationLevel.FPS)
            {
                graphicsContext.SpriteBatch.DrawStringFaded(font, this.FPS, this.DisplayPosition, Color.Black, Color.Yellow); // (font, this.FPS, new Vector2(12, 4), Color.Black);

                if (this.DebugInformationLevel >= DebugInformationLevel.DetailedFPS)
                {
                    // Frame time
                    graphicsContext.SpriteBatch.DrawStringFaded(font, graphicsContext.GameTime.ElapsedGameTime.TotalMilliseconds, "ms", this.DisplayPosition + Vector2.UnitY*characterHeight*1f);
                    if (this.DebugInformationLevel >= DebugInformationLevel.DetailedFPSAndMemory)
                    {
                        // New bytes allocated per second
                        graphicsContext.SpriteBatch.DrawStringFaded(font, "Allocations: ", _newKilobytesPerSecond, "kb", this.DisplayPosition + Vector2.UnitY*characterHeight*2.5f);

                        if (this.DebugInformationLevel >= DebugInformationLevel.All)
                        {
                            // Used memory
                            graphicsContext.SpriteBatch.DrawStringFaded(font, _previousMemory/1024L, "kb", this.DisplayPosition + Vector2.UnitY*characterHeight*3.5f);

                            // GC
                            graphicsContext.SpriteBatch.DrawStringFaded(font, "GC Collections: ", _garbageCollections, this.DisplayPosition + Vector2.UnitY*characterHeight*4.5f);
                        }

                    }
                }
            }

            graphicsContext.SpriteBatch.End();
        }
    }
}