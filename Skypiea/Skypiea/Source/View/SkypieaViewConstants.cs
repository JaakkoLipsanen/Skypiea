using Flai;
using Flai.Content;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Skypiea.View
{
    public static class SkypieaViewConstants
    {
        public static readonly Color ClearColor = Color.Black;

        public static Vector2 RenderScale
        {
            get
            {
                // the original WP version was coded to run at 800x480, so lets just scale everything to look same
                return new Vector2(
                    FlaiGame.Current.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)FlaiGame.Current.ScreenSize.Width, 
                    FlaiGame.Current.GraphicsDevice.PresentationParameters.BackBufferHeight / (float)FlaiGame.Current.ScreenSize.Height);
            }
        }

        public static Matrix RenderScaleMatrix
        {
            get { return Matrix.CreateScale(SkypieaViewConstants.RenderScale.X, SkypieaViewConstants.RenderScale.Y, 1); }
        }

        public const int PixelSize = 4;
        public const int FadeLength = 20 * SkypieaViewConstants.PixelSize;
        public const float DropArrowAlpha = 0.75f;
        public const string SpriteSheetName = "TextureSpriteSheet";

        // not a constant but whatever. fits here
        public static TextureDefinition LoadTexture(IContentProvider contentProvider, string textureName)
        { 
            return new TextureDefinition(contentProvider.DefaultManager.LoadTexture(SkypieaViewConstants.SpriteSheetName), SkypieaViewConstants.GetTextureBounds(textureName));
         //   return contentProvider.DefaultManager.LoadTextureFromSpriteSheet(SkypieaViewConstants.SpriteSheetName, textureName);
        }

        // Didn't manage to get Spritesheet Content Pipeline extension working in Android, so fuck it I'll just hard code the bounds :D
        private static Rectangle GetTextureBounds(string name)
        {
            switch (name)
            {
                case "BoosterTextBackground": return new Rectangle(16, 1736, 347, 120);
                case "Bullet": return new Rectangle(1912, 476, 8, 5);
                case "CornerFadeTexture": return new Rectangle(1912, 370, 20, 20);
                case "Heart": return new Rectangle(1912, 414, 11, 10);
                case "Laser": return new Rectangle(1956, 365, 14, 200);
                case "Life": return new Rectangle(1956, 326, 15, 15);
                case "LifeDropFan": return new Rectangle(1912, 16, 98, 199);

                case "Map1": return new Rectangle(16, 1144, 512, 288);
                case "Map2": return new Rectangle(1352, 640, 512, 288);
                case "Map3": return new Rectangle(16, 832, 512, 288);
                case "Map4": return new Rectangle(816, 640, 512, 288);
                case "Map5": return new Rectangle(816, 640, 512, 288);
                case "Map6": return new Rectangle(1386, 328, 512, 288);
                case "Map7": return new Rectangle(840, 328, 512, 288);
                case "Map8": return new Rectangle(1376, 16, 512, 288);
                case "Map9": return new Rectangle(840, 16, 512, 288);

                case "MenuBackground": return new Rectangle(552, 784, 200, 120);
                case "Noise": return new Rectangle(16, 1456, 512, 256);
                case "PauseButton": return new Rectangle(1999, 239, 23, 33);
                case "RicochetBullet": return new Rectangle(1912, 326, 20, 20);
                case "RocketBullet": return new Rectangle(1912, 448, 9, 4);
                case "SideFadeTexture": return new Rectangle(1999, 296, 20, 512);
                case "ThumbstickBase": return new Rectangle(552, 520, 240, 240);
                case "Vignette": return new Rectangle(18, 18, 800, 480);
                case "Zombie": return new Rectangle(1912, 239, 63, 63);

                default: throw new NotImplementedException();
            }
        }
    }
}
