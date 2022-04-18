// <copyright file="RibbonService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Impulse.Shared.ReactiveUI;
using Impulse.SharedFramework.Ribbon;
using Impulse.SharedFramework.Services;
using ReactiveUI;

namespace Impulse.Dashboard.Ribbon;

public class RibbonService : IRibbonService
{
    private readonly Dictionary<string, Fluent.RibbonTabItem> tabLookup;

    private readonly Dictionary<string, Fluent.RibbonGroupBox> groupLookup;

    private readonly Dictionary<string, Fluent.Button> buttonLookup;

    private readonly Dictionary<string, RibbonButton> ribbonButtonLookup;

    private readonly RibbonView ribbonView;

    public RibbonService()
    {
        ribbonView = new RibbonView();
        Ribbon = ribbonView.FluentRibbon;

        tabLookup = new Dictionary<string, Fluent.RibbonTabItem>();
        groupLookup = new Dictionary<string, Fluent.RibbonGroupBox>();
        buttonLookup = new Dictionary<string, Fluent.Button>();

        ribbonButtonLookup = new Dictionary<string, RibbonButton>();
    }

    public Fluent.Ribbon Ribbon { get; }

    public void AddTab(string tabId, string title)
    {
        InsertTab(Ribbon.Tabs.Count(), tabId, title);
    }

    public void InsertTab(int index, string tabId, string title)
    {
        var tab = new Fluent.RibbonTabItem()
        {
            Header = title
        };

        tabLookup.Add(tabId, tab);

        Ribbon.Tabs.Insert(index, tab);

        Ribbon.SelectedTabItem = tab;
    }

    public void AddGroup(string groupId, string title)
    {
        var tabId = GetParentIdFromChildId(groupId);

        if (!tabLookup.TryGetValue(tabId, out var tab))
        {
            throw new Exception($"A tab with Id '{tabId}' could not be found");
        }

        var group = new Fluent.RibbonGroupBox()
        {
            Header = title
        };

        groupLookup.Add(groupId, group);

        tab.Groups.Add(group);
    }

    public void AddButton(RibbonButton ribbonButton)
    {
        var groupId = GetParentIdFromChildId(ribbonButton.Id);

        if (!groupLookup.TryGetValue(groupId, out var group))
        {
            throw new Exception($"A group with Id '{groupId}' could not be found.");
        }

        var template = ribbonView.FindResource("LargeRibbonButton") as ControlTemplate;

        var button = new Fluent.Button()
        {
            Header = ribbonButton.Title,
            Icon = ribbonButton.Icon,
            LargeIcon = ribbonButton.Icon,
            IsEnabled = ribbonButton.IsEnabled,
            Template = template,
        };

        button.DataContext = ribbonButton;

        ribbonButton.WhenAnyValue(b => b.IsEnabled).Subscribe(e =>
        {
            button.IsEnabled = e;
        });
        button.Click += (_, __) => ribbonButton.Callback();

        buttonLookup.Add(ribbonButton.Id, button);
        ribbonButtonLookup.Add(ribbonButton.Id, ribbonButton);

        group.Items.Add(button);
    }

    public void SetButtonContext(string buttonId, ReactiveScreen context, string property)
    {
        var button = ribbonButtonLookup[buttonId];
        button.EnabledPropertyName = property;
        button.Context = context;
    }

    public string GetParentIdFromChildId(string childId)
    {
        var lastIndex = childId.LastIndexOf('.');
        var output = childId.Substring(0, lastIndex);

        return output;
    }

    public UserControl GetRibbonControl()
    {
        return ribbonView;
    }

    public void SetButtonEnabledState(string buttonId, bool isEnabled)
    {
        buttonLookup[buttonId].IsEnabled = isEnabled;
    }
}
