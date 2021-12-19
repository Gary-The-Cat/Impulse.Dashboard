// <copyright file="Person.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using Impulse.Shared.ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo
{
    public class Person : ReactiveViewModelBase
    {
        public Person(string name, float weight, float height)
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public string BMI => (Weight / Math.Pow(Height / 100.0, 2)).ToString("#.00");
    }
}
