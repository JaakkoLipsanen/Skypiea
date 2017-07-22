#if WINDOWS

using System;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flai.ScreenManagement.Screens
{
    public class TextScreen : GameScreen
    {
        private readonly string _text;
        private readonly string _fontName;
        private readonly Color _backgroundColor;
        private readonly Keys _actionKey;
        private readonly Action _action;
        private readonly bool _canDoActionMultipleTimes;

        private bool _hasDoneAction = false;

        private SpriteFont _font;

        public TextScreen(string text, string fontName, Keys actionKey, Action action)
            : this(text, fontName, actionKey, action, Color.Black, false)
        {
        }

        public TextScreen(string text, string fontName, Keys actionKey, Action action, Color backgroundColor, bool canDoActionMultipleTimes = false)
        {
            _text = text;
            _fontName = fontName;
            _actionKey = actionKey;
            _action = action;
            _backgroundColor = backgroundColor;
            _canDoActionMultipleTimes = canDoActionMultipleTimes;

            base.TransitionOnTime = TimeSpan.FromSeconds(1);
            base.TransitionOffTime = TimeSpan.FromSeconds(1);
            base.FadeType = FadeType.FadeToBlack;
        }

        protected override void LoadContent()
        {
            _font = base.FontContainer[_fontName];
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (updateContext.InputState.IsNewKeyPress(_actionKey) && (_canDoActionMultipleTimes || !_hasDoneAction))
            {
                _action();
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.GraphicsDevice.Clear(_backgroundColor);

            graphicsContext.SpriteBatch.Begin();
            graphicsContext.SpriteBatch.DrawStringCentered(_font, _text, graphicsContext.ScreenSize.ToVector2i() / 2f);
            graphicsContext.SpriteBatch.End();
        }
    }
}
#endif