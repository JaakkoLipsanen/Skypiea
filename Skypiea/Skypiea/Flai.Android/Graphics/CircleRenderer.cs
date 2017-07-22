using Flai.Graphics.Effects;
using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    public class CircleRenderer
    {
        private readonly CircleEffect _effect;
        public CircleRenderer()
        {
            _effect = new CircleEffect(FlaiGame.Current.GraphicsDevice);
        }

        public void Begin(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(_effect);
        }

        public void End(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.End();
        }

        // size = radius, not diameter!
        public void DrawCircle(GraphicsContext graphicsContext, Vector2 position, Color color, float size)
        {
            _effect.FillColor = color;
            _effect.StrokeColor = Color.Transparent;
            _effect.StrokeSize = Vector2.Zero;
            _effect.UseStrokeSmoothing = false;

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, position, color, 0, size * 2);
        }

        public void DrawCircle(GraphicsContext graphicsContext, Vector2 position, Color color, float size, Color strokeColor, float strokeSize)
        {
            this.DrawCircle(graphicsContext, position, color, size, strokeColor, strokeSize, false);
        }

        // size = radius, not diameter!
        public void DrawCircle(GraphicsContext graphicsContext, Vector2 position, Color color, float size, Color strokeColor, float strokeSize, bool smoothStroke)
        {
            _effect.FillColor = color;
            _effect.StrokeColor = strokeColor;
            _effect.StrokeSize = new Vector2(strokeSize) / (size * 2);
            _effect.UseStrokeSmoothing = smoothStroke;

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, position, color, 0, size * 2);
        }

        public void DrawCircle(GraphicsContext graphicsContext, Vector2 position, Color color, Vector2 size, Color strokeColor, float strokeSize)
        {
            this.DrawCircle(graphicsContext, position, color, size, strokeColor, strokeSize, false);
        }

        // size = radius, not diameter!
        public void DrawCircle(GraphicsContext graphicsContext, Vector2 position, Color color, Vector2 size, Color strokeColor, float strokeSize, bool smoothStroke)
        {
            _effect.FillColor = color;
            _effect.StrokeColor = strokeColor;
            _effect.StrokeSize = new Vector2(strokeSize / (size.X * 2), strokeSize / (size.Y * 2));
            _effect.UseStrokeSmoothing = smoothStroke;

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, position, color, 0, size * 2);
        } 
    }
}
