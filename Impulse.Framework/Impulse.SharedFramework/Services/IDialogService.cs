// <copyright file="IDialogService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.ToastNotifications;

namespace Impulse.SharedFramework.Services
{
    public interface IDialogService
    {
        public void ShowToast(string message, ToastType type);

        public Task<DialogResult> ShowDialog(string title, string message);

        public Task<DialogResult> ShowWarning(string title, string message);

        public void ShowError(string title, string message);

        public void ShowProgressMessage(string message);

        public void ShowException(string title, string message);

        public Task<DialogResult> ShowDialogWithCancel(string title, string message);

        public Task<DialogResult> ShowConfirmation(string title, string message);

        public Task<DialogResult> ShowConfirmationWithCancel(string title, string message);

        public (System.Windows.Forms.DialogResult, string) ShowOpenFileDialog(
            IEnumerable<string> fileExtensions,
            string fileTypes,
            string initialDirectory);
    }
}
