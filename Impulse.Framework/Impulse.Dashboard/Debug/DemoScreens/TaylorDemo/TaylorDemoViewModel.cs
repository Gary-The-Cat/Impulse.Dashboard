// <copyright file="TaylorDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Ninject;

namespace Impulse.Dashboard.Debug.DemoScreens.TaylorDemo;

public class TaylorDemoViewModel : DocumentBase
{
    public TaylorDemoViewModel(IDialogService dialogService)
    {
        DisplayName = "Taylor Demo";

        DateWeights = new ObservableCollection<DateWeight>()
        {
            new DateWeight(DateTime.Now.Date.AddDays(-5), 66.35f),
            new DateWeight(DateTime.Now.Date.AddDays(-4), 65.35f),
            new DateWeight(DateTime.Now.Date.AddDays(-3), 64.35f),
            new DateWeight(DateTime.Now.Date.AddDays(-2), 63.35f),
            new DateWeight(DateTime.Now.Date.AddDays(-1), 62.35f),
        };

        DialogService = dialogService;
    }

    public IDialogService DialogService { get; set; }

    public ObservableCollection<DateWeight> DateWeights { get; set; }

    public string Weight { get; set; }

    public DateTime CurrentDate => DateTime.Now.Date;

    public string CurrentDateFormatted => DateTime.Now.Date.ToShortDateString();
}
