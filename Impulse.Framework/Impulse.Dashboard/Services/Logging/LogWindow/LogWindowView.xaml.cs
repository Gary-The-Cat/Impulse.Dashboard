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

        var builder = new StringBuilder();
        var timestamp = record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
        var severity = LogRecordCriticality.Get(record);

        builder.AppendLine($"[{timestamp}] {severity}");
        builder.AppendLine(record.Message);

        if (record is ExceptionLogRecord exceptionRecord)
        {
            if (!string.IsNullOrWhiteSpace(exceptionRecord.ExceptionType))
            {
                builder.AppendLine(exceptionRecord.ExceptionType);
            }

            if (!string.IsNullOrWhiteSpace(exceptionRecord.ExceptionMessage))
            {
                builder.AppendLine(exceptionRecord.ExceptionMessage);
            }

            if (!string.IsNullOrWhiteSpace(exceptionRecord.StackTrace))
            {
                builder.AppendLine();
                builder.AppendLine(exceptionRecord.StackTrace);
            }
        }

        Clipboard.SetText(builder.ToString());
    }
}
