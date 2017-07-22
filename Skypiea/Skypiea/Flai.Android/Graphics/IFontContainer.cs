
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public interface IFontContainer
    {
        SpriteFont DefaultFont { get; }

        void AddFont(string fontName);
        void AddFont(string fontName, SpriteFont font);

        bool RemoveFont(string fontName);
        SpriteFont GetFont(string fontName);
        bool ContainsFont(string fontName);
               
        SpriteFont this[string fontName] { get; set; }       
    }
}
