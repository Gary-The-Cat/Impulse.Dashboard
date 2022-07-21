namespace Impulse.SharedFramework.Plugin;

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services;

public interface IPlugin
{
    [RequiresPreviewFeatures]
    public static abstract IPlugin Create(IRibbonService ribbonService, IDocumentService documentService);

    IEnumerable<Type> GetRequiredServices();

    public Task Initialize();

    public Task OnClose();
}