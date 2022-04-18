// <copyright file="BindingDemoView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo;

/// <summary>
/// Interaction logic for BindingDemoView.xaml
/// </summary>
public partial class BindingDemoView
{
    public BindingDemoView()
    {
        InitializeComponent();
    }

    public BindingDemoViewModel ViewModel => (BindingDemoViewModel)this.DataContext;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.People.Add(new Person(
            ViewModel.Name,
            ViewModel.Weight,
            ViewModel.Height));
    }
}
