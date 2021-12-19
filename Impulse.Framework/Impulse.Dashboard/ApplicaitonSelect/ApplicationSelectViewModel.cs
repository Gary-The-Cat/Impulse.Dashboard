// <copyright file="ApplicationSelectViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Impulse.SharedFramework.Application;
using Impulse.SharedFramework.Reactive;

namespace Impulse.Dashboard.ApplicaitonSelect
{
    public class ApplicationSelectViewModel : ReactiveScreen
    {
        public ApplicationSelectViewModel(IEnumerable<IApplication> applications)
        {
            DisplayName = "Select Application";

            Applications = new ObservableCollection<IApplication>(applications.ToList());
        }

        public ObservableCollection<IApplication> Applications { get; set; }

        public IApplication SelectedApplication { get; set; }

        public void Close()
        {
            this.TryClose(true);
        }
    }
}
