// <copyright file="IPlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services;

namespace Impulse.SharedFramework.Application;

public interface IPlugin
{
    [RequiresPreviewFeatures]
    public static abstract IPlugin Create(IRibbonService ribbonService, IDocumentService documentService);

    IEnumerable<Type> GetRequiredServices();

    public Task Initialize();

    public Task OnClose();
}