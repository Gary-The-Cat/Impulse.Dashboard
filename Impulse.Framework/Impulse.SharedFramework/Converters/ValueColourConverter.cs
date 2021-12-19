using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Impulse.SharedFramework.Converters
{
    public class ValueColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double)value;
            if (doubleValue > 0)
            {
                doubleValue += 0.4;
                doubleValue = Math.Min(doubleValue, 1);
            }

            var g = (byte)(255 * (1 - doubleValue));
            var r = (byte)(255 * doubleValue);
            var newColor = Color.FromRgb(r, g, 10);

            return new SolidColorBrush(newColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
