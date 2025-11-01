// <copyright file="ToolWindowService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Dashboard.Shell;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;

namespace Impulse.Dashboard.Services;

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
        OpenToolWindow(toolWindow, Impulse.Shared.Enums.ToolWindowPlacement.Left);
    }

    public void OpenRightPaneToolWindow(ToolWindowBase toolWindow)
    {
        OpenToolWindow(toolWindow, Impulse.Shared.Enums.ToolWindowPlacement.Right);
    }

    public void OpenBottomPaneToolWindow(ToolWindowBase toolWindow)
    {
        OpenToolWindow(toolWindow, Impulse.Shared.Enums.ToolWindowPlacement.Bottom);
    }

    private void OpenToolWindow(ToolWindowBase toolWindow, Impulse.Shared.Enums.ToolWindowPlacement placement)
    {
        var toolIsOpen = shell.Tools.Contains(toolWindow);

        if (toolIsOpen && toolWindow.Placement == placement)
        {
            return;
        }

        if (toolIsOpen)
        {
            shell.Tools.Remove(toolWindow);
        }

        toolWindow.Placement = placement;
        shell.Tools.Add(toolWindow);
    }
}
