using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Impulse.Logging.Contracts;

namespace Impulse.Logging.UI.LogWindow;
/// <summary>
/// Interaction logic for LogWindowView.xaml
/// </summary>
public partial class LogWindowView : UserControl
{
    public static RoutedUICommand CopyDetailsCommand { get; } = new(nameof(CopyDetailsCommand), nameof(CopyDetailsCommand), typeof(LogWindowView));
    public static RoutedUICommand CopyMessageCommand { get; } = new(nameof(CopyMessageCommand), nameof(CopyMessageCommand), typeof(LogWindowView));
    public static RoutedUICommand CopyStackCommand { get; } = new(nameof(CopyStackCommand), nameof(CopyStackCommand), typeof(LogWindowView));
    public static RoutedUICommand RemoveFromViewCommand { get; } = new(nameof(RemoveFromViewCommand), nameof(RemoveFromViewCommand), typeof(LogWindowView));
    public static RoutedUICommand DeleteLogCommand { get; } = new(nameof(DeleteLogCommand), nameof(DeleteLogCommand), typeof(LogWindowView));

    public LogWindowView()
    {
        InitializeComponent();
        CommandBindings.Add(new CommandBinding(CopyDetailsCommand, OnCopyDetailsExecuted, OnLogCommandCanExecute));
        CommandBindings.Add(new CommandBinding(CopyMessageCommand, OnCopyMessageExecuted, OnLogCommandCanExecute));
        CommandBindings.Add(new CommandBinding(CopyStackCommand, OnCopyStackExecuted, OnCopyStackCanExecute));
        CommandBindings.Add(new CommandBinding(RemoveFromViewCommand, OnRemoveFromViewExecuted, OnLogCommandCanExecute));
        CommandBindings.Add(new CommandBinding(DeleteLogCommand, OnDeleteLogExecuted, OnLogCommandCanExecute));
    }

    private void CopyLogButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: LogRecordBase record })
        {
            CopyRecordDetailsToClipboard(record);
        }
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

    private void OnCopyDetailsExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is LogRecordBase record)
        {
            CopyRecordDetailsToClipboard(record);
        }
    }

    private void OnCopyMessageExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is LogRecordBase record && !string.IsNullOrWhiteSpace(record.Message))
        {
            Clipboard.SetText(record.Message);
        }
    }

    private void OnCopyStackExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is ExceptionLogRecord record && !string.IsNullOrWhiteSpace(record.StackTrace))
        {
            Clipboard.SetText(record.StackTrace.TrimEnd());
        }
    }

    private void OnRemoveFromViewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is not LogRecordBase record || DataContext is not LogWindowViewModel viewModel)
        {
            return;
        }

        var targets = GetTargetRecords(record);
        viewModel.RemoveRecords(targets);
    }

    private async void OnDeleteLogExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is not LogRecordBase record || DataContext is not LogWindowViewModel viewModel)
        {
            return;
        }

        var targets = GetTargetRecords(record);
        await viewModel.DeleteRecordsAsync(targets);
    }

    private void OnLogCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
        e.CanExecute = e.Parameter is LogRecordBase;

    private void OnCopyStackCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
        e.CanExecute = e.Parameter is ExceptionLogRecord record && !string.IsNullOrWhiteSpace(record.StackTrace);

    private void LogRecordItem_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem listBoxItem)
        {
            return;
        }

        listBoxItem.Focus();
        if (!listBoxItem.IsSelected)
        {
            listBoxItem.IsSelected = true;
        }
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

    private static void CopyRecordDetailsToClipboard(LogRecordBase record) =>
        Clipboard.SetText(BuildLogText(record));

    private List<LogRecordBase> GetTargetRecords(LogRecordBase primaryRecord)
    {
        if (LogList.SelectedItems is { Count: > 1 } selection && selection.Contains(primaryRecord))
        {
            return selection.OfType<LogRecordBase>().ToList();
        }

        return new List<LogRecordBase> { primaryRecord };
    }
}
