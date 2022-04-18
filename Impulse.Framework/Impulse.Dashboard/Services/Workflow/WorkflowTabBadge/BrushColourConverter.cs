// <copyright file="BrushColourConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Impulse.Dashboard.Services.Workflow.WorkflowTabBadge;

public class BrushColourConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var selected = Application.Current.TryFindResource("PrimaryAccentBrush");
        var deselected = Application.Current.TryFindResource("BackgroundBrush");
        if ((bool)value)
        {
            {
                return selected;
            }
        }

        return deselected;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
