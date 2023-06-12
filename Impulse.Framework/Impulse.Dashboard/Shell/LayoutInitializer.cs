// <copyright file="LayoutInitializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using AvalonDock.Layout;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Shell;

internal class LayoutInitializer : ILayoutUpdateStrategy
{
    private bool initialized;
    private LayoutAnchorablePane leftPane;
    private LayoutAnchorablePane rightPane;
    private LayoutAnchorablePane bottomPane;

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
        var toolWindow = anchorableToShow.Content as ToolWindowBase;
        var group = GetGroupFromSide(layout, toolWindow.Placement);
        anchorableToShow.Title = toolWindow.DisplayName;
        group.Children.Add(anchorableToShow);

        return true;
    }

    public static LayoutAnchorGroup GetGroupFromSide(LayoutRoot layout, ToolWindowPlacement placement)
        => placement switch
        {
            ToolWindowPlacement.Left => layout.Children.OfType<LayoutAnchorSide>()
                .First(s => s.Side == AnchorSide.Left)
                .Children.OfType<LayoutAnchorGroup>().First(),
            ToolWindowPlacement.Right => layout.Children.OfType<LayoutAnchorSide>()
                .First(s => s.Side == AnchorSide.Right)
                .Children.OfType<LayoutAnchorGroup>().First(),
            ToolWindowPlacement.Bottom => layout.Children.OfType<LayoutAnchorSide>()
                .First(s => s.Side == AnchorSide.Bottom)
                .Children.OfType<LayoutAnchorGroup>().First(),
        };
}
