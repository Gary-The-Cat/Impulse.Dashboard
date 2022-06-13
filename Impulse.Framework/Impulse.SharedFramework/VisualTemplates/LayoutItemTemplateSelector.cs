// <copyright file="LayoutItemTemplateSelector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Controls;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.SharedFramework.VisualTemplates;

public class LayoutItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate Template { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is ToolWindowDocumentBase)
        {
            return Template;
        }

        return base.SelectTemplate(item, container);
    }
}
