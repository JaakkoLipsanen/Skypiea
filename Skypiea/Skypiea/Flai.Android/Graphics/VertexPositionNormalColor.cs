using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public struct VertexPositionNormalColor : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
        {
            this.Position = position;
            this.Normal = normal;
            this.Color = color;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
			new VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0),
		})
        {
            Name = "VertexPositionNormalColor.VertexDeclaration"
        };

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionNormalColor.VertexDeclaration; }
        }
    }
}
