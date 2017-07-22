#if WINDOWS

using System;
using Flai.Graphics;
using Flai.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai
{
    // Use pragma to disable "FlaiGame.Current.IsMouseVisible is obsolete..." -warnings
#pragma warning disable 0618
    public static class Cursor
    {
        public static event EventHandler VisibilityChanged;

        private static Texture2D _cursorTexture;
        private static Color _tint = Color.White;
        private static bool _visible = false;

        public static Texture2D CursorTexture
        {
            get { return _cursorTexture; }
            set
            {
                _cursorTexture = value;
                if (_cursorTexture == null)
                {
                    FlaiGame.Current.IsMouseVisible = _visible;
                }
                else
                {
                    FlaiGame.Current.IsMouseVisible = false;
                }
            }
        }

        public static Color Tint
        {
            get { return _tint; }
            set { _tint = value; }
        }

        public static bool IsVisible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    FlaiGame.Current.IsMouseVisible = _visible && _cursorTexture == null;

                    Common.InvokeIfNotNull(Cursor.VisibilityChanged, null);
                }
            }
        }

        internal static void Draw(GraphicsContext graphicsContext)
        {
            if (_cursorTexture != null && _visible)
            {
                graphicsContext.SpriteBatch.Begin();
                graphicsContext.SpriteBatch.Draw(_cursorTexture, SystemInput.MousePosition.ToVector2(), null, _tint, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                graphicsContext.SpriteBatch.End();
            }
        }
    }
#pragma warning restore 0618
}

#endif
