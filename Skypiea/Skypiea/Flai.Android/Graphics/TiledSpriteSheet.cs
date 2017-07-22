using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class TileSpriteSheet : ITileSpriteSheet
    {
        private readonly Texture2D _spriteSheetTexture;
        private readonly Size _sheetSize;

        private readonly Vector2 _frameSize;

        public Texture2D Texture
        {
            get { return _spriteSheetTexture; }
        }

        public Size SheetSize
        {
            get { return _sheetSize; }
        }

        public Vector2 FrameSize
        {
            get { return _frameSize; }
        }

        public TileSpriteSheet(Texture2D spriteSheetTexture, int width, int height)
        {
            _spriteSheetTexture = spriteSheetTexture;
            _sheetSize = new Size(width, height);

            _frameSize = new Vector2(_spriteSheetTexture.Width / (float)width, _spriteSheetTexture.Height / (float)height);
        }

        public Rectangle GetSourceRectangle(Vector2i frame)
        {
            return this.GetSourceRectangle(frame.X, frame.Y);
        }

        public Rectangle GetSourceRectangle(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _sheetSize.Width || y >= _sheetSize.Height)
            {
                throw new ArgumentOutOfRangeException("Parameter frame is out of range");
            }

            return new Rectangle(
                (int)Math.Round(x * _frameSize.X),
                (int)Math.Round(y * _frameSize.Y),
                (int)Math.Round(_frameSize.X),
                (int)Math.Round(_frameSize.Y));
        }

        public RectangleF GetUvCoordinates(Vector2i frame)
        {
            return this.GetUvCoordinates(frame.X, frame.Y);
        }

        public RectangleF GetUvCoordinates(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _sheetSize.Width || y >= _sheetSize.Height)
            {
                throw new ArgumentOutOfRangeException("Parameter frame is out of range");
            }

            return new RectangleF(
                (x * _frameSize.X) / _spriteSheetTexture.Width,
                (y * _frameSize.Y) / _spriteSheetTexture.Height,
                _frameSize.X / _spriteSheetTexture.Width,
                _frameSize.Y / _spriteSheetTexture.Height);
        }
    }
}
