
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class Sprite
    {
        protected readonly Texture2D _texture;
        protected Color[] _pixelData;

        public Color Tint = Color.White;
        public float Rotation = 0f;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public float LayerDepth = 0f;

        public bool SavePixelData { get; set; }
        public Rectangle? SourceRectangle { get; protected set; }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public int Width
        {
            get { return this.SourceRectangle.HasValue ? this.SourceRectangle.Value.Width : this.Texture.Width; }
        }

        public int Height
        {
            get { return this.SourceRectangle.HasValue ? this.SourceRectangle.Value.Height : this.Texture.Height; }
        }

        public virtual Color[] PixelData
        {
            get
            {
                if (_pixelData != null)
                {
                    return _pixelData;
                }

                if (this.SavePixelData)
                {
                    _pixelData = new Color[this.Texture.Width * this.Texture.Height];
                    this.Texture.GetData<Color>(_pixelData);
                    return _texture.GetData<Color>(); ;
                }

                return _texture.GetData<Color>();

            }
        }

        public Sprite(Texture2D texture)
            : this(texture, false)
        {
        }

        public Sprite(TextureDefinition texture)
            : this(texture.Texture, false)
        {
            this.SourceRectangle = texture.SourceRectangle;
        }

        public Sprite(Texture2D texture, bool originInTextureCenter)
        {
            Ensure.NotNull(texture);

            _texture = texture;
            if (originInTextureCenter)
            {
                this.Origin = this.Texture.Center();
            }
        }

        public virtual void Update(UpdateContext updateContext) { }
    }
}
