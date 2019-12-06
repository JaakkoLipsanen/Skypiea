using System;
using Flai.DataStructures;
using Flai.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public enum Corner // ehh name?
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
    }

    public struct ColorChannels
    {
        public static readonly ColorChannels All = new ColorChannels(1, 1, 1);

        public readonly float R;
        public readonly float G;
        public readonly float B;

        public ColorChannels(float r, float g, float b)
        {
            Ensure.IsValid(r);
            Ensure.WithinRange(r, 0, 1);

            Ensure.IsValid(g);
            Ensure.WithinRange(g, 0, 1);

            Ensure.IsValid(b);
            Ensure.WithinRange(b, 0, 1);

            this.R = r;
            this.G = g;
            this.B = b;
        }

        public static ColorChannels operator *(ColorChannels left, ColorChannels right)
        {
            return new ColorChannels(left.R * right.R, left.G * right.G, left.B * right.B);
        }
    }

    public class FlaiSpriteBatch
    {
        private readonly SpriteBatch _innerSpriteBatch;
        private readonly GraphicsDevice _graphicsDevice;

        private readonly ValueStackAggregatorWithPredicate<float> _alphaStack = new ValueStackAggregatorWithPredicate<float>(1, (oldValue, newValue) => oldValue * newValue, value => Check.IsValid(value) && 0 <= value && value <= 1);
        private readonly ValueStackAggregatorWithPredicate<Vector2> _offsetStack = new ValueStackAggregatorWithPredicate<Vector2>(Vector2.Zero, (oldValue, newValue) => oldValue + newValue, Check.IsValid);
        private readonly ValueStackAggregator<ColorChannels> _colorChannelStack = new ValueStackAggregator<ColorChannels>(Flai.Graphics.ColorChannels.All, (oldValue, newValue) => oldValue * newValue);

        public IValueStack<float> Alpha
        {
            get { return _alphaStack; }
        }

        public IValueStack<Vector2> Offset
        {
            get { return _offsetStack; }
        }

        public IValueStack<ColorChannels> ColorChannels
        {
            get { return _colorChannelStack; }
        }

        // can be used for high-performance stuff. using this *CAN* mess up the FlaiSpriteBatch's state, so use carefully
        public SpriteBatch InnerSpriteBatch
        {
            get { return _innerSpriteBatch; }
        }

        public bool IsRunning { get; private set; }

        public FlaiSpriteBatch(GraphicsDevice graphicsDevice)
        {
            _innerSpriteBatch = new SpriteBatch(graphicsDevice);
            _graphicsDevice = graphicsDevice;
        }

        #region Begin

        public void Begin()
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);
        }

        public void Begin(SpriteSortMode spriteSortMode)
        {
            this.BeginInner(spriteSortMode, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);
        }

        public void Begin(BlendState blendState)
        {
            this.BeginInner(SpriteSortMode.Deferred, blendState, null, null, null, null, Matrix.Identity);
        }

        public void Begin(BlendState blendState, SamplerState samplerState)
        {
            this.BeginInner(SpriteSortMode.Deferred, blendState, samplerState, null, null, null, Matrix.Identity);
        }

        public void Begin(SpriteSortMode spriteSortMode, Matrix transform)
        {
            this.BeginInner(spriteSortMode, BlendState.AlphaBlend, null, null, null, null, transform);
        }

        public void Begin(SpriteSortMode spriteSortMode, BlendState blendState, Matrix transform)
        {
            this.BeginInner(spriteSortMode, blendState, null, null, null, null, transform);
        }

        public void Begin(SamplerState samplerState)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, null, null, null, Matrix.Identity);
        }

        public void Begin(SamplerState samplerState, Matrix transform)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, null, null, null, transform);
        }

        public void Begin(Matrix transform)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, transform);
        }

        public void Begin(Effect effect, Matrix transform)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, effect, transform);
        }

        public void Begin(Effect effect)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, effect, Matrix.Identity);
        }

        public void Begin(ICamera2D camera)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.GetTransform());
        }

        public void Begin(SamplerState samplerState, ICamera2D camera)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, null, null, null, camera.GetTransform());
        }

        public void Begin(Effect effect, ICamera2D camera)
        {
            this.BeginInner(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, effect, camera.GetTransform());
        }

        public void Begin(SpriteSortMode spriteSortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            this.BeginInner(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
        }

        public void Begin(SpriteSortMode spriteSortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            this.BeginInner(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        protected virtual void BeginInner(SpriteSortMode spriteSortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            _innerSpriteBatch.Begin(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            this.IsRunning = true;
        }

        #endregion

        #region Draw Texture

        protected virtual void DrawInner(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            this.ApplyModifiers(ref color, ref position);

#if WP8_MONOGAME
            //Common.SwapReferences(ref position.X, ref position.Y);
            //position.Y = _graphicsDevice.GetScreenSize().Height - position.X;
            //rotation += FlaiMath.PiOver2;
#endif
            _innerSpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        protected virtual void DrawInner(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            this.ApplyModifiers(ref color, ref destinationRectangle);

#if WP8_MONOGAME
            //Common.SwapReferences(ref destinationRectangle.X, ref destinationRectangle.Y);
            //Common.SwapReferences(ref destinationRectangle.Width, ref destinationRectangle.Height);
            //destinationRectangle.X = _graphicsDevice.GetScreenSize().Height - destinationRectangle.X;
            //rotation += FlaiMath.PiOver2;
#endif

            _innerSpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            this.DrawInner(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(Sprite sprite, Rectangle area)
        {
            this.DrawInner(sprite.Texture, area, sprite.SourceRectangle, sprite.Tint, sprite.Rotation, Vector2.Zero, sprite.SpriteEffects, sprite.LayerDepth);
        }

        public void Draw(Sprite sprite, RectangleF area)
        {
            Vector2 scale = new Vector2(area.Width / sprite.Width, area.Height / sprite.Height) * sprite.Scale;
            this.DrawInner(sprite.Texture, area.TopLeft, sprite.SourceRectangle, sprite.Tint, sprite.Rotation, Vector2.Zero, scale, sprite.SpriteEffects, sprite.LayerDepth);
        }

        public void Draw(Sprite sprite, Vector2 position)
        {
            this.DrawInner(sprite.Texture, position, sprite.SourceRectangle, sprite.Tint, sprite.Rotation, sprite.Origin, sprite.Scale, sprite.SpriteEffects, sprite.LayerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position)
        {
            this.DrawInner(texture, position, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            this.DrawInner(texture, position, null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            this.DrawInner(texture, position, sourceRectangle, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawInner(texture, position, null, color, rotation, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawInner(texture, position, null, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
        {
            this.DrawInner(texture, position, null, color, rotation, origin, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale)
        {
            this.DrawInner(texture.Texture, position, GraphicsHelper.CombineSourceRectangle(texture.SourceRectangle, sourceRectangle), color, rotation, origin, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(texture.Texture, position, GraphicsHelper.CombineSourceRectangle(texture.SourceRectangle, sourceRectangle), color, rotation, origin, new Vector2(scale, scale), spriteEffects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            this.DrawInner(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, float scale)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0f);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, float scale)
        {
            this.DrawInner(texture.Texture, position, GraphicsHelper.CombineSourceRectangle(texture.SourceRectangle, sourceRectangle), color, rotation, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0f);
        }
        // RectangleF
        public void Draw(Texture2D texture, Rectangle destinationRectangle)
        {
            this.DrawInner(texture, destinationRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        // RectangleF
        public void Draw(TextureDefinition textureDefinition, Rectangle destinationRectangle)
        {
            this.DrawInner(textureDefinition.Texture, destinationRectangle, textureDefinition.SourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, RectangleF destinationRectangle)
        {
            Vector2 scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            this.DrawInner(texture, destinationRectangle.TopLeft, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color)
        {
            Vector2 scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            this.DrawInner(texture, destinationRectangle.TopLeft, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceArea, Color color)
        {
            Vector2 scale;
            if (sourceArea.HasValue)
            {
                scale = new Vector2(destinationRectangle.Width / sourceArea.Value.Width, destinationRectangle.Height / sourceArea.Value.Height);
            }
            else
            {
                scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            }
            this.DrawInner(texture, destinationRectangle.TopLeft, sourceArea, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color, float rotation)
        {
            Vector2 scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            this.DrawInner(texture, destinationRectangle.TopLeft, null, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin)
        {
            Vector2 scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            this.DrawInner(texture, destinationRectangle.TopLeft, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        // Not sure if correct
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects spriteEffects, float layerDepth)
        {
            Vector2 scale = new Vector2(destinationRectangle.Width / texture.Width, destinationRectangle.Height / texture.Height);
            this.DrawInner(texture, destinationRectangle.TopLeft, sourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        // Draw Centered
        public void DrawCentered(Texture2D texture, Vector2 position)
        {
            this.DrawInner(texture, position, null, Color.White, 0f, texture.Center(), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Color color)
        {
            this.DrawInner(texture, position, null, color, 0f, texture.Center(), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawCentered(TextureDefinition texture, Vector2 position, Color color)
        {
            this.DrawInner(texture.Texture, position, texture.SourceRectangle, color, 0f, texture.Origin, Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Color color, float rotation)
        {
            this.DrawInner(texture, position, null, color, rotation, texture.Center(), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawInner(texture, position, null, color, rotation, texture.Center(), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void DrawCentered(TextureDefinition texture, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawInner(texture.Texture, position, texture.SourceRectangle, color, rotation, texture.Origin, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawInner(texture, position, null, color, rotation, texture.Center(), scale, SpriteEffects.None, 0f);
        }

        public void DrawCentered(TextureDefinition texture, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawInner(texture.Texture, position, texture.SourceRectangle, color, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            this.DrawInner(texture, position, sourceRectangle, color, 0f, texture.Center(), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, texture.Center(), new Vector2(scale, scale), spriteEffects, layerDepth);
        }

        public void DrawCentered(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(texture, position, sourceRectangle, color, rotation, texture.Center(), scale, spriteEffects, layerDepth);
        }

        public void DrawFullscreen(Texture2D texture)
        {
            this.DrawInner(texture, _graphicsDevice.Viewport.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(TextureDefinition texture)
        {
            this.DrawInner(texture.Texture, _graphicsDevice.Viewport.Bounds, texture.SourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(TextureDefinition texture, Color color)
        {
            this.DrawInner(texture.Texture, _graphicsDevice.Viewport.Bounds, texture.SourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(Texture2D texture, Camera2D camera)
        {
            this.Draw(texture, camera.GetArea(texture.GraphicsDevice), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(Texture2D texture, Camera2D camera, Color color)
        {
            this.Draw(texture, camera.GetArea(texture.GraphicsDevice), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(Texture2D texture, Color color)
        {
            this.DrawInner(texture, _graphicsDevice.Viewport.Bounds, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(Texture2D texture, Color color, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(texture, _graphicsDevice.Viewport.Bounds, null, color, 0f, Vector2.Zero, spriteEffects, layerDepth);
        }

        public void DrawFullscreen(Texture2D texture, Rectangle? sourceRectangle, Color color)
        {
            this.DrawInner(texture, _graphicsDevice.Viewport.Bounds, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawFullscreen(Texture2D texture, Rectangle? sourceRectangle, Color color, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(texture, _graphicsDevice.Viewport.Bounds, sourceRectangle, color, 0f, Vector2.Zero, spriteEffects, layerDepth);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color)
        {
            this.Draw(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, origin, new Vector2(scale), SpriteEffects.None, 0);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, origin, new Vector2(scale), spriteEffects, layerDepth);
        }

        public void Draw(TextureDefinition textureDefinition, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawInner(textureDefinition.Texture, position, textureDefinition.SourceRectangle, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Corner anchorCorner, Color color)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height;
            }

            this.Draw(texture, position, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Corner anchorCorner, Color color, float rotation, float scale)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.Draw(texture, position, color, rotation, scale);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Corner anchorCorner, Color color, float rotation, float scale)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.Draw(texture.Texture, position, texture.SourceRectangle, color, rotation, scale);
        }

        public void Draw(Texture2D texture, Vector2 position, Corner anchorCorner, Rectangle? sourceRectangle, Color color, float rotation, float scale)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.Draw(texture, position, sourceRectangle, color, rotation, scale);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Corner anchorCorner, Rectangle? sourceRectangle, Color color, float rotation, float scale)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.Draw(texture.Texture, position, GraphicsHelper.CombineSourceRectangle(texture.SourceRectangle, sourceRectangle), color, rotation, scale);
        }

        public void Draw(Texture2D texture, Vector2 position, Corner anchorCorner, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.DrawInner(texture, position, null, color, rotation, Vector2.Zero, new Vector2(scale), spriteEffects, 0f);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Corner anchorCorner, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.DrawInner(texture.Texture, position, texture.SourceRectangle, color, rotation, Vector2.Zero, new Vector2(scale), spriteEffects, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Corner anchorCorner, Rectangle? sourceRectangle, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.DrawInner(texture, position, sourceRectangle, color, rotation, Vector2.Zero, new Vector2(scale), spriteEffects, 0f);
        }

        public void Draw(TextureDefinition texture, Vector2 position, Corner anchorCorner, Rectangle? sourceRectangle, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.TopRight)
            {
                position.X -= texture.Width * scale;
            }

            if (anchorCorner == Corner.BottomRight || anchorCorner == Corner.BottomLeft)
            {
                position.Y -= texture.Height * scale;
            }

            this.DrawInner(texture.Texture, position, GraphicsHelper.CombineSourceRectangle(texture.SourceRectangle, sourceRectangle), color, rotation, Vector2.Zero, new Vector2(scale), spriteEffects, 0f);
        }

        #endregion

        #region Draw String

        protected virtual void DrawStringInner(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.ApplyModifiers(ref color, ref position);

#if WP8_MONOGAME
            //Common.SwapReferences(ref position.X, ref position.Y);
            //position.X = _graphicsDevice.GetScreenSize().Height - position.X;
            //rotation += FlaiMath.PiOver2;
#endif

            _innerSpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        protected virtual void DrawStringInner(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.ApplyModifiers(ref color, ref position);

#if WP8_MONOGAME
            //Common.SwapReferences(ref position.X, ref position.Y);
            //position.Y = _graphicsDevice.GetScreenSize().Height - position.X;
            //rotation += FlaiMath.PiOver2;
#endif

            _innerSpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringInner(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringInner(spriteFont, text, position, color, rotation, origin, scale, spriteEffects, layerDepth);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            this.DrawStringInner(font, text, position, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Corner anchorCorner, Color color)
        {
            Vector2 size = font.MeasureString(text);
            if (anchorCorner == Corner.BottomLeft || anchorCorner == Corner.BottomRight)
            {
                position.Y -= size.Y;
            }

            if (anchorCorner == Corner.TopRight || anchorCorner == Corner.BottomRight)
            {
                position.X -= size.X;
            }

            this.DrawString(font, text, position, color);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position)
        {
            this.DrawString(font, text, position, Color.White);
        }

        public void DrawString(SpriteFont font, char character, Vector2 position)
        {
            this.DrawString(font, Common.CharacterToString(character), position, Color.White);
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawStringInner(font, text, position, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, char character, Vector2 position)
        {
            this.DrawStringInner(font, Common.CharacterToString(character), position, Color.White, 0f, font.Center(character), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position)
        {
            this.DrawStringInner(font, text, position, Color.White, 0f, font.Center(text), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, char character, Vector2 position, Color color)
        {
            this.DrawStringInner(font, Common.CharacterToString(character), position, color, 0f, font.Center(character), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, char character, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawStringInner(font, Common.CharacterToString(character), position, color, rotation, font.Center(character), scale, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, char character, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawStringInner(font, Common.CharacterToString(character), position, color, rotation, font.Center(character), scale, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color)
        {
            this.DrawStringInner(font, text, position, color, 0f, font.Center(text), Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale)
        {
            this.DrawStringInner(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringInner(font, text, position, color, rotation, font.Center(text), scale, spriteEffects, layerDepth);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            this.DrawStringInner(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None, 0f);
        }

        public void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringInner(font, text, position, color, rotation, font.Center(text), scale, spriteEffects, layerDepth);
        }

        public void DrawStringFaded(SpriteFont font, string text, Vector2 position)
        {
            this.DrawString(font, text, position, Color.Black);
            this.DrawString(font, text, position + Vector2.One, Color.White);
        }

        public void DrawStringFaded(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor)
        {
            this.DrawString(font, text, position, backColor);
            this.DrawString(font, text, position + Vector2.One, frontColor);
        }

        // This is wrong. With rotation, it shouldn't add Vector2.One to the latter string, but rather do some vector mathematics and calculate the correct direction
        public void DrawStringFaded(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor, float rotation, float scale)
        {
            this.DrawString(font, text, position, backColor, rotation, scale);
            this.DrawString(font, text, position + Vector2.One, frontColor, rotation, scale);
        }

        // This is wrong. With rotation, it shouldn't add Vector2.One to the latter string, but rather do some vector mathematics and calculate the correct direction
        public void DrawStringFaded(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringInner(font, text, position, backColor, rotation, origin, scale, spriteEffects, layerDepth);
            this.DrawStringInner(font, text, position + Vector2.One, frontColor, rotation, origin, scale, spriteEffects, layerDepth);
        }

        public void DrawStringFadedCentered(SpriteFont font, string text, Vector2 position)
        {
            this.DrawStringCentered(font, text, position, Color.Black);
            this.DrawStringCentered(font, text, position + Vector2.One, Color.White);
        }

        public void DrawStringFadedCentered(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor)
        {
            this.DrawStringCentered(font, text, position, backColor);
            this.DrawStringCentered(font, text, position + Vector2.One, frontColor);
        }

        // This is wrong. With rotation, it shouldn't add Vector2.One to the latter string, but rather do some vector mathematics and calculate the correct direction
        public void DrawStringFadedCentered(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor, float rotation, float scale)
        {
            this.DrawStringCentered(font, text, position, backColor, rotation, scale);
            this.DrawStringCentered(font, text, position + Vector2.One, frontColor, rotation, scale);
        }

        // This is wrong. With rotation, it shouldn't add Vector2.One to the latter string, but rather do some vector mathematics and calculate the correct direction
        public void DrawStringFadedCentered(SpriteFont font, string text, Vector2 position, Color backColor, Color frontColor, float rotation, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            this.DrawStringCentered(font, text, position, backColor, rotation, scale, spriteEffects, layerDepth);
            this.DrawStringCentered(font, text, position + Vector2.One, frontColor, rotation, scale, spriteEffects, layerDepth);
        }

        public void DrawStringFadedCentered(SpriteFont font, char character, Vector2 position)
        {
            this.DrawStringCentered(font, character, position, Color.Black);
            this.DrawStringCentered(font, character, position + Vector2.One, Color.White);
        }

        public void DrawStringFadedCentered(SpriteFont font, char character, Vector2 position, float rotation, float scale)
        {
            this.DrawStringCentered(font, character, position, Color.Black, rotation, scale);
            this.DrawStringCentered(font, character, position + Vector2.One, Color.White, rotation, scale);
        }

        public void DrawStringFadedCentered(SpriteFont font, char character, Vector2 position, Color backColor, Color frontColor, float rotation, float scale)
        {
            this.DrawStringCentered(font, character, position, backColor, rotation, scale);
            this.DrawStringCentered(font, character, position + Vector2.One, frontColor, rotation, scale);
        }

        #endregion

        #region End

        public void End()
        {
            _innerSpriteBatch.End();
            this.IsRunning = false;
        }

        #endregion

        internal void VerifyIsReadyForEndOfFrame()
        {
            if (this.IsRunning || !_colorChannelStack.IsEmpty || !_offsetStack.IsEmpty || !_alphaStack.IsEmpty)
            {
                throw new InvalidOperationException("Invalid state");
            }
        }

        private void ApplyModifiers(ref Color color, ref Vector2 position)
        {
            if (!_offsetStack.IsEmpty)
            {
                position += _offsetStack.CurrentValue;
            }

            if (!_colorChannelStack.IsEmpty)
            {
                ColorChannels channels = _colorChannelStack.CurrentValue;
                color = new Color((byte)(color.R * channels.R), (byte)(color.G * channels.G), (byte)(color.B * channels.B), color.A);
            }

            if (!_alphaStack.IsEmpty)
            {
                color *= _alphaStack.CurrentValue;
            }
        }

        private void ApplyModifiers(ref Color color, ref Rectangle destinationRectangle)
        {
            if (!_offsetStack.IsEmpty)
            {
                destinationRectangle.Offset((int)_offsetStack.CurrentValue.X, (int)_offsetStack.CurrentValue.Y);
            }

            if (!_colorChannelStack.IsEmpty)
            {
                ColorChannels channels = _colorChannelStack.CurrentValue;
                color = new Color((byte)(color.R * channels.R), (byte)(color.G * channels.G), (byte)(color.B * channels.B), color.A);
            }

            if (!_alphaStack.IsEmpty)
            {
                color *= _alphaStack.CurrentValue;
            }
        }
    }
}
