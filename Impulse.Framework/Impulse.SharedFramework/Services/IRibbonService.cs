// <copyright file="IRibbonService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows.Controls;
using Impulse.Shared.ReactiveUI;
using Impulse.SharedFramework.Ribbon;

namespace Impulse.SharedFramework.Services;

public interface IRibbonService
{
    UserControl GetRibbonControl();

    // Example: ApplicationName.Home
    void AddTab(string tabId, string title);

    // Example: (0, "ApplicationName.Home", "Home")
    void InsertTab(int index, string tabId, string title);

    // Example: ApplicationName.Home.Project
    void AddGroup(string groupId, string title);

    // Example: ApplicationName.Home.Project.NewProject
    void AddButton(RibbonButton button);

    void SetButtonEnabledState(string buttonId, bool isEnabled);

    void SetButtonContext(string buttonId, ReactiveScreen context, string property);
}
