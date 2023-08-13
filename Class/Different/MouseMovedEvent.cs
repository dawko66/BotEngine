using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#region Enumerations

public enum MouseMessages
{
    WM_LBUTTONDOWN = 0x0201,
    WM_LBUTTONUP = 0x0202,
    WM_MOUSEMOVE = 0x0200,
    WM_MOUSEWHEEL = 0x020A,
    WM_RBUTTONDOWN = 0x0204,
    WM_RBUTTONUP = 0x0205
}

#endregion Enumerations

#region Delegates

public delegate IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

public delegate void MouseHookFunction(MouseHooker m, MouseMessages message, HookStruct str);

#endregion Delegates

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int x;
    public int y;

    #region Methods

    public static implicit operator Point(POINT point)
    {
        return new Point(point.x, point.y);
    }

    #endregion Methods
}

[StructLayout(LayoutKind.Sequential)]
public class HookStruct
{
    public POINT pt;
    public int hwnd;
    public int wHitTestCode;
    public int dwExtraInfo;
}

public class MouseHooker
{
    #region Fields

    public static MouseHooker Instance;

    private const int WH_MOUSE_LL = 14;
    private const int WH_MOUSE = 7;

    private IntPtr Hook;
    private MouseProc HookHandleFct;

    #endregion Fields

    #region event
    public class MouseeEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MouseMessages MouseAction { get; set; }
    }
    public event EventHandler<MouseeEventArgs> MouseActionEvent;
    protected virtual void MouseAction(MouseeEventArgs e)
    {
        MouseActionEvent?.Invoke(this, e);
    }
    #endregion


    #region Constructors

    public MouseHooker()
    {
        Instance = this;
    }

    #endregion Constructors

    #region Methods
    public void ActivateHook()
    {
        HookHandleFct = HookHandle;
        IntPtr hInstance = LoadLibrary("User32");
        Hook = SetWindowsHookEx(WH_MOUSE_LL, HookHandleFct, hInstance, 0);
    }

    public void DeactivateHook()
    {
        UnhookWindowsHookEx(Hook);
    }

    public IntPtr HookHandle(int code, IntPtr wParam, IntPtr lParam)
    {
        HookStruct hookStruct = (HookStruct)Marshal.PtrToStructure(lParam, typeof(HookStruct));

        int X = hookStruct.pt.x;
        int Y = hookStruct.pt.y;

        X = X < 0 ? 0 : X;
        Y = Y < 0 ? 0 : Y;
        X = X > SystemParameters.PrimaryScreenWidth - 1 ? (int)SystemParameters.PrimaryScreenWidth - 1  : X;
        Y = Y > SystemParameters.PrimaryScreenHeight - 1 ? (int)SystemParameters.PrimaryScreenHeight - 1 : Y;

        MouseAction(new MouseeEventArgs()
        {
            X = X,
            Y = Y,
            MouseAction = (MouseMessages)wParam
        });

        return CallNextHookEx(Instance.Hook, code, (int)wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(int idHook, MouseProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    #endregion Methods
}