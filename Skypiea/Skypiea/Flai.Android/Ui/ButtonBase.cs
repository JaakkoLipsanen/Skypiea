
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#elif WINDOWS
using Flai.Input;
#endif

namespace Flai.Ui
{
    public abstract class ButtonBase : UiObject
    {
        public event GenericEvent Click;

        protected ButtonBase()
        {
        }

        protected ButtonBase(RectangleF area)
            : base(area)
        {
        }

        public override void Update(UpdateContext updateContext)
        {
            base.Update(updateContext);
#if WINDOWS
            if (_area.Contains(updateContext.InputState.MousePosition) && updateContext.InputState.WasMouseButtonPressed(MouseButton.Left) && updateContext.InputState.IsMouseButtonReleased(MouseButton.Left))
            {
                this.OnClickInner();
            }
#elif WINDOWS_PHONE
            bool isTapped = false;

            // Use Tap-gesture if it is enabled
            if (TouchPanel.EnabledGestures.ContainsFlag(GestureType.Tap))
            {
                isTapped = updateContext.InputState.HasGesture(GestureType.Tap, base.Area.ToRectangle());
            }
            else if (updateContext.InputState.IsTouchAt(base.Area.ToRectangle(), TouchLocationState.Released))
            {
                isTapped = true;
            }

            if (isTapped)
            {
                this.OnClickInner();
            }
#endif
        }

        public void ManualClick()
        {
            this.OnClick();
        }

        private void OnClickInner()
        {
            this.OnClick();
            this.Click.InvokeIfNotNull();
        }

        protected virtual void OnClick()
        {
        }
    }
}
