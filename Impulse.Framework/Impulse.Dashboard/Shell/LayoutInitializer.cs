// <copyright file="LayoutInitializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AvalonDock.Layout;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Shell;

internal class LayoutInitializer : ILayoutUpdateStrategy
{
    public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
    {
    }

    public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
    {
        return anchorableToShow.Content is ToolWindowBase;
    }

    public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
    {
    }

    public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
    {
        if (anchorableToShow.Content is not ToolWindowBase toolWindow)
        {
            return false;
        }

        if (destinationContainer != null && IsAutoHideDestination(destinationContainer))
        {
            // Let AvalonDock handle auto-hide transitions.
            return false;
        }

        var targetPane = GetPane(layout, toolWindow.Placement);
        if (targetPane == null)
        {
            return false;
        }

        anchorableToShow.Title = toolWindow.DisplayName;

        if (!targetPane.Children.Contains(anchorableToShow))
        {
            targetPane.Children.Add(anchorableToShow);
        }

        return true;
    }

    internal static LayoutAnchorablePane? GetPane(LayoutRoot layout, ToolWindowPlacement placement)
    {
        var (leftPane, rightPane, bottomPane) = LocatePanes(layout);

        return placement switch
        {
            ToolWindowPlacement.Left => leftPane,
            ToolWindowPlacement.Right => rightPane,
            ToolWindowPlacement.Bottom => bottomPane,
            _ => null,
        };
    }

    private static (LayoutAnchorablePane? left, LayoutAnchorablePane? right, LayoutAnchorablePane? bottom) LocatePanes(LayoutRoot layout)
    {
        if (layout.RootPanel is not LayoutPanel rootPanel)
        {
            return (null, null, null);
        }

        var horizontalPanel = rootPanel.Children.OfType<LayoutPanel>()
            .FirstOrDefault(panel => panel.Orientation == Orientation.Horizontal);

        var anchorablePanes = horizontalPanel?.Children.OfType<LayoutAnchorablePane>().ToList() ?? new();

        var leftPane = anchorablePanes.ElementAtOrDefault(0);
        var rightPane = anchorablePanes.Count > 1
            ? anchorablePanes.ElementAt(anchorablePanes.Count - 1)
            : anchorablePanes.ElementAtOrDefault(0);

        var bottomPane = rootPanel.Children.OfType<LayoutAnchorablePane>()
            .FirstOrDefault(pane => pane.DockHeight.Value > 0);

        return (leftPane, rightPane, bottomPane);
    }

    private static bool IsAutoHideDestination(ILayoutContainer destinationContainer)
    {
        if (destinationContainer is not ILayoutElement element)
        {
            return false;
        }

        while (element != null)
        {
            if (element is LayoutAnchorSide)
            {
                return true;
            }

            element = element.Parent;
        }

        return false;
    }
}
