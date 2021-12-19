// <copyright file="Car.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo
{
    public class Car
    {
        public Car(string owner, string name, string manufacturer, int yearOfRelease)
        {
            Owner = owner;
            Name = name;
            Manufacturer = manufacturer;
            YearOfRelease = yearOfRelease;
        }

        public string Owner { get; set; }

        public string Name { get; set; }

        public int YearOfRelease { get; set; }

        public string Manufacturer { get; set; }
    }
}