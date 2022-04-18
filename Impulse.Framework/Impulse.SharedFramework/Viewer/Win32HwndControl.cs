// <copyright file="Win32HwndControl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace Impulse.SharedFramework.Viewer;

/// <summary>
/// Creates internal Hwnd to host DirectXComponent within a control in the window.
/// </summary>
public abstract class Win32HwndControl : HwndWrapper
{
    private const string WindowClass = "HwndWrapper";

    protected Win32HwndControl()
    {
        Loaded += OnLoaded;
    }

    public IntPtr Hwnd { get; set; }

    protected bool HwndInitialized { get; private set; }

    // :TODO: This needs to be managed by the parent control (MainViewModel inside Impulse.Application)
    // rather than by itself, as it will cause unecessary unloads (like when changing the theme currently)
    public void OnClosing()
    {
        Uninitialize();
        HwndInitialized = false;
        Dispose();
    }

    protected abstract void Initialize();

    protected abstract void Uninitialize();

    protected abstract void Resized();

    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        var wndClass = default(NativeMethods.WNDCLASSEX);
        wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
        wndClass.hInstance = NativeMethods.GetModuleHandle(null);
        wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
        wndClass.lpszClassName = WindowClass;
        wndClass.hCursor = NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.IDC_ARROW);

        NativeMethods.RegisterClassEx(ref wndClass);

        Hwnd = NativeMethods.CreateWindowEx(0, WindowClass, string.Empty, NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE, 0, 0, (int)Width, (int)Height, hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, 0);

        return new HandleRef(this, Hwnd);
    }

    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        NativeMethods.DestroyWindow(hwnd.Handle);
        Hwnd = IntPtr.Zero;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        UpdateWindowPos();

        base.OnRenderSizeChanged(sizeInfo);

        if (HwndInitialized)
        {
            Resized();
        }
    }

    protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case NativeMethods.WM_LBUTTONDOWN:
                RaiseMouseEvent(MouseButton.Left, Mouse.MouseDownEvent);
                break;
            case NativeMethods.WM_LBUTTONUP:
                RaiseMouseEvent(MouseButton.Left, Mouse.MouseUpEvent);
                break;
            case NativeMethods.WM_RBUTTONDOWN:
                RaiseMouseEvent(MouseButton.Right, Mouse.MouseDownEvent);
                break;
            case NativeMethods.WM_RBUTTONUP:
                RaiseMouseEvent(MouseButton.Right, Mouse.MouseUpEvent);
                break;
        }

        return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
    }

    private void RaiseMouseEvent(MouseButton button, RoutedEvent @event)
    {
        RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, button)
        {
            RoutedEvent = @event,
            Source = this,
        });
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        Initialize();
        HwndInitialized = true;
        Loaded -= OnLoaded;
    }
}
