// <copyright file="ProjectExplorerView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.ProjectExplorer
{
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

        private void OnProjectExplorerKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                OnItemSelected();
            }
        }

        private void OnItemSelected()
        {
            var item = treeView.SelectedItems.Cast<ProjectExplorerItemBase>().FirstOrDefault();

            if (item != null)
            {
                ViewModel.OpenItem(item);
            }
        }
    }
}
