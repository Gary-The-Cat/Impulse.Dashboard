// <copyright file="BindingDemo2ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Impulse.Dashboard.Debug.DemoScreens.BindingDemo;
using Impulse.SharedFramework.Reactive;

namespace Impulse.Dashboard.Debug.DemoScreens.BindingDemo2
{
    public class BindingDemo2ViewModel : ReactiveScreen
    {
        public BindingDemo2ViewModel()
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

            Cars = new ObservableCollection<Car>()
            {
                new Car("Luke", "Lukes Bug", "Volkswagon", 1990),
                new Car("Taylor", "Taylors Bug", "Volkswagon", 1990),
                new Car("Luke", "Lukes Barina", "Holden", 2010),
                new Car("Taylor", "Taylors Barina", "Holden", 1988)
            };
        }

        public ObservableCollection<Person> People { get; set; }

        public ObservableCollection<Car> Cars { get; set; }

        public IEnumerable<string> CarMakes => Cars.Select(c => c.Manufacturer).Distinct();

        public Person SelectedPerson { get; set; }

        public string SelectedMake { get; set; }

        public string Name { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }
    }
}
