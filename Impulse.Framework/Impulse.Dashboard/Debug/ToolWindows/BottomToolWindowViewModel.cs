namespace Impulse.Dashboard.Debug.ToolWindows;

using System;
using System.Collections.ObjectModel;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;

internal class BottomToolWindowViewModel : ToolWindowBase
{
    public BottomToolWindowViewModel()
    {
        DisplayName = "Bottom Debug Tool";
        Placement = ToolWindowPlacement.Bottom;
        Messages = new ObservableCollection<string>
        {
            "This tool window lives in the bottom pane.",
            $"Opened at {DateTime.Now:T}",
        };
    }

    public ObservableCollection<string> Messages { get; }
}
