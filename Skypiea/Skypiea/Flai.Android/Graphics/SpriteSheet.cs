using System.Collections.Generic;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class SpriteSheet : ISpriteSheet
    {
        private readonly Texture2D _texture;
        private readonly Dictionary<string, Rectangle> _sprites = new Dictionary<string, Rectangle>();
        private readonly ReadOnlyDictionary<string, Rectangle> _readOnlySprites;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public ReadOnlyDictionary<string, Rectangle> Sprites
        {
            get { return _readOnlySprites; }
        }

        public SpriteSheet(Texture2D texture, Dictionary<string, Rectangle> sprites)
        {
            Ensure.NotNull(texture);
            _texture = texture;
            _sprites = new Dictionary<string, Rectangle>(sprites);

            _readOnlySprites = new ReadOnlyDictionary<string, Rectangle>(_sprites);
        }

        public bool ContainsSprite(string name)
        {
            return _sprites.ContainsKey(name);
        }

        public Rectangle GetSourceRectangle(string spriteName)
        {
            return _sprites[spriteName];
        }

        public RectangleF GetUvCoordinates(string spriteName)
        {
            Rectangle sourceRectangle = _sprites[spriteName];
            return new RectangleF(
                sourceRectangle.X / (float)_texture.Width,
                sourceRectangle.Y / (float)_texture.Height,
                sourceRectangle.Width/ (float)_texture.Width,
                sourceRectangle.Height / (float)_texture.Height);
        }
    }
}
