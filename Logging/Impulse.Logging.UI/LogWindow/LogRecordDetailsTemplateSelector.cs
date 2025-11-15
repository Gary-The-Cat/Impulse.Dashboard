namespace Impulse.Logging.UI.LogWindow;

using System;
using System.Windows;
using System.Windows.Controls;
using Impulse.Logging.Contracts;

public sealed class LogRecordDetailsTemplateSelector : DataTemplateSelector
{
    public DataTemplate? DefaultTemplate { get; set; }

    public DataTemplate? ExceptionTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is ExceptionLogRecord && ExceptionTemplate != null)
        {
            return ExceptionTemplate;
        }

        return DefaultTemplate ?? base.SelectTemplate(item, container);
    }
}
