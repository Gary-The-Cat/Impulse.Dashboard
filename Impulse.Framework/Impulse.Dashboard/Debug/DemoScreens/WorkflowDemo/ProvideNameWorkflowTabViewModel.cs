// <copyright file="ProvideNameWorkflowTabViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Attributes;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Debug.DemoScreens.WorkflowDemo;

public class ProvideNameWorkflowTabViewModel : WorkflowTabBase
{
    public override string DisplayName => "Provide Name";

    public override string Descrption => Name;

    [ExportProperty]
    public string Name { get; set; }
}
