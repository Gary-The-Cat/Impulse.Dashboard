// <copyright file="ProjectExplorerView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.ProjectExplorer;

/// <summary>
/// Interaction logic for ProjectExplorerView.xaml
/// </summary>
public partial class ProjectExplorerView : UserControl
{
    public ProjectExplorerView()
    {
        InitializeComponent();
    }

    public ProjectExplorerViewModel ViewModel => (ProjectExplorerViewModel)this.DataContext;

    private void OnProjectExplorerItemSelected(object sender, MouseButtonEventArgs e)
    {
        if (!(e.Source is MultiSelectTreeViewItem item)
            || !treeView.SelectedItems.Contains(item.DataContext))
        {
            return;
        }

        e.Handled = true;

        OnItemSelected();
    }

    private async void OnProjectExplorerKeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.FocusedElement is TextBox editingTextBox &&
            editingTextBox.DataContext is ProjectExplorerItemBase editingItem &&
            editingItem.IsEditing)
        {
            // Allow the in-place editor to handle key presses such as Enter, Escape, Delete, etc.
            e.Handled = false;
            return;
        }

        if (e.Key == Key.Return || e.Key == Key.Enter)
        {
            e.Handled = true;
            OnItemSelected();
            return;
        }

        if (e.Key == Key.F2)
        {
            var item = treeView.SelectedItems.Cast<ProjectExplorerItemBase>().FirstOrDefault();

            if (item != null)
            {
                e.Handled = true;
                await ViewModel.BeginRenameAsync(item);
                return;
            }
        }

        if (e.Key == Key.Delete)
        {
            var items = treeView.SelectedItems.Cast<ProjectExplorerItemBase>().ToList();

            if (items.Any())
            {
                e.Handled = true;
                await ViewModel.DeleteItemsAsync(items);
                return;
            }
        }

        e.Handled = false;
    }

    private void OnItemSelected()
    {
        var item = treeView.SelectedItems.Cast<ProjectExplorerItemBase>().FirstOrDefault();

        if (item != null)
        {
            ViewModel.OpenItem(item);
        }
    }

    private void OnEditTextBoxLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        if (textBox.DataContext is ProjectExplorerItemBase item && item.IsEditing)
        {
            textBox.Focus();
            textBox.SelectAll();
        }
    }

    private void OnEditTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        if (textBox.DataContext is ProjectExplorerItemBase item)
        {
            CommitRename(item);
        }
    }

    private void OnEditTextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not TextBox textBox)
        {
            return;
        }

        if (textBox.Visibility != Visibility.Visible)
        {
            return;
        }

        if (textBox.DataContext is ProjectExplorerItemBase item && item.IsEditing)
        {
            textBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                textBox.Focus();
                textBox.SelectAll();
            }));
        }
    }

    private void OnEditTextBoxKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox || textBox.DataContext is not ProjectExplorerItemBase item)
        {
            return;
        }

        if (e.Key == Key.Enter || e.Key == Key.Return)
        {
            e.Handled = true;
            CommitRename(item);
        }
        else if (e.Key == Key.Escape)
        {
            e.Handled = true;
            item.DisplayName = item.OriginalDisplayName;
            item.IsEditing = false;
        }
    }

    private void CommitRename(ProjectExplorerItemBase item)
    {
        if (!item.IsEditing)
        {
            return;
        }

        if (ViewModel == null)
        {
            return;
        }

        ViewModel.CommitRename(item, item.DisplayName ?? string.Empty);
    }
}
