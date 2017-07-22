using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    // todo: ITextureDefinition so there can be for example AnimatedTextureDefinition tms
    public struct TextureDefinition
    {
        public readonly Texture2D Texture;
        public readonly Rectangle? SourceRectangle;

        public int Width
        {
            get { return this.SourceRectangle.HasValue ? this.SourceRectangle.Value.Width : (this.Texture != null ? this.Texture.Width : 0); }
        }

        public int Height
        {
            get { return this.SourceRectangle.HasValue ? this.SourceRectangle.Value.Height : (this.Texture != null ? this.Texture.Height : 0); }
        }

        public Vector2 Origin
        {
            get { return new Vector2(this.Width / 2f, this.Height / 2f); }
        }

        public TextureDefinition(Texture2D texture)
            : this(texture, null)
        {
        }

        public TextureDefinition(Texture2D texture, Rectangle? sourceRectangle)
            : this()
        {
            Ensure.NotNull(texture);

            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
        }

        public static bool operator ==(TextureDefinition left, TextureDefinition right)
        {
            return (left.Texture == right.Texture && left.SourceRectangle == right.SourceRectangle);
        }

        public static bool operator !=(TextureDefinition left, TextureDefinition right)
        {
            return !(left == right);
        }

        public static implicit operator TextureDefinition(Texture2D texture)
        {
            return new TextureDefinition(texture);
        }

        public bool Equals(TextureDefinition other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextureDefinition)
            {
                return this == (TextureDefinition)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((this.Texture != null ? this.Texture.GetHashCode() : 0) * 397) ^ this.SourceRectangle.GetHashCode();
        }

        public static readonly TextureDefinition Empty = new TextureDefinition();
    }
}
