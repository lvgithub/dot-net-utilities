using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Core.Systems
{
    /// <summary>
    /// The global mouse hook. This can be used to globally capture mouse input.
    /// </summary>
    public static class MouseHook
    {
        // The handle to the hook (used for installing/uninstalling it).
        private static IntPtr hHook = IntPtr.Zero;

        //Delegate that points to the filter function
        private static Hooks.HookProc hookproc = new Hooks.HookProc(Filter);

        /// <summary>
        /// Delegate for handling mouse input.
        /// </summary>
        /// <param name="button">The mouse button that was pressed.</param>
        /// <returns>True if you want the key to pass through
        /// (be recognized for the app), False if you want it
        /// to be trapped (app never sees it).</returns>
        public delegate bool MouseButtonHandler(MouseButtons button);

        public delegate bool MouseMoveHandler(Point point);

        public delegate bool MouseScrollHandler(int delta);

        public static MouseButtonHandler ButtonDown;
        public static MouseButtonHandler ButtonUp;
        public static MouseMoveHandler Moved;
        public static MouseScrollHandler Scrolled;

        /// <summary>
        /// Keep track of the hook state.
        /// </summary>
        private static bool Enabled;

        /// <summary>
        /// Start the mouse hook.
        /// </summary>
        /// <returns>True if no exceptions.</returns>
        public static bool Enable()
        {
            if (Enabled == false)
            {
                try
                {
                    using (Process curProcess = Process.GetCurrentProcess())
                    using (ProcessModule curModule = curProcess.MainModule)
                        hHook = Hooks.SetWindowsHookEx((int)Hooks.HookType.WH_MOUSE_LL, hookproc, Hooks.GetModuleHandle(curModule.ModuleName), 0);
                    Enabled = true;
                    return true;
                }
                catch
                {
                    Enabled = false;
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Disable mouse hooking.
        /// </summary>
        /// <returns>True if disabled correctly.</returns>
        public static bool Disable()
        {
            if (Enabled == true)
            {
                try
                {
                    Hooks.UnhookWindowsHookEx(hHook);
                    Enabled = false;
                    return true;
                }
                catch
                {
                    Enabled = true;
                    return false;
                }
            }
            else
                return false;
        }

        private static IntPtr Filter(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool result = true;

            if (nCode >= 0)
            {
                Hooks.MouseHookStruct info = (Hooks.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Hooks.MouseHookStruct));
                switch ((int)wParam)
                {
                    case Hooks.WM_LBUTTONDOWN:
                        result = OnMouseDown(MouseButtons.Left);
                        break;
                    case Hooks.WM_LBUTTONUP:
                        result = OnMouseUp(MouseButtons.Left);
                        break;
                    case Hooks.WM_RBUTTONDOWN:
                        result = OnMouseDown(MouseButtons.Right);
                        break;
                    case Hooks.WM_RBUTTONUP:
                        result = OnMouseUp(MouseButtons.Right);
                        break;
                    case Hooks.WM_MBUTTONDOWN:
                        result = OnMouseDown(MouseButtons.Middle);
                        break;
                    case Hooks.WM_MBUTTONUP:
                        result = OnMouseUp(MouseButtons.Middle);
                        break;
                    case Hooks.WM_XBUTTONDOWN:
                        if (info.Data >> 16 == Hooks.XBUTTON1)
                            result = OnMouseDown(MouseButtons.XButton1);
                        else if (info.Data >> 16 == Hooks.XBUTTON2)
                            result = OnMouseDown(MouseButtons.XButton2);
                        break;
                    case Hooks.WM_XBUTTONUP:
                        if (info.Data >> 16 == Hooks.XBUTTON1)
                            result = OnMouseUp(MouseButtons.XButton1);
                        else if (info.Data >> 16 == Hooks.XBUTTON2)
                            result = OnMouseUp(MouseButtons.XButton2);
                        break;
                    case Hooks.WM_MOUSEMOVE:
                        result = OnMouseMove(new Point(info.Point.X, info.Point.Y));
                        break;
                    case Hooks.WM_MOUSEWHEEL:
                        result = OnMouseWheel((info.Data >> 16) & 0xffff);
                        break;
                }
            }

            return result ? Hooks.CallNextHookEx(hHook, nCode, wParam, lParam) : new IntPtr(1);
        }

        private static bool OnMouseDown(MouseButtons button)
        {
            if (ButtonDown != null)
                return ButtonDown(button);
            else
                return true;
        }

        private static bool OnMouseUp(MouseButtons button)
        {
            if (ButtonUp != null)
                return ButtonUp(button);
            else
                return true;
        }

        private static bool OnMouseMove(Point point)
        {
            if (Moved != null)
                return Moved(point);
            else
                return true;
        }

        private static bool OnMouseWheel(int delta)
        {
            if (Scrolled != null)
                return Scrolled(delta);
            else
                return true;
        }
    }

    public static class Hooks
    {
        #region Interop

        internal const int WM_SIZE = 0x5;
        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_CHAR = 0x0102;
        internal const int WM_SYSKEYDOWN = 0x0104;
        internal const int WM_SYSKEYUP = 0x0105;

        //This is the Import for the SetWindowsHookEx function.
        //Use this function to install a thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

        //This is the Import for the UnhookWindowsHookEx function.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);

        //This is the Import for the CallNextHookEx function.
        //Use this function to pass the hook information to the next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        // The type of method used as a handler (Filter)
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Different types of hooks.
        internal enum HookType : int
        {
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        //Mouse stuff
        internal const int WM_MOUSEMOVE = 0x200;
        internal const int WM_MOUSEWHEEL = 0x020A;
        internal const int WM_LBUTTONDOWN = 0x201;
        internal const int WM_RBUTTONDOWN = 0x204;
        internal const int WM_MBUTTONDOWN = 0x207;
        internal const int WM_XBUTTONDOWN = 0x20B;
        internal const int WM_LBUTTONUP = 0x202;
        internal const int WM_RBUTTONUP = 0x205;
        internal const int WM_MBUTTONUP = 0x208;
        internal const int WM_XBUTTONUP = 0x20C;

        internal const int XBUTTON1 = 0x1;
        internal const int XBUTTON2 = 0x2;

        [StructLayout(LayoutKind.Sequential)]
        internal struct MouseHookStruct
        {
            public Point Point;
            public int Data;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Point
        {
            public int X;
            public int Y;
        }
    }
}
