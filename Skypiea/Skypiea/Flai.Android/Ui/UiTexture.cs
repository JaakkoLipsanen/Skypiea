using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Ui
{
    public class UiTexture : UiObject
    {
        private readonly Texture2D _texture;
        private Color _tint = Color.White;
        private Vector2 _scale = Vector2.One;
        private float _rotation = 0f;

        public Color Tint
        {
            get { return _tint; }
            set { _tint = value; }
        }

        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public UiTexture(Texture2D texture, Vector2 position)
            : this(texture, new RectangleF(position.X - texture.Width / 2f, position.Y - texture.Height / 2f, texture.Width, texture.Height))
        {
        }

        public UiTexture(Texture2D texture, RectangleF area)
            : base(area)
        {
            _texture = texture;
            _scale = new Vector2(area.Width / (float)_texture.Width, area.Height / (float)_texture.Height);
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Draw(_texture, _area.Center, Color.White, _rotation, _scale);
        }
    }
}
