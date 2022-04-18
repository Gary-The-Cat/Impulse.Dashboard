// <copyright file="ViewerView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Viewer.ViewerControl;

using System;
using Impulse.SharedFramework.Viewer;

public class ViewerView : Win32HwndControl
{
    private ViewerViewModel ViewModel => (ViewerViewModel)this.DataContext;

    protected override sealed void Initialize()
    {
        ViewModel.Initialize();
    }

    protected sealed override void Resized()
    {
        ViewModel.OnResize();
    }

    protected override sealed void Uninitialize()
    {
    }

    protected override void Render(IntPtr windowHandle)
    {
        ViewModel.Render();
    }
}
