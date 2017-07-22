using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Flai.DataStructures;
using Flai.Graphics;
using System.Collections;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;

using XnaTouchCollection = Microsoft.Xna.Framework.Input.Touch.TouchCollection;
using XnaTouchLocation = Microsoft.Xna.Framework.Input.Touch.TouchLocation;
#endif

#region Windows Version
#if WINDOWS

namespace Flai.Input
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle,

        X1,
        X2,
    }

    public sealed class InputState
    {
#region Fields and Properties

        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _mouseState;
        private MouseState _previousMouseState;

        private Vector2i _mouseOffset = Vector2i.Zero;

        public KeyboardState KeyboardState
        {
            get { return _keyboardState; }
        }

        public KeyboardState PreviousKeyboardState
        {
            get { return _previousKeyboardState; }
        }

        public MouseState MouseState
        {
            get { return _mouseState; }
        }

        public MouseState PreviousMouseState
        {
            get { return _previousMouseState; }
        }

        public Vector2i MousePosition
        {
            get { return new Vector2i(this.MouseState.X - _mouseOffset.X, this.MouseState.Y - _mouseOffset.Y); }
        }

        public Vector2i PreviousMousePosition
        {
            get { return new Vector2i(this.PreviousMouseState.X - _mouseOffset.X, this.PreviousMouseState.Y - _mouseOffset.Y); }
        }

        public Vector2 MousePositionDelta
        {
            get { return this.MousePosition - this.PreviousMousePosition; }
        }

        public int ScrollWheelDelta
        {
            get { return this.MouseState.ScrollWheelValue - this.PreviousMouseState.ScrollWheelValue; }
        }

        public Vector2i MouseOffset
        {
            get { return _mouseOffset; }
            set { _mouseOffset = value; }
        }

#if EDITOR
        publ
#endif

#endregion

        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        internal InputState()
        {
            _keyboardState = _previousKeyboardState = Keyboard.GetState();
            _mouseState = _previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates the InputState
        /// </summary>
        internal void Update()
        {
            _previousKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _previousMouseState = _mouseState;
            _mouseState = Mouse.GetState();
        }

#region Mouse

        public ButtonState GetMouseButtonState(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.MouseState.LeftButton;

                case MouseButton.Right:
                    return this.MouseState.RightButton;

                case MouseButton.Middle:
                    return this.MouseState.MiddleButton;

                case MouseButton.X1:
                    return this.MouseState.XButton1;

                case MouseButton.X2:
                    return this.MouseState.XButton2;

                default:
                    throw new ArgumentException("button");
            }
        }

        public bool IsNewMouseButtonPress(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.MouseState.LeftButton == ButtonState.Pressed && this.PreviousMouseState.LeftButton == ButtonState.Released;

                case MouseButton.Right:
                    return this.MouseState.RightButton == ButtonState.Pressed && this.PreviousMouseState.RightButton == ButtonState.Released;

                case MouseButton.Middle:
                    return this.MouseState.MiddleButton == ButtonState.Pressed && this.PreviousMouseState.MiddleButton == ButtonState.Released;

                case MouseButton.X1:
                    return this.MouseState.XButton1 == ButtonState.Pressed && this.PreviousMouseState.XButton1 == ButtonState.Released;

                case MouseButton.X2:
                    return this.MouseState.XButton2 == ButtonState.Pressed && this.PreviousMouseState.XButton2 == ButtonState.Released;

                default:
                    throw new ArgumentException("button");
            }
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.MouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return this.MouseState.RightButton == ButtonState.Pressed;

                case MouseButton.Middle:
                    return this.MouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.X1:
                    return this.MouseState.XButton1 == ButtonState.Pressed;

                case MouseButton.X2:
                    return this.MouseState.XButton2 == ButtonState.Pressed;

                default:
                    throw new ArgumentException("button");
            }
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.MouseState.LeftButton == ButtonState.Released;

                case MouseButton.Right:
                    return this.MouseState.RightButton == ButtonState.Released;

                case MouseButton.Middle:
                    return this.MouseState.MiddleButton == ButtonState.Released;

                case MouseButton.X1:
                    return this.MouseState.XButton1 == ButtonState.Released;

                case MouseButton.X2:
                    return this.MouseState.XButton2 == ButtonState.Released;

                default:
                    throw new ArgumentException("button");
            }
        }

        public bool WasMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return this.PreviousMouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return this.PreviousMouseState.RightButton == ButtonState.Pressed;

                case MouseButton.Middle:
                    return this.PreviousMouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.X1:
                    return this.PreviousMouseState.XButton1 == ButtonState.Pressed;

                case MouseButton.X2:
                    return this.PreviousMouseState.XButton2 == ButtonState.Pressed;

                default:
                    throw new ArgumentException("button");
            }
        }

#endregion

#region Keyboard

        /// <summary>
        /// Checks if a key is pressed during the last frame
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return this.KeyboardState.IsKeyDown(key) && this.PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a key is pressed
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return this.KeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key is released
        /// </summary>
        public bool IsKeyReleased(Keys key)
        {
            return this.KeyboardState.IsKeyUp(key);
        }

        public bool WasKeyPressed(Keys key)
        {
            return this.PreviousKeyboardState.IsKeyDown(key);
        }

        // TODO pretty slow atm, MAKE IT FAST
        public IEnumerable<Keys> NewPressedKeys
        {
            get { return this.KeyboardState.GetPressedKeys().Except(this.PreviousKeyboardState.GetPressedKeys()); }
        }

#endregion
    }
}

#endif
#endregion

#region Windows Phone Version
#if WINDOWS_PHONE

namespace Flai.Input
{
    public struct TouchCollection : IEnumerable<TouchLocation>
    {
        public TouchLocation[] TouchLocations { get; private set; }
        public int Count { get { return this.TouchLocations.Length; } }

        public TouchCollection(TouchLocation[] touchLocations)
        {
            this.TouchLocations = touchLocations;
        }

        public TouchLocation this[int i]
        {
            get { return this.TouchLocations[i]; }
        }

        public static implicit operator TouchCollection(XnaTouchCollection xnaCollection)
        {
            return new TouchCollection(xnaCollection.Select(x => new TouchLocation(x)).ToArray());
        }

        public IEnumerator<TouchLocation> GetEnumerator()
        {
            return ((IEnumerable<TouchLocation>)this.TouchLocations).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public struct TouchLocation
    {
        private XnaTouchLocation _xnaLocation;
        public TouchLocationState State { get { return _xnaLocation.State; } }
        public Vector2 Position { get { return _xnaLocation.Position / InputState.TouchLocationScale; } }
        public int Id { get { return _xnaLocation.Id; } }

        public TouchLocation(XnaTouchLocation xnaLocation)
        {
            _xnaLocation = xnaLocation;
        }

        public bool TryGetPreviousLocation(out TouchLocation previousLocation)
        {
            XnaTouchLocation prev;
            bool wasSuccess = _xnaLocation.TryGetPreviousLocation(out prev);

            previousLocation = new TouchLocation(prev);
            return wasSuccess;
        }
    }

    public class InputState
    {
        private readonly List<GestureSample> _gestures = new List<GestureSample>();
        private readonly ReadOnlyList<GestureSample> _readOnlyGestures; 

        private GamePadState _previousGamePadState;
        private GamePadState _gamePadState;

        private TouchCollection _touchLocations;

        #region Properties

        public static Vector2 TouchLocationScale { get; set; } = Vector2.One;

        /// <summary>
        /// Gestures that are currently ongoing
        /// </summary>
        public ReadOnlyList<GestureSample> Gestures
        {
            get { return _readOnlyGestures; }
        }

        public TouchCollection TouchLocations
        {
            get { return _touchLocations; }
        }

        public IEnumerable<TouchLocation> NewTouchLocations
        {
            get
            {
                for (int i = 0; i < this.TouchLocations.Count; i++)
                {
                    if (this.TouchLocations[i].State == TouchLocationState.Pressed)
                    {
                        yield return this.TouchLocations[i];
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether back button was pressed during the previous frame
        /// </summary>
        public bool IsBackButtonPressed
        {
            get { return _gamePadState.IsButtonDown(Buttons.Back) && _previousGamePadState.IsButtonUp(Buttons.Back); }
        }

        public bool HasTouch
        {
            get
            {
                for (int i = 0; i < _touchLocations.Count; i++)
                {
                    if (_touchLocations[i].State == TouchLocationState.Pressed || _touchLocations[i].State == TouchLocationState.Moved)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasNewTouch
        {
            get
            {
                for (int i = 0; i < _touchLocations.Count; i++)
                {
                    if (_touchLocations[i].State == TouchLocationState.Pressed)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        internal InputState()
        {
            _readOnlyGestures = new ReadOnlyList<GestureSample>(_gestures);

            _touchLocations = TouchPanel.GetState();
            // TouchPanel.IsGestureAvailable throws excetion if enabled gestures has not been set
            TouchPanel.EnabledGestures = TouchPanel.EnabledGestures;
        }

        /// <summary>
        /// Updates the InputState. Call only once per frame
        /// </summary>
        internal void Update()
        {
            _touchLocations = TouchPanel.GetState();

            _gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                _gestures.Add(TouchPanel.ReadGesture());
            }

            _previousGamePadState = _gamePadState;
            _gamePadState = GamePad.GetState(PlayerIndex.One);
            if (_gamePadState.IsButtonDown(Buttons.Back))
            {
                FlaiGame.Current.Services.Get<IFontContainer>();
            }
        }

        public bool IsTouchAt(Rectangle screenArea)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (screenArea.Contains(_touchLocations[i].Position) && (_touchLocations[i].State == TouchLocationState.Pressed || _touchLocations[i].State == TouchLocationState.Moved))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTouchAt(Rectangle screenArea, TouchLocationState state)
        {
            TouchLocation touchLocation;
            return this.IsTouchAt(screenArea, state, out touchLocation);
        }

        public bool IsTouchAt(Rectangle area, out TouchLocation touchLocation)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (area.Contains(_touchLocations[i].Position) && (_touchLocations[i].State == TouchLocationState.Pressed || _touchLocations[i].State == TouchLocationState.Moved))
                {
                    touchLocation = _touchLocations[i];
                    return true;
                }
            }

            touchLocation = default(TouchLocation);
            return false;
        }

        public bool IsTouchAt(Rectangle screenArea, TouchLocationState state, out TouchLocation touchLocation)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (_touchLocations[i].State == state && screenArea.Contains(_touchLocations[i].Position))
                {
                    touchLocation = _touchLocations[i];
                    return true;
                }
            }

            touchLocation = default(TouchLocation);
            return false;
        }

        public bool IsNewTouchAt(Rectangle screenArea)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (screenArea.Contains(_touchLocations[i].Position) && _touchLocations[i].State == TouchLocationState.Pressed)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<TouchLocation> GetTouchLocationsAt(Rectangle screenArea)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (screenArea.Contains(_touchLocations[i].Position))
                {
                    yield return _touchLocations[i];
                }
            }
        }

        public IEnumerable<TouchLocation> GetNewTouchLocationsAt(Rectangle screenArea)
        {
            for (int i = 0; i < _touchLocations.Count; i++)
            {
                if (screenArea.Contains(_touchLocations[i].Position) && _touchLocations[i].State == TouchLocationState.Pressed)
                {
                    yield return _touchLocations[i];
                }
            }
        }

        public bool HasGesture(GestureType type)
        {
            GestureSample gesture;
            return this.HasGesture(type, out gesture);
        }

        public bool HasGesture(GestureType gestureType, out GestureSample gesture)
        {
            for (int i = 0; i < _gestures.Count; i++)
            {
                if (gestureType.ContainsFlag(_gestures[i].GestureType))
                {
                    gesture = _gestures[i];
                    return true;
                }
            }

            gesture = default(GestureSample);
            return false;
        }

        public bool HasGesture(GestureType type, Rectangle area)
        {
            GestureSample gesture;
            return this.HasGesture(type, area, out gesture);
        }

        public bool HasGesture(GestureType gestureType, Rectangle area, out GestureSample gesture)
        {
            for (int i = 0; i < _gestures.Count; i++)
            {
                if (gestureType.ContainsFlag(_gestures[i].GestureType) && area.Contains(_gestures[i].Position))
                {
                    gesture = _gestures[i];
                    return true;
                }
            }

            gesture = default(GestureSample);
            return false;
        }

        public IEnumerable<GestureSample> GetGesturesAt(Rectangle area)
        {
            for (int i = 0; i < _gestures.Count; i++)
            {
                if (area.Contains(_gestures[i].Position))
                {
                    yield return _gestures[i];
                }
            }
        }

        public IEnumerable<GestureSample> GetGesturesAt(GestureType gestureType, Rectangle area)
        {
            for (int i = 0; i < _gestures.Count; i++)
            {
                if (gestureType.ContainsFlag(_gestures[i].GestureType) && area.Contains(_gestures[i].Position))
                {
                    yield return _gestures[i];
                }
            }
        }
    }
}

#endif
#endregion