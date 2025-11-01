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
            new LogRecordViewModel()
            {
                Timestamp = System.DateTime.Now.AddMinutes(-2),
                Criticality = Criticality.Info,
                Message = "Background sync completed successfully."
            },
            new LogRecordViewModel()
            {
                Timestamp = System.DateTime.Now.AddMinutes(-1),
                Criticality = Criticality.Warning,
                Message = "Importer skipped 3 records because of validation errors."
            },
            new LogRecordViewModel()
            {
                Timestamp = System.DateTime.Now.AddSeconds(-30),
                Criticality = Criticality.Error,
                Message = "Unhandled exception while processing dashboard widgets.",
                StackTrace = @"System.InvalidOperationException: Sequence contains no elements
   at Impulse.Framework.Dashboard.Services.WidgetService.RefreshAsync()
   at Impulse.Framework.Dashboard.Services.Logging.LogWindow.LogWindowViewModel.<.ctor>b__4_0()"
            },
        };

    public override string DisplayName => "Log Viewer";
}
