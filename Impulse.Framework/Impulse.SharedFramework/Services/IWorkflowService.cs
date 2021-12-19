// <copyright file="IWorkflowService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.Services
{
    public interface IWorkflowService
    {
        public IWorkflowViewModel CreateWorkflow(params WorkflowTabBase[] workflowTabs);

        public void ShowWorkflow(IWorkflowViewModel workflow);
    }
}
