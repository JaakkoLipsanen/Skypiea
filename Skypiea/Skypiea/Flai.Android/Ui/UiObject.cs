using System;
using Flai.Graphics;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#elif WINDOWS
using Flai.Input;
#endif

namespace Flai.Ui
{
    public abstract class UiObject
    {
        private readonly BasicUiContainer _children = new BasicUiContainer();

        private bool _enabled = true;
        private bool _visible = true;

#if WINDOWS
        private bool _isPressedDown = false;
        private bool _isHoveredOver = false;
#elif WINDOWS_PHONE
        private bool _isTouchedDown = false;
#endif

        protected RectangleF _area = RectangleF.Empty;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public BasicUiContainer Children
        {
            get { return _children; }
        }

#if WINDOWS
        public bool IsPressedDown
        {
            get { return _isPressedDown; }
        }

        public bool IsHoveredOver
        {
            get { return _isHoveredOver; }
        }
#elif WINDOWS_PHONE
        public bool IsTouchedDown
        {
            get { return _isTouchedDown; }
        }
#endif

        public RectangleF Area
        {
            get { return _area; }
        }

        public object Tag { get; set; }
        public string ID { get; set; }

#if WINDOWS
        public EventHandler<EventArgs> Press;
#elif WINDOWS_PHONE
        public EventHandler<EventArgs> TouchDown;
#endif

        protected UiObject()
        {
        }

        protected UiObject(RectangleF area)
        {
            _area = area;
        }

        public virtual void Update(UpdateContext updateContext)
        {
#if WINDOWS
            if (_area.Contains(updateContext.InputState.MousePosition))
            {
                _isHoveredOver = true;
                if (updateContext.InputState.IsMouseButtonPressed(MouseButton.Left))
                {
                    _isPressedDown = true;
                    this.OnPressedDownInner();
                }
                else
                {
                    _isPressedDown = false;
                }
            }
            else
            {
                _isHoveredOver = false;
                _isPressedDown = false;
            }
#elif WINDOWS_PHONE
            Flai.Input.TouchLocation touchLocation;
            if (updateContext.InputState.IsTouchAt(this.Area.ToRectangle(), out touchLocation))
            {
                _isTouchedDown = true;
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    this.OnTouchDown();
                }
            }
            else
            {
                _isTouchedDown = false;
            }
#endif
        }

        public virtual void Draw(GraphicsContext graphicsContext) { }

#if WINDOWS
        private void OnPressedDownInner()
        {
            this.OnPressedDown();

            var handler = this.Press;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnPressedDown()
        {
        }
#elif WINDOWS_PHONE
        private void OnTouchDownInner()
        {
            this.OnTouchDown();

            var handler = this.TouchDown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnTouchDown()
        {
        }
#endif
    }
}
