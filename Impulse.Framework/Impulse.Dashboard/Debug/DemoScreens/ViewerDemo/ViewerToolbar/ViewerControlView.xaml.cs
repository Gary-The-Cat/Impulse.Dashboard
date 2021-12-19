// <copyright file="ViewerControlView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Impulse.Dashboard.Debug.DemoScreens.ViewerDemo.ViewerToolbar
{
    /// <summary>
    /// Interaction logic for ViewerControlView.xaml
    /// </summary>
    public partial class ViewerControlView : UserControl
    {
        public ViewerControlView()
        {
            InitializeComponent();
        }

        private ViewerControlViewModel ViewModel => (ViewerControlViewModel)this.DataContext;

        private void NewViewer_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadNewObjFile();
        }
    }
}
