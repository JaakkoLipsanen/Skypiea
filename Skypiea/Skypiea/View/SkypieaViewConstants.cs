using Flai.Content;
using Flai.Graphics;
using Microsoft.Xna.Framework;

namespace Skypiea.View
{
    public static class SkypieaViewConstants
    {
        public static readonly Color ClearColor = Color.Black;

        public const int PixelSize = 4;
        public const int FadeLength = 20 * SkypieaViewConstants.PixelSize;
        public const float DropArrowAlpha = 0.75f;
        public const string SpriteSheetName = "TextureSpriteSheet";

        public static TextureDefinition LoadTexture(IContentProvider contentProvider, string textureName)
        {
            return contentProvider.DefaultManager.LoadTextureFromSpriteSheet(SkypieaViewConstants.SpriteSheetName, textureName);
        }
    }
}
