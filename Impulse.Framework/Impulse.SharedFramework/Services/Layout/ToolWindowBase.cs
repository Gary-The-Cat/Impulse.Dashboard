// <copyright file="ToolWindowBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Reactive;

namespace Impulse.SharedFramework.Services.Layout
{
    public class ToolWindowBase : ReactiveScreen
    {
        public ToolWindowBase()
        {
            Id = Guid.NewGuid();
            Placement = ToolWindowPlacement.Left;
        }

        public ToolWindowPlacement Placement { get; set; }
    }
}
