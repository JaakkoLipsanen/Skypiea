
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class AnimatedSprite : Sprite, ITileSpriteSheet
    {
        protected readonly Dictionary<Vector2i, Color[]> _framePixelDatas = new Dictionary<Vector2i, Color[]>();

        protected readonly Vector2 _frameSize;
        protected Vector2i _framePosition;
        protected Size _sheetSize;

        protected double _frameTime;
        private double _timeUntilNewFrame;

        #region Properties

        /// <summary>
        /// If AnimatedSprite is frozen, it doesn't animate
        /// </summary>
        public bool Frozen { get; set; }

        // Jesus fuck, should there be SheetWidth/Height and FrameWidth/Height properties? Could be more readable when using this

        public Size SheetSize
        {
            get { return _sheetSize; }
        }


        public Vector2 FrameSize
        {
            get { return _frameSize; }
        }

        public override Color[] PixelData
        {
            get
            {
                if (_framePixelDatas.ContainsKey(_framePosition))
                {
                    return _framePixelDatas[_framePosition];
                }

                if (_texture.Width % _sheetSize.Width != 0 || _texture.Height % _sheetSize.Height != 0)
                {
                    throw new InvalidOperationException("Texture size must be dividable by sheet size to get pixel data");
                }


                if (_pixelData == null)
                {
                    _pixelData = _texture.GetData<Color>();
                }

                Color[] framePixelData;
                if (base.SourceRectangle.HasValue)
                {
                    framePixelData = _texture.GetPartialData<Color>(_pixelData, base.SourceRectangle.Value);
                }
                else
                {
                    framePixelData = _pixelData;
                }

                if (this.SavePixelData)
                {
                    _framePixelDatas.Add(_framePosition, framePixelData);
                }
                else
                {
                    _pixelData = null;
                }

                return framePixelData;
            }
        }

        #endregion

        public AnimatedSprite(Texture2D spriteSheet, int sheetWidth, int sheetHeight, double frameTime)
            : base(spriteSheet, false)
        {
            _sheetSize = new Size(sheetWidth, sheetHeight);
            _frameSize = new Vector2(spriteSheet.Width / (float)sheetWidth, spriteSheet.Height / (float)sheetHeight);

            _frameTime = frameTime;
            _timeUntilNewFrame = frameTime;

            this.UpdateSourceRectangle();
        }

        /// <summary>
        /// Updates the animation if necessary
        /// </summary>
        public override void Update(UpdateContext updateContext)
        {
            if (!this.Frozen)
            {
                _timeUntilNewFrame -= updateContext.GameTime.DeltaSeconds;
                if (_timeUntilNewFrame <= 0)
                {
                    _timeUntilNewFrame += _frameTime;

                    this.UpdateFrame();
                }
            }
        }

        protected virtual void UpdateFrame()
        {
            _framePosition.X++;
            if (_framePosition.X >= _sheetSize.Width)
            {
                _framePosition.X = 0;

                _framePosition.Y++;
                if (_framePosition.Y >= _sheetSize.Height)
                {
                    _framePosition.Y = 0;
                }
            }

            // Update the source rectangle to represent the frame that was just updated
            this.UpdateSourceRectangle();
        }

        protected void UpdateSourceRectangle()
        {
            base.SourceRectangle = new Rectangle(
                (int)(_framePosition.X * _frameSize.X),
                (int)(_framePosition.Y * _frameSize.Y),
                (int)(_frameSize.X),
                (int)(_frameSize.Y));
        }

        public void SetFramePosition(int x, int y)
        {
            Ensure.WithinRange(x, 0, _sheetSize.Width - 1);
            Ensure.WithinRange(y, 0, _sheetSize.Height - 1);

            _framePosition = new Vector2i(x, y);
            this.UpdateSourceRectangle();
        }

        public Rectangle GetSourceRectangle(int x, int y)
        {
            return this.GetSourceRectangle(new Vector2i(x, y));
        }

        public Rectangle GetSourceRectangle(Vector2i frame)
        {
            Ensure.WithinRange(frame.X, 0, _sheetSize.Width - 1, "Parameter frame is out of range");
            Ensure.WithinRange(frame.Y, 0, _sheetSize.Height - 1, "Parameter frame is out of range");

            return new Rectangle(
                (int)Math.Round(frame.X * _frameSize.X),
                (int)Math.Round(frame.Y * _frameSize.Y),
                (int)Math.Round(_frameSize.X),
                (int)Math.Round(_frameSize.Y));
        }
    }
}
