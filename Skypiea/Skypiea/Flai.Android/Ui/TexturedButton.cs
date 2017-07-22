
using Flai.General;
using Flai.Graphics;
using Microsoft.Xna.Framework;

namespace Flai.Ui
{
    public class TexturedButton : ButtonBase
    {
        protected RectangleF _visualArea;
        public Sprite Sprite { get; private set; }

        public float InflateAmount
        {
            set
            {
                Ensure.IsValid(value);
                _area = new RectangleF(_visualArea.Left - value, _visualArea.Top - value, _visualArea.Width + value * 2, _visualArea.Height + value * 2);
            }
        }

        public TexturedButton(Sprite sprite, Vector2 centerPosition)
            : this(sprite, new RectangleF(centerPosition.X - sprite.Width / 2f * sprite.Scale.X, centerPosition.Y - sprite.Height / 2f * sprite.Scale.Y, sprite.Width * sprite.Scale.Y, sprite.Height * sprite.Scale.Y))
        {
        }

        public TexturedButton(Sprite sprite, Vector2 centerPosition, GenericEvent clicked)
            : this(sprite, new RectangleF(centerPosition.X - sprite.Width / 2f * sprite.Scale.X, centerPosition.Y - sprite.Height / 2f * sprite.Scale.Y, sprite.Width * sprite.Scale.Y, sprite.Height * sprite.Scale.Y))
        {
            this.Click += clicked;
        }

        public TexturedButton(Sprite sprite, RectangleF area)
            : base(area)
        {
            this.Sprite = sprite;
            _visualArea = area.ToRectangle(RoundingOptions.Round);
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Draw(this.Sprite, _visualArea);
        //  graphicsContext.SpriteBatch.Draw(graphicsContext.BlankTexture, this.Area, Color.Red * 0.4f);
        }
    }
}
