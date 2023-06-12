// <copyright file="ShellViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using Caliburn.Micro;
using Impulse.Dashboard.Ribbon;
using Impulse.Shared.ReactiveUI;
using Impulse.SharedFramework.Application;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Shell;
using Ninject;
using ReactiveUI;

namespace Impulse.Dashboard.Shell;

public class ShellViewModel : Conductor<ReactiveScreen>.Collection.OneActive, IShellViewModel
{
    public ShellViewModel(IKernel kernel)
    {
        Kernel = kernel;

        Tools = new ObservableCollection<ToolWindowBase>();

        var ribbonService = kernel.Get<IRibbonService>();

        RibbonContent = ((RibbonService)ribbonService).GetRibbonControl();

        this.WhenAnyValue(v => v.ActiveItem).Where(i => i != null).Subscribe(item =>
        {
            // We do not have an active application.
            if (ActiveApplication == null)
            {
                return;
            }
        });
    }

    public UserControl RibbonContent { get; set; }

    public IApplication ActiveApplication { get; set; }

    public ObservableCollection<ToolWindowBase> Tools { get; set; }

    public IEnumerable<DocumentBase> Documents => this.Items.Except(this.Tools).OfType<DocumentBase>();

    public AvalonDock.Themes.Theme Theme { get; set; }

    public IKernel Kernel { get; set; }

    public string ProgressText { get; set; }
}
