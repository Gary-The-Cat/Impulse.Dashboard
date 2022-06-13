// <copyright file="ViewerDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Dashboard.Debug.DemoScreens.ViewerDemo.ViewerToolbar;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.Viewer.ViewerControl;
using Ninject;
using Ninject.Parameters;
using System.Threading;
using System.Threading.Tasks;

namespace Impulse.Dashboard.Debug.DemoScreens.ViewerDemo.ResidentialView;

public class ViewerDemoViewModel : ToolWindowDocumentBase
{
    private ViewerControlViewModel viewerControlViewModel;

    public ViewerDemoViewModel(IKernel kernel) : base(kernel.Get<IToolWindowService>())
    {
        DisplayName = "Viewer";

        ViewerViewModel = kernel.Get<ViewerViewModel>();

        // Create the view for our tool window and attach its DataContext
        viewerControlViewModel = kernel.Get<ViewerControlViewModel>(
            new ConstructorArgument("viewer", ViewerViewModel));

        viewerControlViewModel.DisplayName = "Viewer Control 1";

        // Ask the document service to show the panel
        AddToolWindow(viewerControlViewModel);
    }

    public ViewerViewModel ViewerViewModel { get; private set; }

    public override async Task TryCloseAsync(bool? dialogResult = null)
    {
        await base.TryCloseAsync(dialogResult);

        ViewerViewModel.OnClosing();
    }

    protected async override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        ViewerViewModel.OnSelected();
    }

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        await base.OnDeactivateAsync(close, cancellationToken);
        ViewerViewModel.OnDeselected();
    }
}
