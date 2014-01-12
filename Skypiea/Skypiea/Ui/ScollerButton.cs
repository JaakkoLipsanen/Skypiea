using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Skypiea.Ui
{
    public class ScrollerButton : TextButton
    {
        private readonly Scroller _scroller;
        private float _realVerticalPosition;

        public ScrollerButton(string text, Vector2 position, Scroller scroller, GenericEvent clickFunction)
            : base(text, position, clickFunction)
        {
            _realVerticalPosition = position.Y;
            _scroller = scroller;
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
            this.SetVerticalOffset(_scroller.ScrollValue);
        }

        public void SetVerticalPosition(float value)
        {
            _realVerticalPosition = value;
        }

        private void SetVerticalOffset(float verticalOffset)
        {
            _centerPosition.Y = _realVerticalPosition - verticalOffset;
            this.UpdateArea();
        }
    }

    public class ScrollerTextureButton : TexturedButton
    {
        private readonly Scroller _scroller;
        private float _realVerticalPosition;

        public ScrollerTextureButton(Sprite sprite, Vector2 position, Scroller scroller, GenericEvent clickFunction)
            : base(sprite, position, clickFunction)
        {
            _realVerticalPosition = position.Y;
            _scroller = scroller;
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
            this.SetVerticalOffset(_scroller.ScrollValue);
        }

        public void SetVerticalPosition(float value)
        {
            _realVerticalPosition = value;
        }

        private void SetVerticalOffset(float verticalOffset)
        {
            _area.Center = _visualArea.Center = new Vector2(_area.Center.X, _realVerticalPosition - verticalOffset);
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
                graphicsContext.PrimitiveRenderer.DrawRectangle(_area, Color.Black * 0.15f);
                graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(_area, Color.Black * 0.5f, 1f);
            }

            base.Draw(graphicsContext);            
        }

        private void SetVerticalOffset(float verticalOffset)
        {
            _area.Y = _initialVerticalPosition - verticalOffset;
        }
    }
}
