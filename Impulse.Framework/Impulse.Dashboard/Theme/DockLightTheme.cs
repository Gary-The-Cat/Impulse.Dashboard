// <copyright file="DockLightTheme.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Dashboard.Themes
{
    internal class DockLightTheme : AvalonDock.Themes.Theme
    {
        public override Uri GetResourceUri()
        {
            return new Uri(
                "pack://application:,,,/Impulse.Dashboard;component/Theme/Theme.xaml",
                UriKind.Absolute);
        }
    }
}
