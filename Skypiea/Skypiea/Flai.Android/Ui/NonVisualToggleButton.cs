
using Microsoft.Xna.Framework;
namespace Flai.Ui
{
    //
    public class NonVisualToggleButton : ToggleButtonBase
    {
        public NonVisualToggleButton(Vector2 centerPosition, Vector2i size, bool isToggled)
            : this(new RectangleF(centerPosition.X - size.X / 2, centerPosition.Y - size.Y / 2, size.X, size.Y), isToggled)
        {
        }

        public NonVisualToggleButton(RectangleF area, bool isToggled)
            : base(area, isToggled)
        {
            base.Visible = false;         
        }
    }
}
