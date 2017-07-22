using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public struct VertexPosition : IVertexType
    {
        public Vector3 Position;

        public VertexPosition(Vector3 position)
        {
            this.Position = position;
        }

        private static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0));

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPosition.VertexDeclaration; }
        }     
    }
}
