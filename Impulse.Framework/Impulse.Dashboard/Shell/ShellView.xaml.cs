// <copyright file="ShellView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using AvalonDock;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;

namespace Impulse.Dashboard.Shell;

public partial class ShellView : IShellView
{
    public ShellView()
    {
        SourceInitialized += Window_SourceInitialized;

        InitializeComponent();
    }

    public ShellViewModel ViewModel => (ShellViewModel)this.DataContext;

    private void Dock_DocumentClosing(object sender, DocumentClosingEventArgs e)
    {
        if (e.Document.Content is DocumentBase document)
        {
            e.Cancel = true;

            document.TryCloseAsync();
        }
    }

    /// <summary>
    /// TitleBar_MouseDown - Drag if single-click, resize if double-click
    /// </summary>
    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left)
        {
            return;
        }

        if (e.ClickCount == 2)
        {
            AdjustWindowSize();
            return;
        }

        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
        }

        this.DragMove();
    }

    /// <summary>
    /// CloseButton_Clicked
    /// </summary>
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    /// <summary>
    /// MaximizedButton_Clicked
    /// </summary>
    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        AdjustWindowSize();
    }

    /// <summary>
    /// Minimized Button_Clicked
    /// </summary>
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    /// <summary>
    /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
    /// </summary>
    private void AdjustWindowSize()
    {
        this.WindowState = this.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void Window_SourceInitialized(object sender, EventArgs e)
    {
        IntPtr handle = new WindowInteropHelper(this).Handle;
        HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
    }

    private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case 0x0024:
                Native.WmGetMinMaxInfo(hwnd, lParam, (int)MinWidth, (int)MinHeight);
                handled = true;
                break;
        }

        return (IntPtr)0;
    }

    private void ButtonExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
