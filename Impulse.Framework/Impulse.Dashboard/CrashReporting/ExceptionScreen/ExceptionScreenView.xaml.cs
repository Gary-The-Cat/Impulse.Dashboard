// <copyright file="ExceptionScreenView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Dashboard.CrashReporting.ExceptionScreen;

/// <summary>
/// Interaction logic for ExceptionScreenView.xaml
/// </summary>
public partial class ExceptionScreenView
{
    public ExceptionScreenView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Close();
    }
}
