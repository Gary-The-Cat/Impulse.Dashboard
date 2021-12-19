// <copyright file="IDocumentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.Services
{
    public interface IDocumentService
    {
        void OpenDocument(DocumentBase document);

        void CloseDocument(Guid documentId);

        void CloseAllDocuments();
    }
}
