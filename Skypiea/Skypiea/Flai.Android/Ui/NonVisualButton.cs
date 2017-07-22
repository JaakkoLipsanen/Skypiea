
namespace Flai.Ui
{
    //
    public class NonVisualButton : ButtonBase
    {
        public NonVisualButton(RectangleF area)
            : base(area)
        {
            base.Visible = false;
        }
    }
}
