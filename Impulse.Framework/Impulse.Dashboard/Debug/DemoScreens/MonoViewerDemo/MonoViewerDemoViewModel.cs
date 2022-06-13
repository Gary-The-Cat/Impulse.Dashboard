// <copyright file="MonoViewerDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Mono.Viewer.ViewerControl;
using Impulse.SharedFramework.Services.Layout;
using Ninject;

namespace Impulse.Dashboard.Debug.DemoScreens.MonoViewerDemo;

public class MonoViewerDemoViewModel : DocumentBase
{
    public MonoViewerDemoViewModel()
    {
        MonoViewerViewModel = new MonoViewerViewModel();

        DisplayName = "Mono Demo";
    }

    public MonoViewerViewModel MonoViewerViewModel { get; set; }

    public void OnResize()
    {
        MonoViewerViewModel.OnResize();
    }
}
