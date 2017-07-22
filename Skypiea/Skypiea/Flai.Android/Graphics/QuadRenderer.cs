
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public static class QuadRenderer
    {
        private static readonly VertexPositionTexture[] _fullscreenVertices;
        private static readonly VertexPositionTexture[] _vertices;
        private static readonly short[] _indices;

        public static readonly VertexPositionTexture[] FullscreenQuadVertices;
        public static readonly short[] QuadIndices;

        static QuadRenderer()
        {
            _vertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,0)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,0))
            };

            _fullscreenVertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(
                    new Vector3(-1, -1, 0),
                    new Vector2(0, 1)),

                new VertexPositionTexture(
                    new Vector3(-1, 1, 0),
                    new Vector2(0, 0)),

                new VertexPositionTexture(
                    new Vector3(1, 1, 0),
                    new Vector2(1, 0)),

                new VertexPositionTexture(
                    new Vector3(1, -1, 0),
                    new Vector2(1, 1)),
            };

            _indices = new short[] { 0, 1, 2, 2, 3, 0 };

            QuadRenderer.FullscreenQuadVertices = _fullscreenVertices;
            QuadRenderer.QuadIndices = _indices;
        }

        public static void Render(GraphicsContext graphicsContext, Vector2 v1, Vector2 v2)
        {
            _vertices[0].Position.X = v2.X;
            _vertices[0].Position.Y = v1.Y;

            _vertices[1].Position.X = v1.X;
            _vertices[1].Position.Y = v1.Y;

            _vertices[2].Position.X = v1.X;
            _vertices[2].Position.Y = v2.Y;

            _vertices[3].Position.X = v2.X;
            _vertices[3].Position.Y = v2.Y;

            graphicsContext.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                _vertices,
                0, 4, _indices, 0, 2);
        }

        public static void RenderFullscreen(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
               PrimitiveType.TriangleList,
               _fullscreenVertices,
               0, 4, _indices, 0, 2);
        }
    }
}
