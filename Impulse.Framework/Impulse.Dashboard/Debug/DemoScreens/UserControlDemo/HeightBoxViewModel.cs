// <copyright file="HeightBoxViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Reactive;

namespace Impulse.Dashboard.Debug.DemoScreens.UserControlDemo
{
    public class HeightBoxViewModel : ReactiveViewModelBase
    {
        public double HeightValue { get; set; }

        public string Name { get; set; }
    }
}
