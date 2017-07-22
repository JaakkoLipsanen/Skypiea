#if WINDOWS

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;

namespace Flai.Input
{
    #region Input Args

    public class CharacterEventArgs : EventArgs
    {
        private readonly char _character;
        private readonly int _lParam;

        public char Character { get { return _character; } }
        public int Param { get { return _lParam; } }
        public int RepeatCount { get { return _lParam & 0xffff; } }
        public bool ExtendedKey { get { return (_lParam & (1 << 24)) > 0; } }
        public bool AltPressed { get { return (_lParam & (1 << 29)) > 0; } }
        public bool PreviousState { get { return (_lParam & (1 << 30)) > 0; } }
        public bool TransitionState { get { return (_lParam & (1 << 31)) > 0; } }

        internal CharacterEventArgs(char character, int lParam)
        {
            _character = character;
            _lParam = lParam;
        }
    }

    public class KeyEventArgs : EventArgs
    {
        private readonly Keys _key;

        public Keys Key
        {
            get { return _key; }
        }

        internal KeyEventArgs(Keys key)
        {
            _key = key;
        }
    }

    public class MouseEventArgs : EventArgs
    {
        private readonly MouseButton? _button;
        private readonly int _clicks;
        private readonly int _x;
        private readonly int _y;
        private readonly int _delta;

        public MouseButton? Button
        {
            get { return _button; }
        }

        public int Clicks
        {
            get { return _clicks; }
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public Point Location
        {
            get { return new Point(_x, _y); }
        }

        public int Delta
        {
            get { return _delta; }
        }

        internal MouseEventArgs(MouseButton? button, int clicks, int x, int y, int delta)
        {
            _button = button;
            _clicks = clicks;
            _x = x;
            _y = y;
            _delta = delta;
        }
    }

    public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    #endregion

    public static class SystemInput
    {
        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private static IntPtr _prevWndProc;
        private static WndProc _hookProcDelegate;
        private static IntPtr _hIMC;

        #region Events

        /// <summary>
        /// Event raised when a character has been entered.
        /// </summary>         
        public static event CharEnteredHandler CharEntered;

        /// <summary>
        /// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
        /// </summary>          
        public static event KeyEventHandler KeyDown;

        /// <summary>
        /// Event raised when a key has been released.
        /// </summary>
        public static event KeyEventHandler KeyUp;

        /// <summary>
        /// Event raised when a mouse button is pressed.
        /// </summary>
        public static event MouseEventHandler MouseDown;

        /// <summary>
        /// Event raised when a mouse button is released.
        /// </summary>   
        public static event MouseEventHandler MouseUp;

        /// <summary>
        /// Event raised when the mouse changes location.
        /// </summary>
        public static event MouseEventHandler MouseMove;

        /// <summary>
        /// Event raised when the mouse has hovered in the same location for a short period of time.
        /// </summary>        
        public static event MouseEventHandler MouseHover;

        /// <summary>
        /// Event raised when the mouse wheel has been moved.
        /// </summary>       
        public static event MouseEventHandler MouseWheel;

        /// <summary>
        /// Event raised when a mouse button has been double clicked.
        /// </summary>       
        public static event MouseEventHandler MouseDoubleClick;

        #endregion

        #region Win32 Constants

        private const int GWL_WNDPROC = -4;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_CHAR = 0x102;
        private const int WM_IME_SETCONTEXT = 0x281;
        private const int WM_INPUTLANGCHANGE = 0x51;
        private const int WM_GETDLGCODE = 0x87;
        private const int WM_IME_COMPOSITION = 0x10F;
        private const int DLGC_WANTALLKEYS = 4;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x20A;
        private const int WM_XBUTTONDOWN = 0x20B;
        private const int WM_XBUTTONUP = 0x20C;
        private const int WM_XBUTTONDBLCLK = 0x20D;
        private const int WM_MOUSEHOVER = 0x2A1;

        #endregion

        #region DLL Imports

        [DllImport("Imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        public static Vector2i MousePosition
        {
            get
            {
                MouseState mouseState = Mouse.GetState();
                return new Vector2i(mouseState.X, mouseState.Y);
            }
        }

        static SystemInput()
        {
            if (FlaiGame.Current == null)
            {
                throw new InvalidOperationException("Instance of FlaiGame must be initialized before accessing SystemInput");
            }

            _hookProcDelegate = SystemInput.HookProc;
            _prevWndProc = (IntPtr)SystemInput.SetWindowLong(FlaiGame.Current.Window.Handle, GWL_WNDPROC, (int)Marshal.GetFunctionPointerForDelegate(_hookProcDelegate));
            _hIMC = SystemInput.ImmGetContext(FlaiGame.Current.Window.Handle);
        }

        // Using this seems to cause some problems. Something isnt disposed I think?
        public static void Reinitialize(IntPtr handle)
        {
            _hookProcDelegate = SystemInput.HookProc;
            _prevWndProc = (IntPtr)SystemInput.SetWindowLong(handle, GWL_WNDPROC, (int)Marshal.GetFunctionPointerForDelegate(_hookProcDelegate));
            _hIMC = SystemInput.ImmGetContext(handle);
        }

        #region Process Input Event

        private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr returnCode = SystemInput.CallWindowProc(_prevWndProc, hWnd, msg, wParam, lParam);
            switch (msg)
            {
                case WM_GETDLGCODE:
                    returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
                    break;
                case WM_KEYDOWN:
                    if (SystemInput.KeyDown != null)
                        SystemInput.KeyDown(null, new KeyEventArgs((Keys)wParam));
                    break;
                case WM_KEYUP:
                    if (SystemInput.KeyUp != null)
                        SystemInput.KeyUp(null, new KeyEventArgs((Keys)wParam));
                    break;
                case WM_CHAR:
                    if (SystemInput.CharEntered != null)
                        SystemInput.CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;
                case WM_IME_SETCONTEXT:
                    if (wParam.ToInt32() == 1)
                        SystemInput.ImmAssociateContext(hWnd, _hIMC);
                    break;
                case WM_INPUTLANGCHANGE:
                    SystemInput.ImmAssociateContext(hWnd, _hIMC);
                    returnCode = (IntPtr)1;
                    break;

                // Mouse messages                      
                case WM_MOUSEMOVE:
                    if (SystemInput.MouseMove != null)
                    {
                        short x, y;
                        SystemInput.MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        SystemInput.MouseMove(null, new MouseEventArgs(null, 0, x, y, 0));
                    }
                    break;
                case WM_MOUSEHOVER:
                    if (SystemInput.MouseHover != null)
                    {
                        short x, y;
                        SystemInput.MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        SystemInput.MouseHover(null, new MouseEventArgs(null, 0, x, y, 0));
                    }
                    break;
                case WM_MOUSEWHEEL:
                    if (SystemInput.MouseWheel != null)
                    {
                        short x, y;
                        SystemInput.MouseLocationFromLParam(lParam.ToInt32(), out x, out y);
                        SystemInput.MouseWheel(null, new MouseEventArgs(null, 0, x, y, (wParam.ToInt32() >> 16) / 120));
                    }
                    break;
                case WM_LBUTTONDOWN:
                    SystemInput.RaiseMouseDownEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_LBUTTONUP:
                    SystemInput.RaiseMouseUpEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_LBUTTONDBLCLK:
                    SystemInput.RaiseMouseDblClickEvent(MouseButton.Left, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONDOWN:
                    SystemInput.RaiseMouseDownEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONUP:
                    SystemInput.RaiseMouseUpEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_RBUTTONDBLCLK:
                    SystemInput.RaiseMouseDblClickEvent(MouseButton.Right, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONDOWN:
                    SystemInput.RaiseMouseDownEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONUP:
                    SystemInput.RaiseMouseUpEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_MBUTTONDBLCLK:
                    SystemInput.RaiseMouseDblClickEvent(MouseButton.Middle, wParam.ToInt32(), lParam.ToInt32());
                    break;
                case WM_XBUTTONDOWN:
                    if ((wParam.ToInt32() & 0x10000) != 0)
                    {
                        SystemInput.RaiseMouseDownEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    }
                    else if ((wParam.ToInt32() & 0x20000) != 0)
                    {
                        SystemInput.RaiseMouseDownEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
                case WM_XBUTTONUP:
                    if ((wParam.ToInt32() & 0x10000) != 0)
                    {
                        SystemInput.RaiseMouseUpEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    }
                    else if ((wParam.ToInt32() & 0x20000) != 0)
                    {
                        SystemInput.RaiseMouseUpEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
                case WM_XBUTTONDBLCLK:
                    if ((wParam.ToInt32() & 0x10000) != 0)
                    {
                        SystemInput.RaiseMouseDblClickEvent(MouseButton.X1, wParam.ToInt32(), lParam.ToInt32());
                    }
                    else if ((wParam.ToInt32() & 0x20000) != 0)
                    {
                        SystemInput.RaiseMouseDblClickEvent(MouseButton.X2, wParam.ToInt32(), lParam.ToInt32());
                    }
                    break;
            }

            return returnCode;
        }

        #endregion

        #region Mouse Message Helpers

        private static void RaiseMouseDownEvent(MouseButton button, int wParam, int lParam)
        {
            if (SystemInput.MouseDown != null)
            {
                short x, y;
                SystemInput.MouseLocationFromLParam(lParam, out x, out y);
                SystemInput.MouseDown(null, new MouseEventArgs(button, 1, x, y, 0));
            }
        }

        private static void RaiseMouseUpEvent(MouseButton button, int wParam, int lParam)
        {
            if (SystemInput.MouseUp != null)
            {
                short x, y;
                SystemInput.MouseLocationFromLParam(lParam, out x, out y);
                SystemInput.MouseUp(null, new MouseEventArgs(button, 1, x, y, 0));
            }
        }

        private static void RaiseMouseDblClickEvent(MouseButton button, int wParam, int lParam)
        {
            if (SystemInput.MouseDoubleClick != null)
            {
                short x, y;
                SystemInput.MouseLocationFromLParam(lParam, out x, out y);
                SystemInput.MouseDoubleClick(null, new MouseEventArgs(button, 1, x, y, 0));
            }
        }

        private static void MouseLocationFromLParam(int lParam, out short x, out short y)
        {
            // Cast to signed shorts to get sign extension on negative coordinates (of course this would only be possible if mouse capture was enabled).       
            x = (short)(lParam & 0xFFFF);
            y = (short)(lParam >> 16);
        }

        #endregion
    }

}

#endif