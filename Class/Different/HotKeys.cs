using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Forms;
using AutoBot_2._0.Class.Graph;

public class HotKeys
{
    #region HotKeys initialize & uninitialize
    /*
    HotKeys hotKeys;
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        HotKeys.HotKey[] keys = new HotKeys.HotKey[]
        {
                new HotKeys.HotKey(a, ModifierKeys.None, System.Windows.Forms.Keys.F1),
                new HotKeys.HotKey(b, (int)ModifierKeys.None, (uint)System.Windows.Forms.Keys.F2)
        };


        hotKeys = new HotKeys(this, keys);
    }
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        hotKeys.stop();
    }
    private void a(){}
    private void b(){}
    */
    #endregion

    public struct HotKey
    {
        private readonly Action VoidFunction;
        public uint ModifierKey { get; }
        public uint Key { get; }

        public HotKey(Action VoidFunction, uint ModifierKey, uint Key)
        {
            this.VoidFunction = VoidFunction;
            this.ModifierKey = ModifierKey;
            this.Key = Key;
        }
        public HotKey(Action VoidFunction, ModifierKeys ModifierKey, Key Key)
        {
            this.VoidFunction = VoidFunction;
            this.ModifierKey = (uint)ModifierKey;
            this.Key = (uint)Key;
        }

        public void DoAction()
        {
            VoidFunction();
        }
    }

    private Window window;
    public HotKey[] hotKeys;
    public HotKeys(Window window, HotKey[] hotKeys)
    {
        this.window = window;
        this.hotKeys = hotKeys;
        start();
    }

    IntPtr windowHandle;
    HwndSource source;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    private protected void start()
    {
        windowHandle = new WindowInteropHelper(window).Handle;
        source = HwndSource.FromHwnd(windowHandle);
        source.AddHook(HwndHook);

        /* Key to action */
        for (int i = 0; i < hotKeys.Length; i++)
            RegisterHotKey(windowHandle, i, hotKeys[i].ModifierKey, hotKeys[i].Key);
    }

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {

        #region ~~!!!~~ scan code check here (in work)
        if (msg == 256) // control key
        {

        }


        if (msg == 0x0312)
        {
            AutoBot_2._0.Class.Graph.Console.AddMessage(msg.ToString());


        }

        IntPtr param = new IntPtr(0x0008063c);
        if (hwnd == param || msg == (int)param || wParam == param || lParam == param)
        {
            if (wParam != new IntPtr(0) && wParam != new IntPtr(1) && wParam != new IntPtr(2))
            {

            }
        }
        #endregion

        /* Key actions */
        if (msg == 0x0312)
            for (int i = 0; i < hotKeys.Length; i++)
                if (i == wParam.ToInt32())
                {
                    hotKeys[i].DoAction();
                    break;
                }

        return IntPtr.Zero;
    }

    #region clear hooks
    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private protected void clearHotKeys()
    {
        source.RemoveHook(HwndHook);

        for (int i = 0; i < hotKeys.Length; i++)
            UnregisterHotKey(windowHandle, i);
    }
    public void stop()
    {
        clearHotKeys();
    }
    #endregion
}
