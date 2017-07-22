using System;
using System.Diagnostics;
using System.Linq;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Flai.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Flai.General
{
    public class Scroller
    {
        #region Fields and Properties

        private const float MaxVelocity = 4000;

        private readonly Alignment _scrollingAlignment;
        private Range _scrollingRange;
        private float _currentScrollValue;

        private float _velocity = 0;
        private float _velocityDampingSpeed = 10;

        private bool _allowVelocity = true;
        private float _velocityMultiplier = 1.5f;

        private bool _allowElasticity = true;
        private float _maxElasticity = 128;
        private float _elasticityPower = 15;

        public Range ScrollingRange
        {
            get { return _scrollingRange; }
            set
            {
                _scrollingRange = value;
                Range testRange = new Range(value.Min - _maxElasticity, value.Max + _maxElasticity);
                if (!testRange.Contains(_currentScrollValue))
                {
                    _currentScrollValue = FlaiMath.Clamp(_currentScrollValue, testRange);
                }
            }
        }

        public float ScrollValue
        {
            get { return _currentScrollValue; }
            set
            {
                Ensure.IsValid(value);
                Ensure.WithinRange(value, _scrollingRange);

                _currentScrollValue = value;
            }
        }

        public float VelocityDampingSpeed
        {
            get { return _velocityDampingSpeed; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value > 0);

                _velocityDampingSpeed = value;
            }
        }

        public bool AllowVelocity
        {
            get { return _allowVelocity; }
            set { _allowVelocity = value; }
        }

        public float VelocityMultiplier
        {
            get { return _velocityMultiplier; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value > 0);

                _velocityMultiplier = value;
            }
        }

        public bool AllowElasticity
        {
            get { return _allowElasticity; }
            set { _allowElasticity = value; }
        }

        public float MaxElasticity
        {
            get { return _maxElasticity; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value > 0);

                _maxElasticity = value;
            }
        }

        public float ElasticityPower
        {
            get { return _elasticityPower; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value > 0);

                _elasticityPower = value;
            }
        }

        #endregion

        public Scroller(Range scrollingRange, Alignment scrollingAlignment)
        {
            _scrollingRange = scrollingRange;
            _scrollingAlignment = scrollingAlignment;

            _currentScrollValue = _scrollingRange.Min;
        }

        public Matrix GetTransformMatrix(Size screenSize)
        {
            return Camera2D.CalculateTransform(screenSize, this.GetPosition(screenSize));
        }

        public RectangleF GetArea(Size screenSize)
        {
            return Camera2D.CalculateArea(screenSize, this.GetPosition(screenSize));
        }

        private Vector2 GetPosition(Size screenSize)
        {
            return (_scrollingAlignment == Alignment.Horizontal) ?
                new Vector2(screenSize.Width / 2f + _currentScrollValue, screenSize.Height / 2f) :
                new Vector2(screenSize.Width / 2f, screenSize.Height / 2f + _currentScrollValue);
        }

        public void Update(UpdateContext updateContext)
        {
            this.ApplyVelocity(updateContext);
            this.HandleInput(updateContext);
            this.HandleElasticity(updateContext);
        }

        private void HandleInput(UpdateContext updateContext)
        {
#if WINDOWS_PHONE
            // check if user touches/moves on the screen
            foreach (var touchLocation in updateContext.InputState.TouchLocations)
            {
                if (touchLocation.State == TouchLocationState.Moved || touchLocation.State == TouchLocationState.Pressed)
                {
                    _velocity = 0;
                    Flai.Input.TouchLocation previousLocation;
                    if (touchLocation.State == TouchLocationState.Moved && touchLocation.TryGetPreviousLocation(out previousLocation))
                    {
                        _currentScrollValue += (previousLocation.Position - touchLocation.Position).GetAxis(_scrollingAlignment);
                    }
                }
            }

            // check for flick gestures
            foreach (GestureSample gesture in updateContext.InputState.Gestures)
            {
                if (gesture.GestureType == GestureType.Flick)
                {
                    _velocity = FlaiMath.Clamp(-gesture.Delta.Y * this.VelocityMultiplier, -Scroller.MaxVelocity, Scroller.MaxVelocity);
                }
            }

#else
            throw new NotImplementedException();
#endif
        }

        private void HandleElasticity(UpdateContext updateContext)
        {
#if WINDOWS_PHONE
            // screen is *NOT* touched, so "apply" the elasticity if the scroll value is out of range
            if (updateContext.InputState.TouchLocations.Count == 0)
            {
                // not sure if using smoothstep is the best way to go but it works ok enough
                if (_currentScrollValue < _scrollingRange.Min)
                {
                    _currentScrollValue = FlaiMath.Min(_scrollingRange.Min, FlaiMath.SmoothStep(_currentScrollValue, _scrollingRange.Min, _elasticityPower * updateContext.DeltaSeconds));
                }
                else if (_currentScrollValue > _scrollingRange.Max)
                {
                    _currentScrollValue = FlaiMath.Max(_scrollingRange.Max, FlaiMath.SmoothStep(_currentScrollValue, _scrollingRange.Max, _elasticityPower * updateContext.DeltaSeconds));
                }
            }

            // make sure that the scroll value doesn't exceed the "max elasticity"
            _currentScrollValue = FlaiMath.Clamp(_currentScrollValue, _scrollingRange.Min - _maxElasticity, _scrollingRange.Max + _maxElasticity);
#else
            throw new NotImplementedException();
#endif
        }

        private void ApplyVelocity(UpdateContext updateContext)
        {
            if (!_allowVelocity || _velocity == 0f)
            {
                _velocity = 0;
                return;
            }

            Sign oldSign = FlaiMath.GetSign(_velocity);
            _velocity -= _velocity * _velocityDampingSpeed * updateContext.DeltaSeconds;
            if (FlaiMath.GetSign(_velocity) != oldSign)
            {
                _velocity = 0;
                return;
            }

            _currentScrollValue += _velocity * updateContext.DeltaSeconds; // ... these double delta seconds stuff always confuses me... but i think this is right
        }
    }
}
