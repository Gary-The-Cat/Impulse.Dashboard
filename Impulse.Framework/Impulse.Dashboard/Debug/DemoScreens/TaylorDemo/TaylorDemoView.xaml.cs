// <copyright file="TaylorDemoView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Windows.Controls;

namespace Impulse.Dashboard.Debug.DemoScreens.TaylorDemo;

/// <summary>
/// Interaction logic for TaylorDemoView.xaml
/// </summary>
public partial class TaylorDemoView : UserControl
{
    public TaylorDemoView()
    {
        InitializeComponent();
    }

    private TaylorDemoViewModel ViewModel => (TaylorDemoViewModel)this.DataContext;

    public void AddDateWeight(DateTime date, float weight)
    {
        if (ViewModel.DateWeights.Any(d => d.Date == date.Date))
        {
            ViewModel.DialogService.ShowError("Oh Shit!", "You can't do that, a weight for that date already exists.");
            return;
        }

        ViewModel.DateWeights.Add(new DateWeight(date.Date, weight));
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        AddDateWeight(ViewModel.CurrentDate, float.Parse(ViewModel.Weight));
    }
}
