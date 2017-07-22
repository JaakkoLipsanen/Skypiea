using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Ui
{
    public class TextBlock : UiObject
    {
        private string _font = "Default";
        private string _text;
        private Vector2 _position;
        private Color _color = Color.White;
        private float _scale = 1f;

        private bool _areaNeedsUpdate = true;
        private bool _isFaded = false;
        private bool _isCentered = true;

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                {
                    value = "";
                }

                _text = _text.Trim();
                if (_text != value)
                {
                    _text = value;
                    _areaNeedsUpdate = true;
                }
            }
        }

        public string Font
        {
            get { return _font; }
            set
            {
                _font = _font.TrimIfNotNull().NullIfEmpty();
                if (_font != value)
                {
                    _font = value;
                    _areaNeedsUpdate = true;
                }
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool IsFaded
        {
            get { return _isFaded; }
        }

        // Move this to UiObject?
        // Make Transform class?
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public bool IsCentered
        {
            get { return _isCentered; }
            set { _isCentered = value; }
        }

        public TextBlock(string text, Vector2 position)
        {
            _text = text;
            _position = position;
        }

        public override void Update(UpdateContext updateContext)
        {
            if (_areaNeedsUpdate && !string.IsNullOrEmpty(_font))
            {
                IFontContainer fontContainer = updateContext.Services.Get<IFontContainer>();
                Vector2 textSize = fontContainer[_font].MeasureString(_text);
                _area = new RectangleF(_position.X - textSize.X / 2f, _position.Y - textSize.Y / 2f, textSize.X, textSize.Y);
            }
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            if (!string.IsNullOrEmpty(_font) && !string.IsNullOrEmpty(_text))
            {
                SpriteFont font = graphicsContext.FontContainer[_font];
                if (_isCentered)
                {
                    if (_isFaded)
                    {
                        graphicsContext.SpriteBatch.DrawStringFadedCentered(font, _text, _position, Color.Black, _color, 0, _scale);
                    }
                    else
                    {
                        graphicsContext.SpriteBatch.DrawStringCentered(font, _text, _position, _color, 0, _scale);
                    }
                }
                else
                {
                    if (_isFaded)
                    {
                        graphicsContext.SpriteBatch.DrawStringFaded(font, _text, _position, Color.Black, _color, 0, _scale);
                    }
                    else
                    {
                        graphicsContext.SpriteBatch.DrawString(font, _text, _position, _color, 0, _scale);
                    }
                }
            }
            base.Draw(graphicsContext);
        }
    }
}
