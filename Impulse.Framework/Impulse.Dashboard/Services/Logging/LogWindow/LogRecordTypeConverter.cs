namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;

using System;
using System.Globalization;
using System.Windows.Data;
using Impulse.SharedFramework.Services.Logging;

/// <summary>
/// Converts a <see cref="LogRecordBase"/> into a human readable label that reflects what type of record it is.
/// </summary>
public sealed class LogRecordTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            ExceptionLogRecord => "Exception",
            ErrorLogRecord => "Error",
            WarningLogRecord => "Warning",
            InfoLogRecord => "Info",
            LogRecordBase record => record.GetType().Name,
            _ => Binding.DoNothing,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
