namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;

using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Services.Logging;
using System.Collections.ObjectModel;

internal class LogWindowViewModel : ToolWindowBase
{
    public LogWindowViewModel()
    {
        Placement = ToolWindowPlacement.Bottom;
    }

    public ObservableCollection<LogRecordViewModel> LogRecords { get; set; } =
        new ObservableCollection<LogRecordViewModel>()
        {
            new LogRecordViewModel() { Criticality = Criticality.Info, Message = "Just a test..." },
            new LogRecordViewModel() { Criticality = Criticality.Warning, Message = "Things are breaking." },
            new LogRecordViewModel() { Criticality = Criticality.Error, Message = "Things are baaad!!" },
        };

    public override string DisplayName => "Log Viewer";
}
