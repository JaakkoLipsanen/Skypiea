using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public static class GraphicsHelper
    {
        // Collision stuff to CollisionHelper? 
        public static bool PixelPerfectCollision(
            Color[] firstTextureData, Rectangle firstTextureArea,
            Color[] secondTextureData, Rectangle secondTextureArea)
        {
            return GraphicsHelper.PixelPerfectCollision(
                firstTextureData, firstTextureArea, secondTextureData, secondTextureArea, 1, 0);
        }

        public static bool PixelPerfectCollision(
            Color[] firstTextureData, Rectangle firstTextureArea,
            Color[] secondTextureData, Rectangle secondTextureArea, int step)
        {
            return GraphicsHelper.PixelPerfectCollision(
                firstTextureData, firstTextureArea, secondTextureData, secondTextureArea, step, 0);
        }

        public static bool PixelPerfectCollision(
            Color[] firstTextureData, Rectangle firstTextureArea,
            Color[] secondTextureData, Rectangle secondTextureArea, int step, byte alphaThreshold)
        {
            Ensure.True(step >= 0);
            if (!firstTextureArea.Intersects(secondTextureArea))
            {
                return false;
            }

            int xStart = Math.Max(firstTextureArea.Left, secondTextureArea.Left);
            int xEnd = Math.Min(firstTextureArea.Right, secondTextureArea.Right);

            int yStart = Math.Max(firstTextureArea.Top, secondTextureArea.Top);
            int yEnd = Math.Min(firstTextureArea.Bottom, secondTextureArea.Bottom);

            for (int y = yStart; y < yEnd; y += step)
            {
                int firstTempY = (y - firstTextureArea.Top) * firstTextureArea.Width;
                int secondTempY = (y - secondTextureArea.Top) * secondTextureArea.Width;

                for (int x = xStart; x < xEnd; x += step)
                {
                    // byte firstTexPixelAlpha = firstTextureData[(x - firstTextureArea.Left) + firstTempY].A;
                    // byte secondTexPixelAlpha = secondTextureData[(x - secondTextureArea.Left) + secondTempY].A;

                    if (firstTextureData[(x - firstTextureArea.Left) + firstTempY].A > alphaThreshold &&
                        secondTextureData[(x - secondTextureArea.Left) + secondTempY].A > alphaThreshold)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // PRETTY SURE THESE ARE NOT WORKING 100% YET
        public static bool TransformedPixelPerfectCollision(
            Color[] pixelDataA, int widthA, int heightA, Vector2 positionA, float rotationA,
            Color[] pixelDataB, int widthB, int heightB, Vector2 positionB, float rotationB)
        {
            return GraphicsHelper.TransformedPixelPerfectCollision(
                pixelDataA, widthA, heightA, positionA, rotationA, 1f,
                pixelDataB, widthB, heightB, positionB, rotationB, 1f);
        }

        public static bool TransformedPixelPerfectCollision(
            Color[] pixelDataA, int widthA, int heightA, Vector2 positionA, float rotationA, float scaleA,
            Color[] pixelDataB, int widthB, int heightB, Vector2 positionB, float rotationB, float scaleB)
        {
            // If the texture isn't rotated or scaled, use normal pixel perfect collision
            if (rotationA == 0f && rotationB == 0f && scaleA == 1f && scaleB == 1f)
            {
                return GraphicsHelper.PixelPerfectCollision(
                    pixelDataA, new Rectangle((int)positionA.X, (int)positionA.Y, widthA, heightA),
                    pixelDataB, new Rectangle((int)positionB.X, (int)positionB.Y, widthB, heightB));
            }

            Matrix transformA =
                Matrix.CreateTranslation(new Vector3(-widthA / 2, -heightA / 2, 0)) *
                Matrix.CreateScale(scaleA) *
                Matrix.CreateRotationZ(rotationA) *
                Matrix.CreateTranslation(new Vector3(positionA, 0));

            Matrix transformB =
                Matrix.CreateTranslation(new Vector3(-widthB / 2, -heightB / 2, 0)) *
                Matrix.CreateScale(scaleB) *
                Matrix.CreateRotationZ(rotationB) *
                Matrix.CreateTranslation(new Vector3(positionB, 0));

            return GraphicsHelper.TransformedPixelPerfectCollision(
                pixelDataA, widthA, heightA, transformA,
                pixelDataB, widthB, heightB, transformB);
        }

        public static bool TransformedPixelPerfectCollision(
           Color[] pixelDataA, int widthA, int heightA, Matrix transformA,
           Color[] pixelDataB, int widthB, int heightB, Matrix transformB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    if (pixelDataA[xA + yA * widthA].A != 0)
                    {
                        // Round to the nearest pixel
                        int xB = (int)Math.Round(posInB.X);
                        int yB = (int)Math.Round(posInB.Y);

                        // If the pixel lies within the bounds of B
                        if (0 <= xB && xB < widthB &&
                            0 <= yB && yB < heightB)
                        {
                            // If both pixels are not completely transparent,
                            if (pixelDataB[xB + yB * widthB].A != 0)
                            {
                                // then an intersection has been found
                                return true;
                            }
                        }
                    }
                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        public static void FadeBackBufferToColor(GraphicsContext graphicsContext, Color color)
        {
            GraphicsHelper.FadeBackBufferToColor(graphicsContext, color, false);
        }

        public static void FadeBackBufferToColor(GraphicsContext graphicsContext, Color color, bool isSpritebatchOn)
        {
            if (color == Color.Transparent)
            {
                return;
            }

            if (!isSpritebatchOn)
            {
                graphicsContext.SpriteBatch.Begin();
            }

            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.BlankTexture, color);

            if (!isSpritebatchOn)
            {
                graphicsContext.SpriteBatch.End();
            }
        }

        // These would be a lot faster to do in GPU
        #region Pixelize

        // Doesn't actually downscale. Downscaling could be faster
        public static Texture2D Pixelize(Texture2D texture, int pixelSize, PixelizeFunction function)
        {
            return GraphicsHelper.Pixelize(texture, new Vector2i(texture.Width / pixelSize, texture.Height / pixelSize), function);
        }

        public static Texture2D Pixelize(Texture2D texture, Vector2i pixels, PixelizeFunction function)
        {
            Ensure.False(pixels.X < 0 || pixels.Y < 0 || texture.Width < pixels.X || texture.Height < pixels.Y);

            if (texture.Width == pixels.X && texture.Height == pixels.Y)
            {
                return texture;
            }

            Color[] pixelDataSource = texture.GetData<Color>();
            Color[] pixelDataOutput;

            if (function == PixelizeFunction.Average)
            {
                GraphicsHelper.PixelizeAverage(texture, pixelDataSource, pixels, out pixelDataOutput);
            }
            else if (function == PixelizeFunction.Center)
            {
                GraphicsHelper.PixelizeCenter(texture, pixelDataSource, pixels, out pixelDataOutput);
            }
            else
            {
                throw new ArgumentException("function");
            }

            Texture2D result = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            result.SetData(pixelDataOutput);

            return result;
        }

        private static void PixelizeCenter(Texture2D texture, Color[] pixelDataSource, Vector2i pixels, out Color[] pixelDataOutput)
        {
            pixelDataOutput = new Color[texture.Width * texture.Height];
            Vector2i offset = new Vector2i(texture.Width, texture.Height) / pixels;

            for (int y = 0; y < pixels.Y; y++)
            {
                int tempIndex = (y * offset.Y + offset.Y / 2) * texture.Width + offset.X / 2;
                for (int x = 0; x < pixels.X; x++)
                {
                    Color sourcePixelColor = pixelDataSource[x * offset.X + tempIndex]; // (x * offset.X + offset.X / 2) + (y * offset.Y + offset.Y / 2) * texture.Width
                    int maxY = (y + 1) * offset.Y;
                    for (int y2 = y * offset.Y; y2 < maxY; y2++)
                    {
                        int y2Cached = y2 * texture.Width;

                        int maxX = (x + 1) * offset.X;
                        for (int x2 = x * offset.X; x2 < maxX; x2++)
                        {
                            pixelDataOutput[x2 + y2Cached] = sourcePixelColor;
                        }
                    }
                }
            }
        }

        private static void PixelizeAverage(Texture2D texture, Color[] pixelDataSource, Vector2i pixels, out Color[] pixelDataOutput)
        {
            pixelDataOutput = new Color[texture.Width * texture.Height];
            Vector2i offset = new Vector2i(texture.Width, texture.Height) / pixels;

            for (int y = 0; y < pixels.Y; y++)
            {
                for (int x = 0; x < pixels.X; x++)
                {
                    int R = 0;
                    int G = 0;
                    int A = 0;
                    int B = 0;

                    int count = 0;

                    int maxY = (y + 1) * offset.Y;
                    for (int y2 = y * offset.Y; y2 < maxY; y2++)
                    {
                        int y2Cached = y2 * texture.Width;

                        int maxX = (x + 1) * offset.X;
                        for (int x2 = x * offset.X; x2 < maxX; x2++)
                        {
                            Color color = pixelDataSource[x2 + y2Cached];
                            R += color.R;
                            G += color.G;
                            B += color.B;
                            A += color.A;

                            count++;
                        }
                    }

                    Color pixelColor = new Color(R / count, G / count, B / count, A / count);
                    for (int y2 = y * offset.Y; y2 < maxY; y2++)
                    {
                        int y2Cached = y2 * texture.Width;

                        int maxX = (x + 1) * offset.X;
                        for (int x2 = x * offset.X; x2 < maxX; x2++)
                        {
                            pixelDataOutput[x2 + y2Cached] = pixelColor;
                        }
                    }
                }
            }
        }

        #endregion

        public static RectangleF SourceRectangleToUvRectangle(Texture2D texture, Rectangle sourceRectangle)
        {
            return new RectangleF(sourceRectangle.Left / (float)texture.Width, sourceRectangle.Top / (float)texture.Height, sourceRectangle.Width / (float)texture.Width, sourceRectangle.Height / (float)texture.Height);
        }

        public static Rectangle? CombineSourceRectangle(Rectangle? main, Rectangle? secondary)
        {
            if (main.HasValue)
            {
                if (secondary.HasValue)
                {
                    Rectangle value = secondary.Value;
                    value.Offset(new Vector2i(main.Value.X, main.Value.Y));
                    return value;
                }
                else
                {
                    return main.Value;
                }
            }

            return secondary;
        }

        public static int GetPrimitiveCount(int vertices, PrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                    Ensure.True(vertices % 3 == 0);
                    return vertices / 3;

                default:
                    throw new NotImplementedException("");
            }
        }
    }

    // Shitty name
    public enum PixelizeFunction
    {
        Average,
        Center,
    }
}
