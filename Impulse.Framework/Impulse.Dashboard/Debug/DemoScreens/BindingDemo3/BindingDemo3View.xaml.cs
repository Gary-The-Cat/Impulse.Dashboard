// <copyright file="BindingDemo3View.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;
using System.Windows;
using Impulse.SharedFramework.Services;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo3;

/// <summary>
/// Interaction logic for BindingDemo3View.xaml
/// </summary>
public partial class BindingDemo3View
{
    private readonly IDialogService dialogService;

    public BindingDemo3View(IDialogService dialogService)
    {
        InitializeComponent();
        this.dialogService = dialogService;
    }

    private BindingDemo3ViewModel ViewModel => (BindingDemo3ViewModel)this.DataContext;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var firstPerson = ViewModel.People.First();

        dialogService.ShowToast(
            $"{firstPerson.Name} has a weight of: {firstPerson.Weight}",
            Impulse.SharedFramework.ToastNotifications.ToastType.Information);
    }
}
