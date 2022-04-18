// <copyright file="DialogService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Impulse.Dashboard.CrashReporting.ExceptionScreen;
using Impulse.Dashboard.Shell;
using Impulse.Shared.Enums;
using Impulse.Shared.ExtensionMethods;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Shell;
using Impulse.SharedFramework.ToastNotifications;
using ToastNotifications;
using ToastNotifications.Messages;

namespace Impulse.Dashboard.Services;

public class DialogService : IDialogService
{
    private readonly Notifier notifier;
    private WeakReference<ShellViewModel> shellReference;

    public DialogService(Notifier notifier, IShellViewModel shellViewModel)
    {
        this.notifier = notifier;
        shellReference = new WeakReference<ShellViewModel>((ShellViewModel)shellViewModel);
    }

    private ShellViewModel ShellViewModel => shellReference.Value();

    public void ShowToast(string message, ToastType type)
    {
        switch (type)
        {
            case ToastType.Information:
                notifier.ShowInformation(message);
                break;
            case ToastType.Success:
                notifier.ShowSuccess(message);
                break;
            case ToastType.Warning:
                notifier.ShowWarning(message);
                break;
            case ToastType.Error:
                notifier.ShowError(message);
                break;
        }
    }

    public async Task<DialogResult> ShowDialog(string title, string message)
    {
        return (DialogResult)MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public async Task<DialogResult> ShowWarning(string title, string message)
    {
        return (DialogResult)MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }

    public void ShowError(string title, string message)
    {
        MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    public async Task<DialogResult> ShowDialogWithCancel(string title, string message)
    {
        return (DialogResult)MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.OKCancel,
            MessageBoxImage.Information);
    }

    public async Task<DialogResult> ShowConfirmation(string title, string message)
    {
        return (DialogResult)MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
    }

    public async Task<DialogResult> ShowConfirmationWithCancel(string title, string message)
    {
        return (DialogResult)MessageBox.Show(
            Application.Current.MainWindow,
            message,
            title,
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question);
    }

    public (System.Windows.Forms.DialogResult, string) ShowOpenFileDialog(
        IEnumerable<string> fileExtensions,
        string fileTypes,
        string initialDirectory)
    {
        var openFileDialog = new System.Windows.Forms.OpenFileDialog();
        openFileDialog.Filter = GetFilter(fileTypes, fileExtensions);
        openFileDialog.RestoreDirectory = true;
        openFileDialog.InitialDirectory = initialDirectory;

        return (openFileDialog.ShowDialog(), openFileDialog.FileName);
    }

    public void ShowException(string title, string message)
    {
        var exceptionScreen = new ExceptionScreenView();
        exceptionScreen.ErrorTextBlock.Text = message;
        exceptionScreen.Show();
    }

    public void ShowProgressMessage(string message)
    {
        ShellViewModel.ProgressText = message;
    }

    private string GetFilter(string fileTypes, IEnumerable<string> fileExtensions)
    {
        var output = $"{fileTypes}|";
        foreach (var extension in fileExtensions)
        {
            output += $"*.{extension};";
        }

        return output.Substring(0, output.Length - 1);
    }
}
