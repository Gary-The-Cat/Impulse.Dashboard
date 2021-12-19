// <copyright file="TemplatePracticeViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Impulse.SharedFramework.Reactive;

namespace Impulse.Dashboard.Debug.DemoScreens.TemplatePractice
{
    public class TemplatePracticeViewModel : ReactiveScreen
    {
        public TemplatePracticeViewModel()
        {
            Obamas = new ObservableCollection<Obama>();

            for (int i = 0; i < 10; i++)
            {
                Obamas.Add(ObamaFactory.CreateObama());
            }
        }

        public ObservableCollection<Obama> Obamas { get; set; }
    }
}
