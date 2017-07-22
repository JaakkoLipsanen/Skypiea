using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public struct VertexPositionNormalColorTexture : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;
        public Vector2 TextureCoordinate;

        public VertexPositionNormalColorTexture(Vector3 position, Vector3 normal, Color color, Vector2 textureCoordinate)
        {
            this.Position = position;
            this.Normal = normal;
            this.Color = color;
            this.TextureCoordinate = textureCoordinate;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
			new VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0),
			new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
		})
        {
            Name = "VertexPositionNormalColorTexture.VertexDeclaration"
        };

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionNormalColorTexture.VertexDeclaration; }
        }
    }
}
