// <copyright file="LayoutInitializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using AvalonDock.Layout;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Shell
{
    internal class LayoutInitializer : ILayoutUpdateStrategy
    {
        private bool initialized;
        private LayoutAnchorablePane leftPane;
        private LayoutAnchorablePane rightPane;

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            // Create the left and right panes
            if (!initialized)
            {
                (leftPane, rightPane) = GetPanes();
                initialized = true;
            }

            // Get a reference to the root layout panel to add our tool windows
            var panel = layout.Descendents().OfType<LayoutPanel>().First();

            // Add the panels to their appropriate positions
            if (!panel.Descendents().Contains(leftPane))
            {
                // Insert at the start (horizontal orientated)
                panel.Children.Insert(0, leftPane);
            }

            if (!panel.Descendents().Contains(rightPane))
            {
                // Add to the end (horizontal orientated)
                panel.Children.Add(rightPane);
            }

            // Get the pane we want to insert our anchorable into
            var pane = GetOrCreatePane(((ToolWindowBase)anchorableToShow.Content).Placement);
            if (pane != null)
            {
                pane.Children.Add(anchorableToShow);

                // We have added the visual to the correct pane, mark it as done.
                return true;
            }

            // We were unable to find the correct parent pane, so use the default behaviour.
            return false;
        }

        private (LayoutAnchorablePane leftPane, LayoutAnchorablePane rightPane) GetPanes()
        {
            var leftPane = new LayoutAnchorablePane() { DockWidth = new GridLength(200) };
            var rightPane = new LayoutAnchorablePane() { DockWidth = new GridLength(200) };

            return (leftPane, rightPane);
        }

        private LayoutAnchorablePane GetOrCreatePane(ToolWindowPlacement placement)
        {
            return placement == ToolWindowPlacement.Left ? leftPane : rightPane;
        }
    }
}
