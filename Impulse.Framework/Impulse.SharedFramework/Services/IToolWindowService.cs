// <copyright file="IToolWindowService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.Services
{
    public interface IToolWindowService
    {
        void OpenLeftPaneToolWindow(ToolWindowBase toolWindow);

        void OpenRightPaneToolWindow(ToolWindowBase toolWindow);

        void HidePaneToolWindow(ToolWindowBase toolWindow);
    }
}
