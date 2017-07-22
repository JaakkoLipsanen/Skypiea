using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public interface ISpriteSheet
    {
        ReadOnlyDictionary<string, Rectangle> Sprites { get; }
        Texture2D Texture { get; }

        bool ContainsSprite(string name);
        Rectangle GetSourceRectangle(string spriteName);
        RectangleF GetUvCoordinates(string spriteName);  
    }
}
