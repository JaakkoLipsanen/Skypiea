
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public interface ITileSpriteSheet
    {
        Texture2D Texture { get; }
        Size SheetSize { get; }
        
        Rectangle GetSourceRectangle(Vector2i frame);
        Rectangle GetSourceRectangle(int x, int y);
    }
}
