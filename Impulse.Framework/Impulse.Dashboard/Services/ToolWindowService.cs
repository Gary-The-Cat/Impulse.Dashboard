// <copyright file="ToolWindowService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Dashboard.Shell;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;

namespace Impulse.Dashboard.Services
{
    public class ToolWindowService : IToolWindowService
    {
        private readonly ShellViewModel shell;

        public ToolWindowService(IShellViewModel shell)
        {
            this.shell = (ShellViewModel)shell;
        }

        public void HidePaneToolWindow(ToolWindowBase toolWindow)
        {
            shell.Tools.Remove(toolWindow);
        }

        public void OpenLeftPaneToolWindow(ToolWindowBase toolWindow)
        {
            toolWindow.Placement = Impulse.Shared.Enums.ToolWindowPlacement.Left;
            shell.Tools.Add(toolWindow);
        }

        public void OpenRightPaneToolWindow(ToolWindowBase toolWindow)
        {
            toolWindow.Placement = Impulse.Shared.Enums.ToolWindowPlacement.Right;
            if (!shell.Tools.Contains(toolWindow))
            {
                shell.Tools.Add(toolWindow);
            }
        }
    }
}
