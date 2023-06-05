// <copyright file="ViewerDemoViewModel.cs" company="Tutorials With Gary">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Services.Layout;
using Impulse.Viewer.Mono.ViewerControl;

namespace Impulse.Framework.Dashboard.Demonstrations;
internal class ViewerDemoViewModel : DocumentBase
{
    public override string DisplayName => "Mono Demo";

    public MonoViewerViewModel MonoViewerViewModel { get; set; } = new MonoViewerViewModel();
}
