// <copyright file="WorkflowService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Ninject;
using Ninject.Parameters;

namespace Impulse.Dashboard.Services.Workflow
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IKernel kernel;

        public WorkflowService(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IWorkflowViewModel CreateWorkflow(params WorkflowTabBase[] workflowTabs)
        {
            return kernel.Get<WorkflowViewModel>(
                new ConstructorArgument("workflowTabs", workflowTabs));
        }

        public void ShowWorkflow(IWorkflowViewModel workflow)
        {
            var internalWorkflow = (WorkflowViewModel)workflow;

            var windowService = this.kernel.Get<IWindowManager>();

            windowService.ShowDialogAsync(internalWorkflow);
        }
    }
}
