// <copyright file="DocumentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Impulse.Dashboard.Shell;
using Impulse.Shared.ExtensionMethods;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;

namespace Impulse.Dashboard.Services
{
    public class DocumentService : IDocumentService
    {
        private WeakReference<ShellViewModel> shellReference;

        public DocumentService(IShellViewModel shell)
        {
            shellReference = new WeakReference<ShellViewModel>((ShellViewModel)shell);
        }

        public ShellViewModel Shell => shellReference.Value();

        public void OpenDocument(DocumentBase document)
        {
            var existingDocument = Shell.Items.FirstOrDefault(i => document.Id == i.Id);

            Shell.ActivateItemAsync(existingDocument ?? document);
        }

        public void CloseDocument(Guid documentId)
        {
            var document = Shell.Items.FirstOrDefault(i => documentId == i.Id);
            Shell.Items.Remove(document);
        }

        public void CloseAllDocuments()
        {
            foreach (var document in Shell.Items.ToList())
            {
                Shell.Items.Remove(document);
            }
        }
    }
}
