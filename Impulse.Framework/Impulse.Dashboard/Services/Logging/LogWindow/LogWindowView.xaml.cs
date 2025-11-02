using System.Text;
using System.Windows;
using System.Windows.Controls;
using Impulse.SharedFramework.Services.Logging;

namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;
/// <summary>
/// Interaction logic for LogWindowView.xaml
/// </summary>
public partial class LogWindowView : UserControl
{
    public LogWindowView()
    {
        InitializeComponent();
    }

    private void CopyLogButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: LogRecordBase record })
        {
            return;
        }

        Clipboard.SetText(BuildLogText(record));
    }

    private void CopyVisibleButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not LogWindowViewModel viewModel)
        {
            return;
        }

        var records = viewModel.GetVisibleRecords();
        if (records == null || records.Count == 0)
        {
            return;
        }

        var builder = new StringBuilder();
        for (var i = 0; i < records.Count; i++)
        {
            builder.Append(BuildLogText(records[i]));

            if (i < records.Count - 1)
            {
                builder.AppendLine();
                builder.AppendLine();
            }
        }

        Clipboard.SetText(builder.ToString());
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is LogWindowViewModel viewModel)
        {
            viewModel.ClearLogs();
        }
    }

    private static string BuildLogText(LogRecordBase record)
    {
        var builder = new StringBuilder();
        var timestamp = record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
        var severity = GetSeverityLabel(record);

        builder.AppendLine($"[{timestamp}] {severity}");
        builder.AppendLine(record.Message);

        if (record is ExceptionLogRecord exceptionRecord)
        {
            if (!string.IsNullOrWhiteSpace(exceptionRecord.ExceptionType))
            {
                builder.AppendLine($"Type: {exceptionRecord.ExceptionType}");
            }

            if (!string.IsNullOrWhiteSpace(exceptionRecord.ExceptionMessage))
            {
                builder.AppendLine($"Message: {exceptionRecord.ExceptionMessage}");
            }

            if (!string.IsNullOrWhiteSpace(exceptionRecord.StackTrace))
            {
                builder.AppendLine("Stack Trace:");
                builder.AppendLine(exceptionRecord.StackTrace.TrimEnd());
            }
        }

        return builder.ToString().TrimEnd();
    }

    private static string GetSeverityLabel(LogRecordBase record) =>
        record switch
        {
            ExceptionLogRecord => "Exception",
            ErrorLogRecord => "Error",
            WarningLogRecord => "Warning",
            InfoLogRecord => "Info",
            _ => LogRecordCriticality.Get(record).ToString(),
        };
}
