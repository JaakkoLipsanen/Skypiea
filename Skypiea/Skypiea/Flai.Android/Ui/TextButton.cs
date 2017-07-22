using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Ui
{
    public class TextButton : ButtonBase
    {
        private bool _needsToUpdateArea = true;
        protected string _font = "Default";
        protected string _text = "";

#if WINDOWS
        private int _inflateAmount = 0;
#elif WINDOWS_PHONE
        private int _inflateAmount = 24;
#endif

        protected Vector2 _centerPosition;
        private Color _color = Color.White;
        private bool _scaleDownIfPressed = true;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value && !string.IsNullOrEmpty(value))
                {
                    _text = value;
                }
            }
        }

        public string Font
        {
            get { return _font; }
            set
            {
                if (_font != value && !string.IsNullOrEmpty(value))
                {
                    _font = value;
                    _needsToUpdateArea = true;
                }
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public int InflateAmount
        {
            get { return _inflateAmount; }
            set { _inflateAmount = value; }
        }

        public bool ScaleDownIfPressed
        {
            get { return _scaleDownIfPressed; }
            set { _scaleDownIfPressed = value; }
        }

        public bool DrawFaded { get; set; }

        public TextButton(string text, Vector2 position)
        {
            _centerPosition = position;
            this.Text = text;

            _area = new RectangleF(_centerPosition.X, _centerPosition.Y, 1, 1);
        }

        public TextButton(string text, Vector2 position, GenericEvent clickFunction)
            : this(text, position)
        {
            this.Click += clickFunction;
        }

        public TextButton(string text, Vector2 position, GenericEvent<TextButton> clickFunction)
            : this(text, position)
        {
            this.Click += () => clickFunction(this);
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
            if (_needsToUpdateArea)
            {
                this.UpdateArea();
            }
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            SpriteFont font = graphicsContext.FontContainer[this.Font];
            if (font != null)
            {
#if WINDOWS
                bool isPressedDown = base.IsPressedDown;
#elif WINDOWS_PHONE
                bool isPressedDown = base.IsTouchedDown;
#endif
                // Meh, idk how I would do this better.. Create a "DisabledColor" property maybe?
                Color finalColor = this.Enabled ? this.Color : Color.Lerp(this.Color, Color.Black, 0.5f);

                if (this.DrawFaded)
                {
                    graphicsContext.SpriteBatch.DrawStringFadedCentered(font, this.Text, base.Area.Center, Color.Black, finalColor, 0f, (_scaleDownIfPressed && isPressedDown) ? 0.9f : 1f);
                }
                else
                {
                    graphicsContext.SpriteBatch.DrawStringCentered(font, this.Text, base.Area.Center, finalColor, 0f, (_scaleDownIfPressed && isPressedDown) ? 0.9f : 1f);
                }

            //  graphicsContext.SpriteBatch.Draw(graphicsContext.BlankTexture, this.Area, Color.Red * 0.4f);
            }
        }

        protected void UpdateArea()
        {
            IFontContainer fontProvider = FlaiGame.Current.Services.Get<IFontContainer>();
            if (fontProvider != null)
            {
                SpriteFont font = fontProvider[this.Font];
                Vector2 textSize = font.MeasureString(this.Text);
                _area = new RectangleF(_centerPosition.X - textSize.X / 2, _centerPosition.Y - textSize.Y / 2, textSize.X, textSize.Y).Inflate(_inflateAmount, _inflateAmount);
                _needsToUpdateArea = false;
            }
        }
    }
}
