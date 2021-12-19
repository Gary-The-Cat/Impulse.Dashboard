// <copyright file="BindingDemo2View.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo2
{
    /// <summary>
    /// Interaction logic for BindingDemo2View.xaml
    /// </summary>
    public partial class BindingDemo2View
    {
        public BindingDemo2View()
        {
            InitializeComponent();
        }

        private BindingDemo2ViewModel ViewModel => (BindingDemo2ViewModel)this.DataContext;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Cars.Add(new BindingDemo.Car(ViewModel.Name, string.Empty, ViewModel.SelectedMake, 0));
        }
    }
}
