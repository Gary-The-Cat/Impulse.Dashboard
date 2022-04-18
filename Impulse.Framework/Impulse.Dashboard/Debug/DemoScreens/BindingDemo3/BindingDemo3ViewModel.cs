// <copyright file="BindingDemo3ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Impulse.Dashboard.Debug.DemoScreens.BindingDemo;
using Impulse.SharedFramework.Services.Layout;
using Ninject;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo3;

public class BindingDemo3ViewModel : DocumentBase
{
    public BindingDemo3ViewModel(IKernel kernel) : base(kernel)
    {
        DisplayName = "Taylors second binding demo";

        People = new ObservableCollection<Person>()
        {
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
            new Person("Amos", 110, 185),
            new Person("Holden", 85, 178),
            new Person("Naomi", 65, 165),
            new Person("Alex", 87, 180),
        };
    }

    public ObservableCollection<Person> People { get; set; }

    public Person SelectedPerson { get; set; }

    public string Name { get; set; }

    public float Weight { get; set; }

    public float Height { get; set; }
}
