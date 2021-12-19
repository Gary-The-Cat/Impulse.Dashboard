// <copyright file="ViewerView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.SharedFramework.Viewer;

namespace Impulse.Viewer.ViewerControl
{
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
}
