using Impulse.Shared.Application;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using Impulse.Solvle.SolvleUI;
using Ninject;

namespace Impulse.Solvle;
public class SolvleApplication : IApplication
{
    private IKernel kernel;

    public SolvleApplication(IKernel kernel)
    {
        this.kernel = kernel;
    }

    public string DisplayName => "Solvle";

    public Uri Icon => new Uri(
        "pack://application:,,,/Impulse.Dashboard;component/Icons/Export/Bat.png",
        UriKind.Absolute);

    public async Task Initialize()
    {
        var ribbonService = this.kernel.Get<IRibbonService>();
        
        var taylorDemo = new RibbonButtonViewModel
        {
            Title = "Solvle",
            Id = "Developer.Debug.Taylor.Solvle",
            EnabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Candy.png",
            DisabledIcon = "pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Candy_GS.png",
            IsEnabled = true,
            Callback = OpenSolvleDemo,
        };

        ribbonService.AddButton(taylorDemo);
    }

    private void OpenSolvleDemo()
    {
        var sfmlDemo = kernel.Get<SolvleViewModel>();
        var documentService = kernel.Get<IDocumentService>();

        documentService.OpenDocument(sfmlDemo);
    }

    public async Task LaunchApplication()
    {
    }

    public async Task OnClose()
    {
    }
}
