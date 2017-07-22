using System;
using System.Collections.Generic;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Ui
{
    public class TextToggleButton : ToggleButtonBase
    {
        private readonly string _toggledText;
        private readonly string _unToggledText;

        public string Font { get; set; }
        public Color Color { get; set; }

        public TextToggleButton(RectangleF area, string toggledText, string notToggledText)
            : this(area, toggledText, notToggledText, null)
        {
        }

        public TextToggleButton(RectangleF area, string toggledText, string notToggledText, GenericEvent<bool> toggledEvent)
            : base(area)
        {
            _toggledText = toggledText.NullIfEmpty();
            _unToggledText = notToggledText.NullIfEmpty();

            Ensure.NotNull(_toggledText);
            Ensure.NotNull(_unToggledText);

            this.Color = Color.White;

            if (toggledEvent != null)
            {
                this.Toggled += toggledEvent;
            }
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            string text = this.IsToggled ? _toggledText : _unToggledText;
            SpriteFont font = graphicsContext.FontContainer[this.Font ?? "Default"];

            graphicsContext.SpriteBatch.DrawStringCentered(font, text, _area.Center, this.Color, 0, (this.IsTouchedDown) ? 0.9f : 1f);
        }
    }

    public class TextMultiToggleButton<T> : MultiToggleButtonBase<T>
    {
        private readonly Func<T, string> _toStringFunction;

        public string Font { get; set; }
        public Color Color { get; set; }

        public TextMultiToggleButton(RectangleF area, IList<T> values)
            : this(area, values, value => value.ToString())
        {
        }

        public TextMultiToggleButton(RectangleF area, IList<T> values, Func<T, string> toStringFunction)
            : this(area, values, toStringFunction, null)
        {
        }

        public TextMultiToggleButton(RectangleF area, IList<T> values, Func<T, string> toStringFunction, GenericEvent<T> toggledEvent)
            : base(area, values)
        {

            Ensure.NotNull(toStringFunction);
            _toStringFunction = toStringFunction;
            this.Color = Color.White;

            if (toggledEvent != null)
            {
                this.Toggled += toggledEvent;
            }
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            string text = _toStringFunction(this.SelectedValue); // should the toStringFunction be called only once when the selected value changes or always..?
            SpriteFont font = graphicsContext.FontContainer[this.Font ?? "Default"];

            graphicsContext.SpriteBatch.DrawStringCentered(font, text, _area.Center, this.Color, 0, (this.IsTouchedDown) ? 0.9f : 1f);
        }
    }
}
