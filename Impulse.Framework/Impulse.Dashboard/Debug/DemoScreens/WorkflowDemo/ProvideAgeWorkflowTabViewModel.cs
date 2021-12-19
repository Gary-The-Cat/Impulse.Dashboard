// <copyright file="ProvideAgeWorkflowTabViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Attributes;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Debug.DemoScreens.WorkflowDemo
{
    public class ProvideAgeWorkflowTabViewModel : WorkflowTabBase
    {
        public override string DisplayName => "Provide Age";

        public override string Descrption => $"Age: {Age}";

        [ExportProperty]
        public int Age { get; set; }
    }
}
