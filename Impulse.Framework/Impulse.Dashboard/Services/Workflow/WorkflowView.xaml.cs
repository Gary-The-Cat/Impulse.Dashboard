// <copyright file="WorkflowView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Impulse.Dashboard.Services.Workflow.WorkflowTabBadge;

namespace Impulse.Dashboard.Services.Workflow;

/// <summary>
/// Interaction logic for WorkflowView.xaml
/// </summary>
public partial class WorkflowView
{
    public WorkflowView()
    {
        InitializeComponent();
    }

    private WorkflowViewModel ViewModel => (WorkflowViewModel)this.DataContext;

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var tabs = ViewModel.Items.ToList();

        var grid = WorkflowProgressGrid;

        for (int i = 0; i < tabs.Count(); i++)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinition);
        }

        for (int i = 0; i < tabs.Count(); i++)
        {
            var item = new WorkflowTabBadgeView();
            var dataContext = new WorkflowTabBadgeViewModel() { DisplayName = tabs[i].DisplayName };
            item.DataContext = dataContext;

            // If a column does not exist, this creates a new column with width '*'
            Grid.SetColumn(item, i);

            // All of our centre columns should be centre aligned.
            item.HorizontalAlignment = HorizontalAlignment.Center;

            // Add the badge to the view model to update on tab through
            ViewModel.Badges.Add(dataContext);

            // Add the badge to the grid
            grid.Children.Add(item);
        }

        grid = WorkflowProgressBarGrid;

        for (int i = 0; i < tabs.Count() - 1; i++)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(columnDefinition);
        }

        Window_SizeChanged(null, null);
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (WorkflowProgressGrid.Children.Count == 0)
        {
            return;
        }

        var badge = WorkflowProgressGrid.Children[0];

        Point relativeLocation = badge.TranslatePoint(new Point(0, 0), WorkflowProgressGrid);

        WorkflowProgressBarGrid.Margin = new Thickness(relativeLocation.X, 0, relativeLocation.X, 0);
    }
}
