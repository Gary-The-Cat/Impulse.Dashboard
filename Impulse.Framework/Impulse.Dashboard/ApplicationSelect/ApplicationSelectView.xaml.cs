// <copyright file="ApplicationSelectView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Impulse.SharedFramework.Application;

namespace Impulse.Dashboard.ApplicaitonSelect;

/// <summary>
/// Interaction logic for ApplicationSelectView.xaml
/// </summary>
public partial class ApplicationSelectView
{
    public ApplicationSelectView()
    {
        InitializeComponent();
    }

    public ApplicationSelectViewModel ViewModel => (ApplicationSelectViewModel)this.DataContext;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectedApplication = (IApplication)((Button)sender).DataContext;
        Close();
    }
}
