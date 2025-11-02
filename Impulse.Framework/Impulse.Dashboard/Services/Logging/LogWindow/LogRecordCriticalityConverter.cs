namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;

using System;
using System.Globalization;
using System.Windows.Data;
using Impulse.SharedFramework.Services.Logging;

public sealed class LogRecordCriticalityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is LogRecordBase record)
        {
            return LogRecordCriticality.Get(record);
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}
