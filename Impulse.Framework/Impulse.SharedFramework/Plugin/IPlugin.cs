namespace Impulse.SharedFramework.Plugin;

using System.Runtime.Versioning;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services;

public interface IPlugin
{
    [RequiresPreviewFeatures]
    public static abstract IPlugin Create(IRibbonService ribbonService, IDocumentService documentService);

    public Task Initialize();

    public Task OnClose();
}