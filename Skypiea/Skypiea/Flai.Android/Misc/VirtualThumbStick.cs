
#if WINDOWS_PHONE
using Flai.Input;
using Microsoft.Xna.Framework;

namespace Flai.Misc
{
    public enum ThumbstickStyle
    {
        Fixed,
        Relative,
    }

    // TODO: Dead zone (= min length)
    // fixed position for now
    public class VirtualThumbstick // todo: IVirtualThumbStick?
    {
        private const float DefaultRadius = 100f;
        private readonly VirtualThumbstickImplementation _implementation;

        public Vector2? CenterPosition
        {
            get { return _implementation.CenterPosition; }
        }

        // normalized direction, [-1, 1]
        public Vector2? Direction
        {
            get { return _implementation.Direction; }
        }

        // [0, ]1: 0 = no smoothing, 0.999... = basically full smoothing
        public float SmoothingPower
        {
            get { return _implementation.SmoothingPower; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 0);
                Ensure.True(value < 1);

                _implementation.SmoothingPower = value;
            }
        }

        public float Radius
        {
            get { return _implementation.Radius; }
        }

        public ThumbstickStyle Style
        {
            get { return _implementation.Style; }
        }

        public bool IsPressed
        {
            get { return _implementation.IsPressed; }
        }

        private VirtualThumbstick(VirtualThumbstickImplementation implementation)
        {
            _implementation = implementation;
        }

        public void Update(UpdateContext updateContext)
        {
            _implementation.Update(updateContext);
        }

        public static VirtualThumbstick CreateFixed(Vector2 centerPosition)
        {
            return VirtualThumbstick.CreateFixed(centerPosition, VirtualThumbstick.DefaultRadius);
        }

        public static VirtualThumbstick CreateFixed(Vector2 centerPosition, float thumbstickRadius)
        {
            return new VirtualThumbstick(new FixedVirtualThumbstickImplementation { CenterPosition = centerPosition, Radius = thumbstickRadius });
        }

        public static VirtualThumbstick CreateRelative(RectangleF touchArea)
        {
            return VirtualThumbstick.CreateRelative(touchArea, VirtualThumbstick.DefaultRadius);
        }

        public static VirtualThumbstick CreateRelative(RectangleF touchArea, float thumbstickRadius)
        {
            return new VirtualThumbstick(new RelativeVirtualThumbstickImplementation { TouchArea = touchArea, Radius = thumbstickRadius });
        }

        #region Implementations

        private abstract class VirtualThumbstickImplementation
        {
            protected Vector2? _touchLocation;
            protected int _touchID = -1;

            public Vector2? CenterPosition { get; set; }
            public Vector2? Direction { get; protected set; }
            public float Radius { get; set; }
            public float SmoothingPower { get; set; }

            public abstract ThumbstickStyle Style { get; }

            public virtual bool IsPressed
            {
                get { return _touchID != -1 && this.Direction.HasValue && this.CenterPosition.HasValue; }
            }

            public void Update(UpdateContext updateContext)
            {
                TouchLocation? location = this.FindTouchLocation(updateContext.InputState.TouchLocations);
                if (location.HasValue)
                {
                    _touchLocation = location.Value.Position;
                    _touchID = location.Value.Id;
                }
                else
                {
                    _touchLocation = null;
                    _touchID = -1;
                }

                this.UpdateInner(updateContext);
            }

            private TouchLocation? FindTouchLocation(TouchCollection touches)
            {
                foreach (TouchLocation touchLocation in touches)
                {
                    if (_touchID != -1)
                    {
                        if (touchLocation.Id == _touchID)
                        {
                            return touchLocation;
                        }
                    }
                    else
                    {
                        if (this.AcceptTouchLocation(touchLocation))
                        {
                            return touchLocation;
                        }
                    }
                }

                return null;
            }

            protected abstract void UpdateInner(UpdateContext updateContext);
            protected abstract bool AcceptTouchLocation(TouchLocation location);
        }

        private class FixedVirtualThumbstickImplementation : VirtualThumbstickImplementation
        {
            public override ThumbstickStyle Style
            {
                get { return ThumbstickStyle.Fixed; }
            }

            public override bool IsPressed
            {
                // okay this is not completely true. direction can be Zero even if pressed, but it is extremele unlikely
                get { return this.Direction != Vector2.Zero; }
            }

            protected override void UpdateInner(UpdateContext updateContext)
            {
                this.Direction = this.CalculateDirection();
            }

            private Vector2 CalculateDirection()
            {
                if (_touchID == -1 || !_touchLocation.HasValue || !this.CenterPosition.HasValue)
                {
                    return Vector2.Zero;
                }

                Vector2 direction = (_touchLocation.Value - this.CenterPosition.Value) / this.Radius;
                if (direction.Length() > 1f)
                {
                    direction = direction.NormalizeOrZero();
                }

                return direction;
            }

            protected override bool AcceptTouchLocation(TouchLocation touchLocation)
            {
                TouchLocation previousLocation;
                if (!touchLocation.TryGetPreviousLocation(out previousLocation))
                {
                    previousLocation = touchLocation;
                }

                // todo: InputRadiusBias to parameter when creating the thumbstick? not sure if it is always wanted (and also the amount will vary)
                const float InputRadiusBias = 32;
                if (Vector2.Distance(previousLocation.Position, this.CenterPosition.Value) < this.Radius + InputRadiusBias)
                {
                    return true;
                }

                return false;
            }
        }

        private class RelativeVirtualThumbstickImplementation : VirtualThumbstickImplementation
        {
            public RectangleF TouchArea { get; set; }
            public override ThumbstickStyle Style
            {
                get { return ThumbstickStyle.Relative; }
            }

            protected override void UpdateInner(UpdateContext updateContext)
            {
                if (this.CenterPosition.HasValue)
                {
                    if (_touchLocation.HasValue)
                    {
                        this.Direction = this.CalculateDirection();
                    }
                    else
                    {
                        this.CenterPosition = null;
                        this.Direction = null;
                    }
                }
                else
                {
                    if (_touchLocation.HasValue && this.TouchArea.Contains(_touchLocation.Value))
                    {
                        this.CenterPosition = _touchLocation.Value;
                    }
                }
            }

            private Vector2 CalculateDirection()
            {
                if (_touchID == -1)
                {
                    return Vector2.Zero;
                }

                Vector2 direction = (_touchLocation.Value - this.CenterPosition.Value) / this.Radius;
                if (direction.Length() > 1f)
                {
                    direction = direction.NormalizeOrZero();
                }

                return direction;
            }

            protected override bool AcceptTouchLocation(TouchLocation touchLocation)
            {
                if(touchLocation.State != Microsoft.Xna.Framework.Input.Touch.TouchLocationState.Pressed)
                {
                    return false;
                }

                TouchLocation previousLocation;
                if (!touchLocation.TryGetPreviousLocation(out previousLocation))
                {
                    previousLocation = touchLocation;
                }

                if (this.TouchArea.Contains(previousLocation.Position))
                {
                    return true;
                }

                return false;
            }
        }

        #endregion
    }
}
#endif