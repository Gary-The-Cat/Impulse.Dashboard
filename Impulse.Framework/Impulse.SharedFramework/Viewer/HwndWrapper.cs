// <copyright file="HwndWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Impulse.SharedFramework.Viewer.Controls;

namespace Impulse.SharedFramework.Viewer;

public abstract class HwndWrapper : HwndHost
{
    // The name of our window class
    private const string WindowClass = "GraphicsDeviceControlHostWindowClass";

    // Track the mouse state
    private readonly HwndMouseState mouseState = new HwndMouseState();

    // The HWND we present to when rendering
    private IntPtr hWnd;

    // For holding previous hWnd focus
    private IntPtr hWndPrev;

    // Track if the application has focus
    private bool applicationHasFocus;

    // Track if the mouse is in the window
    private bool mouseInWindow;

    // Track the previous mouse position
    private Point previousPosition;

    // Tracking whether we've "capture" the mouse
    private bool isMouseCaptured;

    protected HwndWrapper()
    {
        // We must be notified of the application foreground status for our mouse input events
        System.Windows.Application.Current.Activated += OnApplicationActivated;
        System.Windows.Application.Current.Deactivated += OnApplicationDeactivated;

        // We use the CompositionTargetEx.Rendering event to trigger the control to draw itself
        CompositionTargetEx.Rendering += OnCompositionTargetExRendering;

        // Check whether the application is active (it almost certainly is, at this point).
        if (System.Windows.Application.Current.Windows.Cast<Window>().Any(x => x.IsActive))
        {
            applicationHasFocus = true;
        }
    }

    /// <summary>
    /// Invoked when the control receives a left mouse down message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndLButtonDown;

    /// <summary>
    /// Invoked when the control receives a left mouse up message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndLButtonUp;

    /// <summary>
    /// Invoked when the control receives a left mouse double click message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndLButtonDblClick;

    /// <summary>
    /// Invoked when the control receives a right mouse down message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndRButtonDown;

    /// <summary>
    /// Invoked when the control receives a right mouse up message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndRButtonUp;

    /// <summary>
    /// Invoked when the control receives a rigt mouse double click message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndRButtonDblClick;

    /// <summary>
    /// Invoked when the control receives a middle mouse down message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMButtonDown;

    /// <summary>
    /// Invoked when the control receives a middle mouse up message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMButtonUp;

    /// <summary>
    /// Invoked when the control receives a middle mouse double click message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMButtonDblClick;

    /// <summary>
    /// Invoked when the control receives a mouse down message for the first extra button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDown;

    /// <summary>
    /// Invoked when the control receives a mouse up message for the first extra button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX1ButtonUp;

    /// <summary>
    /// Invoked when the control receives a double click message for the first extra mouse button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDblClick;

    /// <summary>
    /// Invoked when the control receives a mouse down message for the second extra button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDown;

    /// <summary>
    /// Invoked when the control receives a mouse up message for the second extra button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX2ButtonUp;

    /// <summary>
    /// Invoked when the control receives a double click message for the first extra mouse button.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDblClick;

    /// <summary>
    /// Invoked when the control receives a mouse move message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMouseMove;

    /// <summary>
    /// Invoked when the control first gets a mouse move message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMouseEnter;

    /// <summary>
    /// Invoked when the control gets a mouse leave message.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMouseLeave;

    /// <summary>
    /// Invoked when the control recieves a mouse wheel delta.
    /// </summary>
    public event EventHandler<HwndMouseEventArgs> HwndMouseWheel;

    public new bool IsMouseCaptured
    {
        get { return isMouseCaptured; }
    }

    /// <summary>
    /// Captures the mouse, hiding it and trapping it inside the window bounds.
    /// </summary>
    /// <remarks>
    /// This method is useful for tooling scenarios where you only care about the mouse deltas
    /// and want the user to be able to continue interacting with the window while they move
    /// the mouse. A good example of this is rotating an object based on the mouse deltas where
    /// through capturing you can spin and spin without having the cursor leave the window.
    /// </remarks>
    public new void CaptureMouse()
    {
        // Don't do anything if the mouse is already captured
        if (isMouseCaptured)
        {
            return;
        }

        NativeMethods.SetCapture(hWnd);
        isMouseCaptured = true;
    }

    /// <summary>
    /// Releases the capture of the mouse which makes it visible and allows it to leave the window bounds.
    /// </summary>
    public new void ReleaseMouseCapture()
    {
        // Don't do anything if the mouse is not captured
        if (!isMouseCaptured)
        {
            return;
        }

        NativeMethods.ReleaseCapture();
        isMouseCaptured = false;
    }

    protected abstract void Render(IntPtr windowHandle);

    protected override void Dispose(bool disposing)
    {
        // Unhook all events.
        CompositionTargetEx.Rendering -= OnCompositionTargetExRendering;
        if (System.Windows.Application.Current != null)
        {
            System.Windows.Application.Current.Activated -= OnApplicationActivated;
            System.Windows.Application.Current.Deactivated -= OnApplicationDeactivated;
        }

        base.Dispose(disposing);
    }

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        // Create the host window as a child of the parent
        hWnd = CreateHostWindow(hwndParent.Handle);
        return new HandleRef(this, hWnd);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        // Destroy the window and reset our hWnd value
        NativeMethods.DestroyWindow(hwnd.Handle);
        hWnd = IntPtr.Zero;
    }

    protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case NativeMethods.WM_LBUTTONDOWN:
                mouseState.LeftButton = MouseButtonState.Pressed;
                RaiseHwndLButtonDown(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_LBUTTONUP:
                mouseState.LeftButton = MouseButtonState.Released;
                RaiseHwndLButtonUp(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_LBUTTONDBLCLK:
                RaiseHwndLButtonDblClick(new HwndMouseEventArgs(mouseState, MouseButton.Left));
                break;
            case NativeMethods.WM_RBUTTONDOWN:
                mouseState.RightButton = MouseButtonState.Pressed;
                RaiseHwndRButtonDown(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_RBUTTONUP:
                mouseState.RightButton = MouseButtonState.Released;
                RaiseHwndRButtonUp(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_RBUTTONDBLCLK:
                RaiseHwndRButtonDblClick(new HwndMouseEventArgs(mouseState, MouseButton.Right));
                break;
            case NativeMethods.WM_MBUTTONDOWN:
                mouseState.MiddleButton = MouseButtonState.Pressed;
                RaiseHwndMButtonDown(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_MBUTTONUP:
                mouseState.MiddleButton = MouseButtonState.Released;
                RaiseHwndMButtonUp(new HwndMouseEventArgs(mouseState));
                break;
            case NativeMethods.WM_MBUTTONDBLCLK:
                RaiseHwndMButtonDblClick(new HwndMouseEventArgs(mouseState, MouseButton.Middle));
                break;
            case NativeMethods.WM_XBUTTONDOWN:
                if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                {
                    mouseState.X1Button = MouseButtonState.Pressed;
                    RaiseHwndX1ButtonDown(new HwndMouseEventArgs(mouseState));
                }
                else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                {
                    mouseState.X2Button = MouseButtonState.Pressed;
                    RaiseHwndX2ButtonDown(new HwndMouseEventArgs(mouseState));
                }

                break;
            case NativeMethods.WM_XBUTTONUP:
                if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                {
                    mouseState.X1Button = MouseButtonState.Released;
                    RaiseHwndX1ButtonUp(new HwndMouseEventArgs(mouseState));
                }
                else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                {
                    mouseState.X2Button = MouseButtonState.Released;
                    RaiseHwndX2ButtonUp(new HwndMouseEventArgs(mouseState));
                }

                break;
            case NativeMethods.WM_XBUTTONDBLCLK:
                if (((int)wParam & NativeMethods.MK_XBUTTON1) != 0)
                {
                    RaiseHwndX1ButtonDblClick(new HwndMouseEventArgs(mouseState, MouseButton.XButton1));
                }
                else if (((int)wParam & NativeMethods.MK_XBUTTON2) != 0)
                {
                    RaiseHwndX2ButtonDblClick(new HwndMouseEventArgs(mouseState, MouseButton.XButton2));
                }

                break;
            case NativeMethods.WM_MOUSEMOVE:
                // If the application isn't in focus, we don't handle this message
                if (!applicationHasFocus)
                {
                    break;
                }

                // record the prevous and new position of the mouse
                mouseState.ScreenPosition = PointToScreen(new Point(
                    NativeMethods.GetXLParam((int)lParam),
                    NativeMethods.GetYLParam((int)lParam)));

                if (!mouseInWindow)
                {
                    mouseInWindow = true;

                    RaiseHwndMouseEnter(new HwndMouseEventArgs(mouseState));

                    // Track the previously focused window, and set focus to this window.
                    hWndPrev = NativeMethods.GetFocus();
                    NativeMethods.SetFocus(hWnd);

                    // send the track mouse event so that we get the WM_MOUSELEAVE message
                    var tme = new NativeMethods.TRACKMOUSEEVENT
                    {
                        cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT)),
                        dwFlags = NativeMethods.TME_LEAVE,
                        hWnd = hwnd
                    };
                    NativeMethods.TrackMouseEvent(ref tme);
                }

                if (mouseState.ScreenPosition != previousPosition)
                {
                    RaiseHwndMouseMove(new HwndMouseEventArgs(mouseState));
                }

                previousPosition = mouseState.ScreenPosition;

                break;
            case NativeMethods.WM_MOUSELEAVE:

                // If we have capture, we ignore this message because we're just
                // going to reset the cursor position back into the window
                if (isMouseCaptured)
                {
                    break;
                }

                // Reset the state which releases all buttons and
                // marks the mouse as not being in the window.
                ResetMouseState();

                RaiseHwndMouseLeave(new HwndMouseEventArgs(mouseState));

                NativeMethods.SetFocus(hWndPrev);

                break;
        }

        return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
    }

    protected virtual void RaiseHwndLButtonDown(HwndMouseEventArgs args)
    {
        var handler = HwndLButtonDown;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndLButtonUp(HwndMouseEventArgs args)
    {
        var handler = HwndLButtonUp;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndRButtonDown(HwndMouseEventArgs args)
    {
        var handler = HwndRButtonDown;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndRButtonUp(HwndMouseEventArgs args)
    {
        var handler = HwndRButtonUp;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMButtonDown(HwndMouseEventArgs args)
    {
        var handler = HwndMButtonDown;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMButtonUp(HwndMouseEventArgs args)
    {
        var handler = HwndMButtonUp;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndLButtonDblClick(HwndMouseEventArgs args)
    {
        var handler = HwndLButtonDblClick;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndRButtonDblClick(HwndMouseEventArgs args)
    {
        var handler = HwndRButtonDblClick;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMButtonDblClick(HwndMouseEventArgs args)
    {
        var handler = HwndMButtonDblClick;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMouseEnter(HwndMouseEventArgs args)
    {
        var handler = HwndMouseEnter;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX1ButtonDown(HwndMouseEventArgs args)
    {
        var handler = HwndX1ButtonDown;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX1ButtonUp(HwndMouseEventArgs args)
    {
        var handler = HwndX1ButtonUp;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX2ButtonDown(HwndMouseEventArgs args)
    {
        var handler = HwndX2ButtonDown;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX2ButtonUp(HwndMouseEventArgs args)
    {
        var handler = HwndX2ButtonUp;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX1ButtonDblClick(HwndMouseEventArgs args)
    {
        var handler = HwndX1ButtonDblClick;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndX2ButtonDblClick(HwndMouseEventArgs args)
    {
        var handler = HwndX2ButtonDblClick;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMouseLeave(HwndMouseEventArgs args)
    {
        var handler = HwndMouseLeave;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMouseMove(HwndMouseEventArgs args)
    {
        var handler = HwndMouseMove;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    protected virtual void RaiseHwndMouseWheel(HwndMouseEventArgs args)
    {
        var handler = HwndMouseWheel;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    private void OnCompositionTargetExRendering(object sender, EventArgs e)
    {
        // Get the current width and height of the control
        var width = (int)ActualWidth;
        var height = (int)ActualHeight;

        // If the control has no width or no height, skip drawing since it's not visible
        if (width < 1 || height < 1)
        {
            return;
        }

        Render(hWnd);
    }

    private void OnApplicationActivated(object sender, EventArgs e)
    {
        applicationHasFocus = true;
    }

    private void OnApplicationDeactivated(object sender, EventArgs e)
    {
        applicationHasFocus = false;
        ResetMouseState();

        if (mouseInWindow)
        {
            mouseInWindow = false;
            RaiseHwndMouseLeave(new HwndMouseEventArgs(mouseState));
        }

        ReleaseMouseCapture();
    }

    private void ResetMouseState()
    {
        // We need to invoke events for any buttons that were pressed
        bool fireL = mouseState.LeftButton == MouseButtonState.Pressed;
        bool fireM = mouseState.MiddleButton == MouseButtonState.Pressed;
        bool fireR = mouseState.RightButton == MouseButtonState.Pressed;
        bool fireX1 = mouseState.X1Button == MouseButtonState.Pressed;
        bool fireX2 = mouseState.X2Button == MouseButtonState.Pressed;

        // Update the state of all of the buttons
        mouseState.LeftButton = MouseButtonState.Released;
        mouseState.MiddleButton = MouseButtonState.Released;
        mouseState.RightButton = MouseButtonState.Released;
        mouseState.X1Button = MouseButtonState.Released;
        mouseState.X2Button = MouseButtonState.Released;

        // Fire any events
        var args = new HwndMouseEventArgs(mouseState);
        if (fireL)
        {
            RaiseHwndLButtonUp(args);
        }

        if (fireM)
        {
            RaiseHwndMButtonUp(args);
        }

        if (fireR)
        {
            RaiseHwndRButtonUp(args);
        }

        if (fireX1)
        {
            RaiseHwndX1ButtonUp(args);
        }

        if (fireX2)
        {
            RaiseHwndX2ButtonUp(args);
        }

        // The mouse is no longer considered to be in our window
        mouseInWindow = false;
    }

    /// <summary>
    /// Creates the host window as a child of the parent window.
    /// </summary>
    private IntPtr CreateHostWindow(IntPtr hWndParent)
    {
        // Register our window class
        RegisterWindowClass();

        // Create the window
        return NativeMethods.CreateWindowEx(
            0,
            WindowClass,
            string.Empty,
            NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE,
            0,
            0,
            (int)Width,
            (int)Height,
            hWndParent,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);
    }

    /// <summary>
    /// Registers the window class.
    /// </summary>
    private void RegisterWindowClass()
    {
        var wndClass = default(NativeMethods.WNDCLASSEX);
        wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
        wndClass.hInstance = NativeMethods.GetModuleHandle(null);
        wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
        wndClass.lpszClassName = WindowClass;
        wndClass.hCursor = NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.IDC_ARROW);

        NativeMethods.RegisterClassEx(ref wndClass);
    }
}
