using System.Text;
using System.Windows;
using System.Windows.Controls;

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
        if (sender is not FrameworkElement { DataContext: LogRecordViewModel record })
        {
            return;
        }

        var builder = new StringBuilder();
        var timestamp = record.Timestamp == default ? "n/a" : record.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

        builder.AppendLine($"[{timestamp}] {record.Criticality}");
        builder.AppendLine(record.Message ?? string.Empty);

        if (!string.IsNullOrWhiteSpace(record.StackTrace))
        {
            builder.AppendLine();
            builder.AppendLine(record.StackTrace);
        }

        Clipboard.SetText(builder.ToString());
    }
}
