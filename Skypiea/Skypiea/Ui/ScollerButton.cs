using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Ui;
using Microsoft.Xna.Framework;

namespace Skypiea.Ui
{
    public class ScrollerButton : TextButton
    {
        private readonly float _initialVerticalPosition;
        private readonly Scroller _scroller;
        public ScrollerButton(string text, Vector2 position, Scroller scroller, GenericEvent clickFunction)
            : base(text, position, clickFunction)
        {
            _initialVerticalPosition = position.Y;
            _scroller = scroller;
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
            this.SetVerticalOffset(_scroller.ScrollValue);
        }

        private void SetVerticalOffset(float verticalOffset)
        {
            _centerPosition.Y = _initialVerticalPosition - verticalOffset;
            this.UpdateArea();
        }
    }

    public class ScrollerToggleButton : TextToggleButton
    {
        private readonly float _initialVerticalPosition;
        private readonly Scroller _scroller;

        public bool DrawOutlines { get; set; }

        public ScrollerToggleButton(RectangleF area, string toggledText, string notToggledText, Scroller scroller)
            : base(area, toggledText, notToggledText)
        {
            _initialVerticalPosition = area.Y;
            _scroller = scroller;
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
            this.SetVerticalOffset(_scroller.ScrollValue);
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            if (this.DrawOutlines)
            {
                RectangleF area = _area;
                if (this.IsTouchedDown)
                {
                 //   area.Inflate(-area.Width * 0.05f, -area.Height * 0.05f);
                }

                graphicsContext.PrimitiveRenderer.DrawRectangle(area, Color.Black * 0.15f);
                graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(area, Color.Black * 0.5f, 1f);
            }

            base.Draw(graphicsContext);            
        }

        private void SetVerticalOffset(float verticalOffset)
        {
            _area.Y = _initialVerticalPosition - verticalOffset;
        }
    }
}
