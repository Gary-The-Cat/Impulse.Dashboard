// <copyright file="ApplicationSelectViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Impulse.Shared.Application;
using Impulse.Shared.ReactiveUI;

namespace Impulse.Dashboard.ApplicaitonSelect;

public class ApplicationSelectViewModel : ReactiveScreen
{
    public ApplicationSelectViewModel(IEnumerable<IApplication> applications)
    {
        DisplayName = "Select Application";

        Applications = new ObservableCollection<IApplication>(applications.ToList());
    }

    public ObservableCollection<IApplication> Applications { get; set; }

    public IApplication SelectedApplication { get; set; }

    public async void Close()
    {
        this.TryCloseAsync(true);
    }
}
