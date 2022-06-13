using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Impulse.SharedFramework.Converters;

public class CaseConverter : IValueConverter
{
    public CaseConverter()
    {
        Case = CharacterCasing.Upper;
    }

    public CharacterCasing Case { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value as string;
        if (str != null)
        {
            switch (Case)
            {
                case CharacterCasing.Lower:
                    return str.ToLower();
                case CharacterCasing.Normal:
                    return str;
                case CharacterCasing.Upper:
                    return str.ToUpper();
                default:
                    return str;
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
